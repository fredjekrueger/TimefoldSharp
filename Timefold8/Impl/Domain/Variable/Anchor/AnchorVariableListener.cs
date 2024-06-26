using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Anchor
{
    public class AnchorVariableListener : VariableListener<object>, AnchorVariableSupply
    {

        protected AnchorShadowVariableDescriptor anchorShadowVariableDescriptor;
        protected VariableDescriptor previousVariableDescriptor;
        protected SingletonInverseVariableSupply nextVariableSupply;

        public AnchorVariableListener(AnchorShadowVariableDescriptor anchorShadowVariableDescriptor,
            VariableDescriptor previousVariableDescriptor,
            SingletonInverseVariableSupply nextVariableSupply)
        {
            this.anchorShadowVariableDescriptor = anchorShadowVariableDescriptor;
            this.previousVariableDescriptor = previousVariableDescriptor;
            this.nextVariableSupply = nextVariableSupply;
        }

        public void AfterVariableChanged(ScoreDirector scoreDirector, object entity)
        {
            Insert((InnerScoreDirector)scoreDirector, entity);
        }

        public void BeforeVariableChanged(ScoreDirector scoreDirector, object entity)
        {
            // No need to retract() because the insert (which is guaranteed to be called later) affects the same trailing entities.
        }

        public void Dispose()
        {
        }

        public object GetAnchor(object entity)
        {
            throw new NotImplementedException();
        }

        public bool RequiresUniqueEntityEvents()
        {
            return false;
        }

        protected void Insert(InnerScoreDirector scoreDirector, object entity)
        {
            object previousEntity = previousVariableDescriptor.GetValue(entity);
            object anchor;
            if (previousEntity == null)
            {
                anchor = null;
            }
            else if (previousVariableDescriptor.IsValuePotentialAnchor(previousEntity))
            {
                anchor = previousEntity;
            }
            else
            {
                anchor = anchorShadowVariableDescriptor.GetValue(previousEntity);
            }
            object nextEntity = entity;
            while (nextEntity != null && anchorShadowVariableDescriptor.GetValue(nextEntity) != anchor)
            {
                scoreDirector.BeforeVariableChanged(anchorShadowVariableDescriptor, nextEntity);
                anchorShadowVariableDescriptor.SetValue(nextEntity, anchor);
                scoreDirector.AfterVariableChanged(anchorShadowVariableDescriptor, nextEntity);
                nextEntity = nextVariableSupply.GetInverseSingleton(nextEntity);
            }
        }

        public void ResetWorkingSolution(ScoreDirector scoreDirector)
        {
        }
    }
}
