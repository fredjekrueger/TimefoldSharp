using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic;

namespace TimefoldSharp.Core.Impl
{
    public abstract class AbstractFromConfigFactory<Config_>
        where Config_ : AbstractConfig<Config_>
    {

        protected readonly Config_ config;

        public static EntitySelectorConfig GetDefaultEntitySelectorConfigForEntity(HeuristicConfigPolicy configPolicy, EntityDescriptor entityDescriptor)
        {
            Type entityClass = entityDescriptor.EntityClass;
            EntitySelectorConfig entitySelectorConfig = new EntitySelectorConfig()
                    .WithId(entityClass.Name)
                    .WithEntityClass(entityClass);
            if (EntitySelectorConfig.HasSorter(configPolicy.BuilderInfo.EntitySorterManner, entityDescriptor))
            {
                throw new NotImplementedException();
                /*entitySelectorConfig = entitySelectorConfig.withCacheType(SelectionCacheType.PHASE)
                        .withSelectionOrder(SelectionOrder.SORTED)
                        .withSorterManner(configPolicy.getEntitySorterManner());*/
            }
            return entitySelectorConfig;
        }

        protected GenuineVariableDescriptor DeduceGenuineVariableDescriptor(EntityDescriptor entityDescriptor, string variableName)
        {
            return variableName == null ? GetTheOnlyVariableDescriptor(entityDescriptor) : GetVariableDescriptorForName(entityDescriptor, variableName);
        }

        protected GenuineVariableDescriptor GetVariableDescriptorForName(EntityDescriptor entityDescriptor,
           string variableName)
        {
            GenuineVariableDescriptor variableDescriptor = entityDescriptor.GetGenuineVariableDescriptor(variableName);
            if (variableDescriptor == null)
            {
                throw new Exception("The config (" + config + ") has a variableName (" + variableName + ") which is not a valid planning variable on entityClass");
            }
            return variableDescriptor;
        }

        protected GenuineVariableDescriptor GetTheOnlyVariableDescriptor(EntityDescriptor entityDescriptor)
        {
            List<GenuineVariableDescriptor> variableDescriptorList = entityDescriptor.GetGenuineVariableDescriptorList();
            if (variableDescriptorList.Count != 1)
            {
                throw new Exception("The config (" + config + ") has no configured variableName for entityClas it cannot be deduced automatically.");
            }
            return variableDescriptorList.GetEnumerator().Current; //hier stond next
        }

        protected EntityDescriptor DeduceEntityDescriptor(HeuristicConfigPolicy configPolicy, Type entityClass)
        {
            SolutionDescriptor solutionDescriptor = configPolicy.BuilderInfo.SolutionDescriptor;
            return entityClass == null ? GetTheOnlyEntityDescriptor(solutionDescriptor) : GetEntityDescriptorForClass(solutionDescriptor, entityClass);
        }

        private EntityDescriptor GetEntityDescriptorForClass(SolutionDescriptor solutionDescriptor, Type entityClass)
        {
            EntityDescriptor entityDescriptor = solutionDescriptor.GetEntityDescriptorStrict(entityClass);
            if (entityDescriptor == null)
            {
                throw new Exception("The confi mplementation's annotated methods too.");
            }
            return entityDescriptor;
        }

        protected EntityDescriptor GetTheOnlyEntityDescriptor(SolutionDescriptor solutionDescriptor)
        {
            List<EntityDescriptor> entityDescriptors = solutionDescriptor.GetGenuineEntityDescriptors();
            if (entityDescriptors.Count != 1)
            {
                throw new Exception("The config (" + config
                        + ") has no ent it cannot be deduced automatically.");
            }
            return entityDescriptors.First();
        }

        public AbstractFromConfigFactory(Config_ config)
        {
            this.config = config;
        }
    }
}
