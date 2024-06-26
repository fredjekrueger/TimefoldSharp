using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Listener;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation
{
    public class ExternalizedSingletonInverseVariableSupply : SourcedVariableListener, VariableListener<object>,SingletonInverseVariableSupply
    {

        protected Dictionary<object, object> inverseEntityMap = null;

        protected VariableDescriptor sourceVariableDescriptor;

        public ExternalizedSingletonInverseVariableSupply(VariableDescriptor sourceVariableDescriptor)
        {
            this.sourceVariableDescriptor = sourceVariableDescriptor;
        }

        public void AfterVariableChanged(ScoreDirector scoreDirector, object entity)
        {
            Insert(entity);
        }

        public void BeforeVariableChanged(ScoreDirector scoreDirector, object entity)
        {
            Retract(entity);
        }

        public void Dispose()
        {
            inverseEntityMap = null;
        }

        public object GetInverseSingleton(object value)
        {
            inverseEntityMap.TryGetValue(value, out object returnValue);
            return returnValue;
        }

        public VariableDescriptor GetSourceVariableDescriptor()
        {
            return sourceVariableDescriptor;
        }

        public bool RequiresUniqueEntityEvents()
        {
            return false;
        }

        public void ResetWorkingSolution(ScoreDirector scoreDirector)
        {
            inverseEntityMap = new Dictionary<object, object>();
            sourceVariableDescriptor.EntityDescriptor.VisitAllEntities(scoreDirector.GetWorkingSolution(), Insert);
        }


        static List<object> retracts = new List<object>();
        static List<object> inserts = new List<object>();

        protected void Insert(object entity)
        {
            object value = sourceVariableDescriptor.GetValue(entity);
            if (value == null)
            {
                return;
            }
            inserts.Add(value); //JDEF ZEKER WEGSMIJTEN
            inverseEntityMap.TryGetValue(value, out object oldInverseEntity);
            
            if (oldInverseEntity != null)
            {
                throw new Exception("The supply (" + this + ") is corrupted,"
                        + " because the entity (" + entity
                        + ") for sourceVariable (" + sourceVariableDescriptor.GetVariableName()
                        + ") cannot be inserted: another entity (" + oldInverseEntity
                        + ") already has that value (" + value + ").");
            }
            inverseEntityMap.Add(value, entity);
        }

        protected void Retract(object entity)
        {

            object value = sourceVariableDescriptor.GetValue(entity);
            if (value == null)
            {
                return;
            }
            retracts.Add(value); //JDEF ZEKER WEGSMIJTEN
            object oldInverseEntity = inverseEntityMap[value];

            if (oldInverseEntity != entity)
            {
                throw new Exception("The supply (" + this + ") is corrupted,"
                        + " because the entity (" + entity
                        + ") for sourceVariable (" + sourceVariableDescriptor.GetVariableName()
                        + ") cannot be retracted: the entity was never inserted for that value (" + value + ").");
            }
            inverseEntityMap.Remove(value);
        }
    }
}
