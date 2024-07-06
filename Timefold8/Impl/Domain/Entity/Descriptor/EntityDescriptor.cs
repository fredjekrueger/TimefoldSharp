using System.Reflection;
using TimefoldSharp.Core.API.Domain.Entity;
using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Policy;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Anchor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Decorator;
using TimefoldSharp.Core.Impl.Score.Director;
using static TimefoldSharp.Core.Impl.Domain.Common.Accessor.MemberAccessorFactory;

namespace TimefoldSharp.Core.Impl.Domain.Entity.Descriptor
{
    public class EntityDescriptor
    {

        private static readonly Type[] VARIABLE_ANNOTATION_CLASSES = {
            typeof(PlanningVariableAttribute),
            typeof(PlanningListVariableAttribute),
            typeof(InverseRelationShadowVariableAttribute),
            typeof(AnchorShadowVariableAttribute),
            typeof(IndexShadowVariableAttribute),
            typeof(PreviousElementShadowVariableAttribute),
            typeof(NextElementShadowVariableAttribute),
            typeof(ShadowVariableAttribute),
            //typeof(ShadowVariable.List),
            typeof(PiggybackShadowVariableAttribute),
            typeof(CustomShadowVariableAttribute) };

        private readonly SolutionDescriptor solutionDescriptor;
        private List<GenuineVariableDescriptor> effectiveGenuineVariableDescriptorList;
        private Func<object, bool> hasNoNullVariables;
        private List<EntityDescriptor> inheritedEntityDescriptorList;

        private Dictionary<string, GenuineVariableDescriptor> declaredGenuineVariableDescriptorMap;
        private Dictionary<string, GenuineVariableDescriptor> effectiveGenuineVariableDescriptorMap;
        private Dictionary<string, ShadowVariableDescriptor> declaredShadowVariableDescriptorMap;
        private Dictionary<string, ShadowVariableDescriptor> effectiveShadowVariableDescriptorMap;
        private Dictionary<string, VariableDescriptor> effectiveVariableDescriptorMap;
        private List<SelectionFilter<object>> declaredPinEntityFilterList;

        private SelectionFilter<object> effectiveMovableEntitySelectionFilter;
        private SelectionFilter<object> declaredMovableEntitySelectionFilter;


        public EntityDescriptor(SolutionDescriptor solutionDescriptor, Type entityClass)
        {
            this.solutionDescriptor = solutionDescriptor;
            this.EntityClass = entityClass;
            hasNoNullVariables = HasNoNullVariables;
        }

        public List<Object> ExtractEntities(ISolution solution)
        {
            List<Object> entityList = new List<object>();
            VisitAllEntities(solution, (a) => entityList.Add(a));
            return entityList;
        }

        public void VisitAllEntities(ISolution solution, Action<Object> visitor)
        {
            solutionDescriptor.VisitEntitiesByEntityClass(solution, EntityClass, visitor);
        }

        public VariableDescriptor GetVariableDescriptor(string variableName)
        {
            return effectiveVariableDescriptorMap[variableName];
        }

        public GenuineVariableDescriptor GetGenuineVariableDescriptor(string variableName)
        {
            effectiveGenuineVariableDescriptorMap.TryGetValue(variableName, out GenuineVariableDescriptor res);
            return res;
        }

        public List<GenuineVariableDescriptor> GetGenuineVariableDescriptorList()
        {
            return effectiveGenuineVariableDescriptorList;
        }

        public bool HasEffectiveMovableEntitySelectionFilter()
        {
            return effectiveMovableEntitySelectionFilter != null;
        }

        public IEnumerable<GenuineVariableDescriptor> GetDeclaredGenuineVariableDescriptors()
        {
            return declaredGenuineVariableDescriptorMap.Values;
        }

        public void LinkVariableDescriptors(DescriptorPolicy descriptorPolicy)
        {
            foreach (var variableDescriptor in declaredGenuineVariableDescriptorMap.Values)
            {
                variableDescriptor.LinkVariableDescriptors(descriptorPolicy);
            }
            foreach (var shadowVariableDescriptor in declaredShadowVariableDescriptorMap.Values)
            {
                shadowVariableDescriptor.LinkVariableDescriptors(descriptorPolicy);
            }
        }

        private void InvestigateParentsToLinkInherited(Type investigateClass)
        {
            inheritedEntityDescriptorList = new List<EntityDescriptor>(4);
            if (investigateClass == null || investigateClass.IsArray)
            {
                return;
            }
            LinkInherited(investigateClass);
            foreach (var superInterface in investigateClass.GetInterfaces())
            {
                LinkInherited(superInterface);
            }
        }

        private void LinkInherited(Type potentialEntityClass)
        {
            EntityDescriptor entityDescriptor = solutionDescriptor.GetEntityDescriptorStrict(potentialEntityClass);
            if (entityDescriptor != null)
            {
                inheritedEntityDescriptorList.Add(entityDescriptor);
            }
            else
            {
                InvestigateParentsToLinkInherited(potentialEntityClass);
            }
        }

        public SolutionDescriptor GetSolutionDescriptor()
        {
            return solutionDescriptor;
        }

        public static void PutAll<TKey, TValue>(IDictionary<TKey, TValue> dic, IDictionary<TKey, TValue> dicToAdd)
        {
            ForEach(dicToAdd, x => dic[x.Key] = x.Value);
        }

        public static void PutAllDer<TKey, TValue, TDerivedValue>(IDictionary<TKey, TValue> dic, IDictionary<TKey, TDerivedValue> dicToAdd) where TDerivedValue : TValue
        {
            ForEach(dicToAdd, x => dic[x.Key] = (TValue)x.Value);
        }

        public static void ForEach<T>(IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        private void CreateEffectiveVariableDescriptorMaps()
        {
            effectiveGenuineVariableDescriptorMap = new Dictionary<string, GenuineVariableDescriptor>(declaredGenuineVariableDescriptorMap.Count);
            effectiveShadowVariableDescriptorMap = new Dictionary<string, ShadowVariableDescriptor>(declaredShadowVariableDescriptorMap.Count);
            foreach (var inheritedEntityDescriptor in inheritedEntityDescriptorList)
            {
                PutAll(effectiveGenuineVariableDescriptorMap, inheritedEntityDescriptor.effectiveGenuineVariableDescriptorMap);
                PutAll(effectiveShadowVariableDescriptorMap, inheritedEntityDescriptor.effectiveShadowVariableDescriptorMap);
            }
            PutAll(effectiveGenuineVariableDescriptorMap, declaredGenuineVariableDescriptorMap);
            PutAll(effectiveShadowVariableDescriptorMap, declaredShadowVariableDescriptorMap);
            effectiveVariableDescriptorMap = new Dictionary<string, VariableDescriptor>(effectiveGenuineVariableDescriptorMap.Count + effectiveShadowVariableDescriptorMap.Count);
            PutAllDer(effectiveVariableDescriptorMap, effectiveGenuineVariableDescriptorMap);
            PutAllDer(effectiveVariableDescriptorMap, effectiveShadowVariableDescriptorMap);
            effectiveGenuineVariableDescriptorList = new List<GenuineVariableDescriptor>(effectiveGenuineVariableDescriptorMap.Values);
        }

        public bool HasAnyDeclaredGenuineVariableDescriptor()
        {
            return declaredGenuineVariableDescriptorMap.Count > 0;
        }


        private void CreateEffectiveMovableEntitySelectionFilter()
        {
            if (declaredMovableEntitySelectionFilter != null && !HasAnyDeclaredGenuineVariableDescriptor())
            {
                throw new Exception("The entityClass), but it has no declared genuine variables, only shadow variables.");
            }
            List<SelectionFilter<object>> selectionFilterList = new List<SelectionFilter<object>>();
            // TODO Also add in child entity selectors
            foreach (var inheritedEntityDescriptor in inheritedEntityDescriptorList)
            {
                if (inheritedEntityDescriptor.HasEffectiveMovableEntitySelectionFilter())
                {
                    // Includes movable and pinned
                    selectionFilterList.Add(inheritedEntityDescriptor.effectiveMovableEntitySelectionFilter);
                }
            }
            if (declaredMovableEntitySelectionFilter != null)
            {
                selectionFilterList.Add(declaredMovableEntitySelectionFilter);
            }
            selectionFilterList.AddRange(declaredPinEntityFilterList);
            if (selectionFilterList.Count == 0)
            {
                effectiveMovableEntitySelectionFilter = null;
            }
            else
            {
                effectiveMovableEntitySelectionFilter = SelectionFilter<object>.Compose(selectionFilterList);
            }
        }

        public void LinkEntityDescriptors(DescriptorPolicy descriptorPolicy)
        {
            InvestigateParentsToLinkInherited(EntityClass);
            CreateEffectiveVariableDescriptorMaps();
            CreateEffectiveMovableEntitySelectionFilter();
        }

        public bool HasAnyGenuineVariables()
        {
            return effectiveGenuineVariableDescriptorMap.Count > 0;
        }

        public bool HasNoNullVariables(Object entity)
        {
            foreach (var variableDescriptor in effectiveGenuineVariableDescriptorList)
            {
                if (variableDescriptor.GetValue(entity) == null)
                {
                    return false;
                }
            }
            return true;
        }

        public Func<A, bool> GetHasNoNullVariables<A>()
        {
            return new Func<A, bool>(a => hasNoNullVariables(a));
        }


        public bool IsInitialized(Object entity)
        {
            foreach (var variableDescriptor in effectiveGenuineVariableDescriptorList)
            {
                if (!variableDescriptor.IsInitialized(entity))
                {
                    return false;
                }
            }
            return true;
        }


        public Type EntityClass { get; set; }


        public void ProcessAnnotations(DescriptorPolicy descriptorPolicy)
        {
            ProcessEntityAnnotations(descriptorPolicy);
            declaredGenuineVariableDescriptorMap = new Dictionary<string, GenuineVariableDescriptor>();
            declaredShadowVariableDescriptorMap = new Dictionary<string, ShadowVariableDescriptor>();
            declaredPinEntityFilterList = new List<SelectionFilter<object>>(2);
            // Only iterate declared fields and methods, not inherited members, to avoid registering the same one twice
            List<MemberInfo> memberList = ConfigUtils.GetDeclaredMembers(EntityClass);
            foreach (var member in memberList)
            {
                ProcessValueRangeProviderAnnotation(descriptorPolicy, member);
                ProcessPlanningVariableAnnotation(descriptorPolicy, member);
                ProcessPlanningPinAnnotation(descriptorPolicy, member);
            }
            if (declaredGenuineVariableDescriptorMap.Count == 0 && declaredShadowVariableDescriptorMap.Count == 0)
            {
                throw new Exception("The entityClass (" + EntityClass
                        + ") should have at least 1 getter method or 1 field with  annotation or a shadow variable annotation.");
            }
            ProcessVariableAnnotations(descriptorPolicy);
        }

        private void ProcessPlanningPinAnnotation(DescriptorPolicy descriptorPolicy, MemberInfo member)
        {
            if (Attribute.IsDefined(member, typeof(PlanningPinAttribute)))
            {
                MemberAccessor memberAccessor = descriptorPolicy.MemberAccessorFactory.BuildAndCacheMemberAccessor(EntityClass, member,
                        MemberAccessorType.PROPERTY_OR_READ_METHOD, typeof(PlanningPinAttribute));
                var type = memberAccessor.GetClass();
                if (!(type == typeof(bool)))
                {
                    throw new Exception("The entityClass (" + EntityClass
                            + ") has a ) that is not a boolean or Boolean.");
                }
                declaredPinEntityFilterList.Add(new PinEntityFilter(memberAccessor));
            }
        }

        private void ProcessPlanningVariableAnnotation(DescriptorPolicy descriptorPolicy, MemberInfo member)
        {
            var variableAnnotationClass = ConfigUtils.ExtractAnnotationClass(member, VARIABLE_ANNOTATION_CLASSES);
            if (variableAnnotationClass != null)
            {
                MemberAccessorFactory.MemberAccessorType memberAccessorType;
                if (variableAnnotationClass == typeof(CustomShadowVariableAttribute)
                    || variableAnnotationClass == typeof(ShadowVariableAttribute)
                    // || variableAnnotationClass == typeof(ShadowVariable.List)
                    || variableAnnotationClass == typeof(PiggybackShadowVariableAttribute))
                {
                    memberAccessorType = MemberAccessorType.PROPERTY_OR_GETTER_METHOD;
                }
                else
                {
                    memberAccessorType = MemberAccessorType.PROPERTY_OR_GETTER_METHOD_WITH_SETTER;
                }
                MemberAccessor memberAccessor = descriptorPolicy.MemberAccessorFactory.BuildAndCacheMemberAccessor(EntityClass, member,
                        memberAccessorType, variableAnnotationClass);
                RegisterVariableAccessor(variableAnnotationClass, memberAccessor);
            }
        }

        private void ProcessValueRangeProviderAnnotation(DescriptorPolicy descriptorPolicy, MemberInfo member)
        {
            if (Attribute.IsDefined(member, typeof(ValueRangeProviderAttribute)))
            {
                MemberAccessor memberAccessor = descriptorPolicy.MemberAccessorFactory.BuildAndCacheMemberAccessor(EntityClass, member,
                        MemberAccessorType.PROPERTY_OR_READ_METHOD, typeof(ValueRangeProviderAttribute));
                descriptorPolicy.AddFromEntityValueRangeProvider(memberAccessor);
            }
        }

        private void ProcessVariableAnnotations(DescriptorPolicy descriptorPolicy)
        {
            foreach (var variableDescriptor in declaredGenuineVariableDescriptorMap.Values)
            {
                variableDescriptor.ProcessAnnotations(descriptorPolicy);
            }
            foreach (var variableDescriptor in declaredShadowVariableDescriptorMap.Values)
            {
                variableDescriptor.ProcessAnnotations(descriptorPolicy);
            }
        }

        private void ProcessMovable(DescriptorPolicy descriptorPolicy, PlanningEntityAttribute entityAnnotation)
        {
            Type pinningFilterClass = entityAnnotation.PinningFilter;
            bool hasPinningFilter = pinningFilterClass != typeof(NullPinningFilter);
            if (hasPinningFilter)
            {
                IPinningFilter pinningFilter = ConfigUtils.NewInstance<IPinningFilter>(pinningFilterClass);
                declaredMovableEntitySelectionFilter = new SelectionFilter<object>();
                declaredMovableEntitySelectionFilter.Accept = (scoreDirector, selection) => !pinningFilter.Accept(scoreDirector.GetWorkingSolution(), selection);
            }
        }

        private void ProcessDifficulty(DescriptorPolicy descriptorPolicy, PlanningEntityAttribute entityAnnotation)
        {
            Type difficultyComparatorClass = entityAnnotation.DifficultyComparatorClass;
            if (difficultyComparatorClass == typeof(NullDifficultyComparator))
            {
                difficultyComparatorClass = null;
            }
            Type difficultyWeightFactoryClass = entityAnnotation.DifficultyWeightFactoryClass;
            if (difficultyWeightFactoryClass == typeof(NullDifficultyWeightFactory))
            {
                difficultyWeightFactoryClass = null;
            }
            if (difficultyComparatorClass != null && difficultyWeightFactoryClass != null)
            {
                throw new Exception("The entityClass (" + EntityClass
                        + ") cannot have a difficultyComparatorClass (" + difficultyComparatorClass.Name
                        + ") and a difficultyWeightFactoryClass (" + difficultyWeightFactoryClass.Name
                        + ") at the same time.");
            }
            if (difficultyComparatorClass != null)
            {
                //var difficultyComparator = ConfigUtils.NewInstance("difficultyComparatorClass", difficultyComparatorClass);
                //decreasingDifficultySorter = new ComparatorSelectionSorter<object>(difficultyComparator, SelectionSorterOrder.DESCENDING);
            }
            if (difficultyWeightFactoryClass != null)
            {
                //var difficultyWeightFactory = ConfigUtils.NewInstance("difficultyWeightFactoryClass", difficultyWeightFactoryClass);
                //decreasingDifficultySorter = new WeightFactorySelectionSorter<object>(difficultyWeightFactory, SelectionSorterOrder.DESCENDING);
            }
        }

        private SelectionSorter<object> decreasingDifficultySorter;

        public SelectionSorter<object> GetDecreasingDifficultySorter()
        {
            return decreasingDifficultySorter;
        }

        private void ProcessEntityAnnotations(DescriptorPolicy descriptorPolicy)
        {
            PlanningEntityAttribute entityAnnotation = (PlanningEntityAttribute)Attribute.GetCustomAttribute(EntityClass, typeof(PlanningEntityAttribute));
            if (entityAnnotation == null)
            {
                throw new Exception("The entityClass annotation.");
            }
            ProcessMovable(descriptorPolicy, entityAnnotation);
            ProcessDifficulty(descriptorPolicy, entityAnnotation);
        }

        private void RegisterVariableAccessor(Type variableAnnotationClass, MemberAccessor memberAccessor)
        {
            String memberName = memberAccessor.GetName();
            if (declaredGenuineVariableDescriptorMap.ContainsKey(memberName)
                    || declaredShadowVariableDescriptorMap.ContainsKey(memberName))
            {
                VariableDescriptor duplicate = declaredGenuineVariableDescriptorMap[memberName];
                if (duplicate == null)
                {
                    duplicate = declaredShadowVariableDescriptorMap[memberName];
                }
                throw new Exception("The entityClass (" + EntityClass
                        + ") has a @" + variableAnnotationClass.Name
                        + " annotated member (" + memberAccessor
                        + ") that is duplicated by another member for variableDescriptor (" + duplicate + ").\n"
                        + "Maybe the annotation is defined on both the field and its getter.");
            }
            if (variableAnnotationClass == typeof(PlanningVariableAttribute))
            {
                GenuineVariableDescriptor variableDescriptor = new BasicVariableDescriptor(this, memberAccessor);
                declaredGenuineVariableDescriptorMap.Add(memberName, variableDescriptor);
            }
            else if (variableAnnotationClass == typeof(PlanningListVariableAttribute))
            {
                if (typeof(List<>).IsAssignableFrom(memberAccessor.GetClass()))
                {
                    throw new NotImplementedException();
                    //GenuineVariableDescriptor<Solution_> variableDescriptor = new ListVariableDescriptor<>(this, memberAccessor);
                    //declaredGenuineVariableDescriptorMap.Add(memberName, variableDescriptor);
                }
                else
                {
                    throw new Exception("The entityClass (" + EntityClass + ") has a @");
                }
            }
            else if (variableAnnotationClass == typeof(InverseRelationShadowVariableAttribute))
            {
                ShadowVariableDescriptor variableDescriptor = new InverseRelationShadowVariableDescriptor(this, memberAccessor);
                declaredShadowVariableDescriptorMap.Add(memberName, variableDescriptor);
            }
            else if (variableAnnotationClass == typeof(AnchorShadowVariableAttribute))
            {
                ShadowVariableDescriptor variableDescriptor = new AnchorShadowVariableDescriptor(this, memberAccessor);
                declaredShadowVariableDescriptorMap.Add(memberName, variableDescriptor);
            }
            else if (variableAnnotationClass == typeof(IndexShadowVariableAttribute))
            {
                throw new NotImplementedException();
                //ShadowVariableDescriptor<Solution_> variableDescriptor = new IndexShadowVariableDescriptor<>(this, memberAccessor);
                //declaredShadowVariableDescriptorMap.Add(memberName, variableDescriptor);
            }
            else if (variableAnnotationClass == typeof(PreviousElementShadowVariableAttribute))
            {
                throw new NotImplementedException();
                //PreviousElementShadowVariableDescriptor<Solution_> variableDescriptor = new PreviousElementShadowVariableDescriptor<>(this, memberAccessor);
                //declaredShadowVariableDescriptorMap.Add(memberName, variableDescriptor);
            }
            else if (variableAnnotationClass == typeof(NextElementShadowVariableAttribute))
            {
                throw new NotImplementedException();
                //NextElementShadowVariableDescriptor<Solution_> variableDescriptor = new NextElementShadowVariableDescriptor<>(this, memberAccessor);
                //declaredShadowVariableDescriptorMap.Add(memberName, variableDescriptor);
            }
            else if (variableAnnotationClass == typeof(ShadowVariableAttribute))
            {
                throw new NotImplementedException();
                //|| variableAnnotationClass == typeof(ShadowVariable.List.class)) {
                //ShadowVariableDescriptor<Solution_> variableDescriptor = new CustomShadowVariableDescriptor<>(this, memberAccessor);
                //declaredShadowVariableDescriptorMap.Add(memberName, variableDescriptor);
            }
            else if (variableAnnotationClass == typeof(PiggybackShadowVariableAttribute))
            {
                throw new NotImplementedException();
                //ShadowVariableDescriptor<Solution_> variableDescriptor = new PiggybackShadowVariableDescriptor<>(this, memberAccessor);
                //declaredShadowVariableDescriptorMap.Add(memberName, variableDescriptor);
            }
            else if (variableAnnotationClass == typeof(CustomShadowVariableAttribute))
            {
                throw new NotImplementedException();
                //ShadowVariableDescriptor<Solution_> variableDescriptor = new LegacyCustomShadowVariableDescriptor<>(this, memberAccessor);
                //declaredShadowVariableDescriptorMap.Add(memberName, variableDescriptor);
            }
            else
            {
                throw new Exception("The variableAnnotationClass ("
                        + variableAnnotationClass + ") is not implemented.");
            }
        }

        public IEnumerable<ShadowVariableDescriptor> GetDeclaredShadowVariableDescriptors()
        {
            return declaredShadowVariableDescriptorMap.Values;
        }

        public IEnumerable<VariableDescriptor> GetDeclaredVariableDescriptors()
        {
            var variableDescriptors = new List<VariableDescriptor>(declaredGenuineVariableDescriptorMap.Count + declaredShadowVariableDescriptorMap.Count);
            variableDescriptors.AddRange(declaredGenuineVariableDescriptorMap.Values.OfType<VariableDescriptor>());
            variableDescriptors.AddRange(declaredShadowVariableDescriptorMap.Values.OfType<VariableDescriptor>());
            return variableDescriptors;
        }

        public bool IsMovable(ScoreDirector scoreDirector, object entity)
        {
            return effectiveMovableEntitySelectionFilter == null || effectiveMovableEntitySelectionFilter.Accept(scoreDirector, entity);
        }

        public SelectionFilter<object> GetEffectiveMovableEntitySelectionFilter()
        {
            return effectiveMovableEntitySelectionFilter;
        }

        public int CountUninitializedVariables(Object entity)
        {
            int count = 0;
            foreach (var variableDescriptor in effectiveGenuineVariableDescriptorList)
            {
                if (!variableDescriptor.IsInitialized(entity))
                {
                    count++;
                }
            }
            return count;
        }

        public bool MatchesEntity(object entity)
        {
            return EntityClass.IsAssignableFrom(entity.GetType());
        }
    }
}