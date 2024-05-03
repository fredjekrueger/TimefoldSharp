using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Reflection;
using TimefoldSharp.Core.API.Domain.Common;
using TimefoldSharp.Core.API.Domain.ConstraintWeight;
using TimefoldSharp.Core.API.Domain.Entity;
using TimefoldSharp.Core.API.Domain.Solution;
using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Impl.Domain.Common;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.ConstraintWeight.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Lookup;
using TimefoldSharp.Core.Impl.Domain.Policy;
using TimefoldSharp.Core.Impl.Domain.Score.Definition;
using TimefoldSharp.Core.Impl.Domain.Score.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Solution.Cloner;
using TimefoldSharp.Core.Impl.Domain.Solution.Cloner.Gizmo;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Score.Director;
using TimefoldSharp.Core.Impl.Util;
using static TimefoldSharp.Core.Impl.Domain.Common.Accessor.MemberAccessorFactory;

namespace TimefoldSharp.Core.Impl.Domain.Solution.Descriptor
{
    public class SolutionDescriptor
    {
        private readonly Dictionary<Type, EntityDescriptor> entityDescriptorMap = new Dictionary<Type, EntityDescriptor>();
        private ScoreDescriptor scoreDescriptor;

        public bool AssertModelForCloning { get; set; }
        private HashSet<Type> problemFactOrEntityClassSet;

        public List<ListVariableDescriptor> ListVariableDescriptors { get; set; }
        public MemberAccessorFactory MemberAccessorFactory { get; set; }
        private List<Type> reversedEntityClassList = new List<Type>();
        private ConcurrentDictionary<Type, EntityDescriptor> lowestEntityDescriptorMap = new ConcurrentDictionary<Type, EntityDescriptor>();
        private static readonly EntityDescriptor NULL_ENTITY_DESCRIPTOR = new EntityDescriptor(null, typeof(PlanningEntityAttribute));
        private readonly Dictionary<string, MemberAccessor> problemFactMemberAccessorMap = new Dictionary<string, MemberAccessor>();
        private readonly Dictionary<string, MemberAccessor> problemFactCollectionMemberAccessorMap = new Dictionary<string, MemberAccessor>();
        private readonly Dictionary<string, MemberAccessor> entityMemberAccessorMap = new Dictionary<string, MemberAccessor>();
        private SolutionCloner solutionCloner;
        private ConstraintConfigurationDescriptor constraintConfigurationDescriptor;
        private readonly Dictionary<string, MemberAccessor> entityCollectionMemberAccessorMap = new Dictionary<string, MemberAccessor>();

        private MemberAccessor constraintConfigurationMemberAccessor;

        public List<EntityDescriptor> GetGenuineEntityDescriptors()
        {
            var genuineEntityDescriptorList = new List<EntityDescriptor>(entityDescriptorMap.Count);
            foreach (var entityDescriptor in entityDescriptorMap)
            {
                if (entityDescriptor.Value.HasAnyDeclaredGenuineVariableDescriptor())
                {
                    genuineEntityDescriptorList.Add(entityDescriptor.Value);
                }
            }
            return genuineEntityDescriptorList;
        }

        private readonly ConcurrentDictionary<Type, MemberAccessor> planningIdMemberAccessorMap = new ConcurrentDictionary<Type, MemberAccessor>();
        private DomainAccessType domainAccessType;
        public Type SolutionClass;

        public SolutionDescriptor(Type solutionClass, Dictionary<string, MemberAccessor> memberAccessorMap)
        {
            this.SolutionClass = solutionClass;
            this.MemberAccessorFactory = new MemberAccessorFactory(memberAccessorMap);
        }

        public MemberAccessorFactory GetMemberAccessorFactory()
        {
            return MemberAccessorFactory;
        }

        public DomainAccessType GetDomainAccessType()
        {
            return domainAccessType;
        }

        public API.Score.Score GetScore(ISolution solution)
        {
            return scoreDescriptor.GetScore(solution);
        }

        public void SetScore(ISolution solution, API.Score.Score score)
        {
            scoreDescriptor.SetScore(solution, score);
        }

        private EntityDescriptor InnerFindEntityDescriptor(Type entitySubclass)
        {
            // Reverse order to find the nearest ancestor
            foreach (var entityClass in reversedEntityClassList)
            {
                if (entityClass.IsAssignableFrom(entitySubclass))
                {
                    return entityDescriptorMap[entityClass];
                }
            }
            return null;
        }

        public EntityDescriptor FindEntityDescriptor(Type entitySubclass)
        {

            EntityDescriptor cachedEntityDescriptor = null;
            lowestEntityDescriptorMap.TryGetValue(entitySubclass, out cachedEntityDescriptor);
            if (cachedEntityDescriptor == NULL_ENTITY_DESCRIPTOR)
            { // Cache hit, no descriptor found.
                return null;
            }
            else if (cachedEntityDescriptor != null)
            { // Cache hit, descriptor found.
                return cachedEntityDescriptor;
            }
            // Cache miss, look for the descriptor.
            EntityDescriptor newEntityDescriptor = InnerFindEntityDescriptor(entitySubclass);
            if (newEntityDescriptor == null)
            {
                // Dummy entity descriptor value, as ConcurrentMap does not allow null values.
                lowestEntityDescriptorMap.AddOrUpdate(entitySubclass, NULL_ENTITY_DESCRIPTOR, (k, v) => NULL_ENTITY_DESCRIPTOR);
                return null;
            }
            else
            {
                lowestEntityDescriptorMap.AddOrUpdate(entitySubclass, newEntityDescriptor, (k, v) => newEntityDescriptor);
                return newEntityDescriptor;
            }
        }

        public MemberAccessor GetPlanningIdAccessor(Type factClass)
        {
            MemberAccessor memberAccessor = planningIdMemberAccessorMap.GetValueOrDefault(factClass);
            if (memberAccessor == null)
            {
                memberAccessor = ConfigUtils.FindPlanningIdMemberAccessor(factClass, GetMemberAccessorFactory(), GetDomainAccessType());
                MemberAccessor nonNullMemberAccessor = memberAccessor ?? DummyMemberAccessor.INSTANCE;
                planningIdMemberAccessorMap.AddOrUpdate(factClass, nonNullMemberAccessor, (k, v) => nonNullMemberAccessor);
                return memberAccessor;
            }
            else if (memberAccessor == DummyMemberAccessor.INSTANCE)
            {
                return null;
            }
            else
            {
                return memberAccessor;
            }
        }

        public ScoreDefinition GetScoreDefinition()
        {
            return scoreDescriptor.ScoreDefinition;
        }

        internal static SolutionDescriptor BuildSolutionDescriptor(DomainAccessType domainAccessType, Type solutionClass, Dictionary<string, MemberAccessor> memberAccessorMap, Dictionary<string, SolutionCloner> solutionClonerMap, List<Type> entityClassList)
        {
            solutionClonerMap = solutionClonerMap ?? new Dictionary<string, SolutionCloner>();
            SolutionDescriptor solutionDescriptor = new SolutionDescriptor(solutionClass, memberAccessorMap);
            DescriptorPolicy descriptorPolicy = new DescriptorPolicy();
            descriptorPolicy.DomainAccessType = domainAccessType;
            descriptorPolicy.GeneratedSolutionClonerMap = solutionClonerMap;
            descriptorPolicy.MemberAccessorFactory = solutionDescriptor.MemberAccessorFactory;

            solutionDescriptor.ProcessAnnotations(descriptorPolicy, entityClassList);
            foreach (var entityClass in SortEntityClassList(entityClassList))
            {
                EntityDescriptor entityDescriptor = new EntityDescriptor(solutionDescriptor, entityClass);
                solutionDescriptor.AddEntityDescriptor(entityDescriptor);
                entityDescriptor.ProcessAnnotations(descriptorPolicy);
            }
            solutionDescriptor.AfterAnnotationsProcessed(descriptorPolicy);
            return solutionDescriptor;
        }

        public void AddEntityDescriptor(EntityDescriptor entityDescriptor)
        {
            Type entityClass = entityDescriptor.EntityClass;
            foreach (var otherEntityClass in entityDescriptorMap.Keys)
            {
                if (entityClass.IsAssignableFrom(otherEntityClass))
                {
                    throw new Exception("An earlier entityClass (" + otherEntityClass
                            + ") should not be a subclass of a later entityClass (" + entityClass
                            + "). Switch their declaration so superclasses are defined earlier.");
                }
            }
            entityDescriptorMap.Add(entityClass, entityDescriptor);
            reversedEntityClassList.Insert(0, entityClass);
            lowestEntityDescriptorMap.AddOrUpdate(entityClass, entityDescriptor, (k, v) => entityDescriptor);
        }

        private static List<Type> SortEntityClassList(List<Type> entityClassList)
        {
            List<Type> sortedEntityClassList = new List<Type>(entityClassList.Count);
            foreach (var entityClass in entityClassList)
            {
                bool added = false;
                for (int i = 0; i < sortedEntityClassList.Count; i++)
                {
                    var sortedEntityClass = sortedEntityClassList[i];
                    if (entityClass.IsAssignableFrom(sortedEntityClass))
                    {
                        sortedEntityClassList.Insert(i, entityClass);
                        added = true;
                        break;
                    }
                }
                if (!added)
                {
                    sortedEntityClassList.Add(entityClass);
                }
            }
            return sortedEntityClassList;
        }



        private void AfterAnnotationsProcessed(DescriptorPolicy descriptorPolicy)
        {
            foreach (var entityDescriptor in entityDescriptorMap.Values)
            {
                entityDescriptor.LinkEntityDescriptors(descriptorPolicy);
            }
            foreach (var entityDescriptor in entityDescriptorMap.Values)
            {
                entityDescriptor.LinkVariableDescriptors(descriptorPolicy);
            }
            DetermineGlobalShadowOrder();
            problemFactOrEntityClassSet = CollectEntityAndProblemFactClasses();
            ListVariableDescriptors = FindListVariableDescriptors();

            InitSolutionCloner(descriptorPolicy);
        }

        private List<ListVariableDescriptor> FindListVariableDescriptors()
        {
            return GetGenuineEntityDescriptors()
         .SelectMany(entityDescriptor => entityDescriptor.GetGenuineVariableDescriptorList())
         .Where(variableDescriptor => variableDescriptor.IsListVariable())
         .Select(variableDescriptor => (ListVariableDescriptor)variableDescriptor)
         .ToList();
        }

        private void InitSolutionCloner(DescriptorPolicy descriptorPolicy)
        {
            if (solutionCloner == null)
            {
                var item = GizmoSolutionClonerFactory.GetGeneratedClassName(this);
                descriptorPolicy.GeneratedSolutionClonerMap.TryGetValue(item, out solutionCloner);
            }
            if (solutionCloner == null)
            {
                switch (descriptorPolicy.DomainAccessType)
                {
                    case DomainAccessType.GIZMO:
                        solutionCloner = GizmoSolutionClonerFactory.Build(this, MemberAccessorFactory.GetGizmoClassLoader());
                        break;
                    case DomainAccessType.REFLECTION:
                        solutionCloner = new FieldAccessingSolutionCloner(this);
                        break;
                    default:
                        throw new Exception("The domainAccessType (" + domainAccessType + ") is not implemented.");
                }
            }
        }

        private HashSet<Type> CollectEntityAndProblemFactClasses()
        {
            // Figure out all problem fact or entity types that are used within this solution,
            // using the knowledge we've already gained by processing all the annotations.
            var entityClassStream = entityDescriptorMap.Keys;
            var factClassStream = problemFactMemberAccessorMap.Values.Select(ma => ma.GetType());
            var problemFactOrEntityClassStream = entityClassStream.Concat(factClassStream);

            var factCollectionClassStream = problemFactCollectionMemberAccessorMap.Values
       .Select(accessor =>
       {
           return ConfigUtils.ExtractCollectionGenericTypeParameterLeniently(
               "solutionClass", SolutionClass, accessor.GetType(), accessor.GetGenericType(),
               typeof(ProblemFactCollectionProperty), accessor.GetName()) ?? (typeof(object));
       });


            problemFactOrEntityClassStream = problemFactOrEntityClassStream.Concat(factCollectionClassStream);
            // Add constraint configuration, if configured.
            if (constraintConfigurationDescriptor != null)
            {
                problemFactOrEntityClassStream = problemFactOrEntityClassStream.Concat(new Type[] { constraintConfigurationDescriptor.ConstraintConfigurationClass });
            }
            return new HashSet<Type>(problemFactOrEntityClassStream);
        }

        public SolutionCloner GetSolutionCloner()
        {
            return solutionCloner;
        }

        private void DetermineGlobalShadowOrder()
        {
            var pairList = new List<MutablePair<ShadowVariableDescriptor, int>>();
            var shadowToPairMap = new Dictionary<ShadowVariableDescriptor, MutablePair<ShadowVariableDescriptor, int>>();
            foreach (var entityDescriptor in entityDescriptorMap.Values)
            {
                foreach (var shadow in entityDescriptor.GetDeclaredShadowVariableDescriptors())
                {
                    int sourceSize = shadow.GetSourceVariableDescriptorList().Count;
                    MutablePair<ShadowVariableDescriptor, int> pair = MutablePairHelper.Of(shadow, sourceSize);
                    pairList.Add(pair);
                    shadowToPairMap.Add(shadow, pair);
                }
            }
            foreach (var entityDescriptor in entityDescriptorMap.Values)
            {
                foreach (var genuine in entityDescriptor.GetDeclaredGenuineVariableDescriptors())
                {
                    foreach (var sink in genuine.SinkVariableDescriptorList)
                    {
                        MutablePair<ShadowVariableDescriptor, int> sinkPair = shadowToPairMap[sink];
                        sinkPair.SetValue(sinkPair.GetValue() - 1);
                    }
                }
            }
            int globalShadowOrder = 0;
            while (pairList.Count > 0)
            {
                pairList.Sort((i1, i2) => i1.GetValue().CompareTo(i2.GetValue()));
                Pair<ShadowVariableDescriptor, int> pair = pairList.ElementAt(0);
                pairList.RemoveAt(0);
                ShadowVariableDescriptor shadow = pair.GetKey();
                if (pair.GetValue() != 0)
                {
                    if (pair.GetValue() < 0)
                    {
                        throw new Exception("Impossible state because the shadowVariable annot be used more as a sink than it has sources.");
                    }
                    throw new Exception("There is a cyclic shadow variable path).");
                }
                foreach (var sink in shadow.SinkVariableDescriptorList)
                {
                    MutablePair<ShadowVariableDescriptor, int> sinkPair = shadowToPairMap[sink];
                    sinkPair.SetValue(sinkPair.GetValue() - 1);
                }
                shadow.GlobalShadowOrder = (globalShadowOrder);
                globalShadowOrder++;
            }
        }

        private ClassAndPlanningIdComparator classAndPlanningIdComparator;
        private AutoDiscoverMemberType autoDiscoverMemberType;
        private LookUpStrategyResolver lookUpStrategyResolver;

        private void ProcessSolutionAnnotations(DescriptorPolicy descriptorPolicy)
        {
            PlanningSolutionAttribute solutionAnnotation = (PlanningSolutionAttribute)Attribute.GetCustomAttribute(SolutionClass, typeof(PlanningSolutionAttribute));
            if (solutionAnnotation == null)
            {
                throw new Exception("The solutionClass (" + SolutionClass
                        + ") has been specified as a solution in the configuration," +
                        " but does not have annotation.");
            }
            autoDiscoverMemberType = solutionAnnotation.AutoDiscoverMemberType;
            Type solutionClonerClass = solutionAnnotation.SolutionCloner;
            if (solutionClonerClass != typeof(NullSolutionCloner))
            {
                solutionCloner = ConfigUtils.NewInstance<SolutionCloner>("solutionClonerClass", solutionClonerClass);
            }
            lookUpStrategyResolver = new LookUpStrategyResolver(descriptorPolicy, solutionAnnotation.LookUpStrategyType);
        }

        private void ProcessAnnotations(DescriptorPolicy descriptorPolicy, List<Type> entityClassList)
        {
            domainAccessType = descriptorPolicy.DomainAccessType;
            classAndPlanningIdComparator = new ClassAndPlanningIdComparator(MemberAccessorFactory, domainAccessType, false);
            ProcessSolutionAnnotations(descriptorPolicy);
            List<MethodInfo> potentiallyOverwritingMethodList = new List<MethodInfo>();

            foreach (var lineageClass in ConfigUtils.GetAllAnnotatedLineageClasses(SolutionClass, typeof(PlanningSolutionAttribute)))
            {
                List<MemberInfo> memberList = ConfigUtils.GetDeclaredMembers(lineageClass);
                foreach (var member in memberList)
                {
                    if (member is MethodInfo method && potentiallyOverwritingMethodList.Any(
                            m => member.Name.Equals(m.Name) // Shortcut to discard negatives faster
                                    && ReflectionHelper.IsMethodOverwritten(method, m.DeclaringType)))
                    {
                        // Ignore member because it is an overwritten method
                        continue;
                    }
                    ProcessValueRangeProviderAnnotation(descriptorPolicy, lineageClass, member);
                    ProcessFactEntityOrScoreAnnotation(lineageClass, descriptorPolicy, member, entityClassList);
                }
                memberList.Where(member => member is MethodInfo).ToList()
                        .ForEach(member => potentiallyOverwritingMethodList.Add((MethodInfo)member));
            }
            if (entityCollectionMemberAccessorMap.Count == 0 && entityMemberAccessorMap.Count == 0)
            {
                throw new Exception("The solutionClass (" + SolutionClass + ") must have at least 1 member with a ation.");
            }
            // Do not check if problemFactCollectionMemberAccessorMap and problemFactMemberAccessorMap are empty
            // because they are only required for ConstraintStreams.
            if (scoreDescriptor == null)
            {
                throw new Exception("The solutionClass (" + SolutionClass + ") must have 1 member with a   annotation.");
            }
            if (constraintConfigurationMemberAccessor != null)
            {
                // The scoreDescriptor is definitely initialized at this point.
                constraintConfigurationDescriptor.ProcessAnnotations(descriptorPolicy, scoreDescriptor.ScoreDefinition);
            }
        }

        private void ProcessFactEntityOrScoreAnnotation(Type clazz, DescriptorPolicy descriptorPolicy, MemberInfo member, List<Type> entityClassList)
        {
            Type annotationClass = ExtractFactEntityOrScoreAnnotationClassOrAutoDiscover(member, entityClassList);
            if (annotationClass == null)
            {
                return;
            }
            else if (annotationClass == typeof(ConstraintConfigurationProviderAttribute))
            {
                ProcessConstraintConfigurationProviderAnnotation(descriptorPolicy, member, annotationClass);
            }
            else if (annotationClass == typeof(ProblemFactPropertyAttribute)
                || annotationClass == typeof(ProblemFactCollectionProperty))
            {
                ProcessProblemFactPropertyAnnotation(clazz, descriptorPolicy, member, annotationClass);
            }
            else if (annotationClass == typeof(PlanningEntityPropertyAttribute)
                || annotationClass == typeof(PlanningEntityCollectionProperty))
            {
                ProcessPlanningEntityPropertyAnnotation(clazz, descriptorPolicy, member, annotationClass);
            }
            else if (annotationClass == typeof(PlanningScoreAttribute))
            {
                if (scoreDescriptor == null)
                {
                    // Bottom class wins. Bottom classes are parsed first due to ConfigUtil.getAllAnnotatedLineageClasses().
                    scoreDescriptor = ScoreDescriptor.BuildScoreDescriptor(descriptorPolicy, member, SolutionClass);
                }
                else
                {
                }
            }
        }

        private void ProcessPlanningEntityPropertyAnnotation(Type clazz, DescriptorPolicy descriptorPolicy, MemberInfo member, Type annotationClass)
        {
            MemberAccessor memberAccessor = descriptorPolicy.MemberAccessorFactory.BuildAndCacheMemberAccessor(clazz, member, MemberAccessorType.PROPERTY_OR_GETTER_METHOD
               , annotationClass, descriptorPolicy.DomainAccessType);
            //assertNoFieldAndGetterDuplicationOrConflict(memberAccessor, annotationClass);
            if (annotationClass == typeof(PlanningEntityPropertyAttribute))
            {
                entityMemberAccessorMap.Add(memberAccessor.GetName(), memberAccessor);
            }
            else if (annotationClass == typeof(PlanningEntityCollectionProperty))
            {
                Type type = memberAccessor.GetType();


                entityCollectionMemberAccessorMap.Add(memberAccessor.GetName(), memberAccessor);
            }
            else
            {
                throw new Exception("Impossible situation with annotationClass (" + annotationClass + ").");
            }
        }


        private void ProcessConstraintConfigurationProviderAnnotation(DescriptorPolicy descriptorPolicy, MemberInfo member, Type annotationClass)
        {
            throw new NotImplementedException();
        }

        private void ProcessProblemFactPropertyAnnotation(Type clazz, DescriptorPolicy descriptorPolicy, MemberInfo member, Type annotationClass)
        {
            MemberAccessor memberAccessor = descriptorPolicy.MemberAccessorFactory.BuildAndCacheMemberAccessor(clazz, member,
               MemberAccessorType.PROPERTY_OR_READ_METHOD, annotationClass, descriptorPolicy.DomainAccessType);
            //assertNoFieldAndGetterDuplicationOrConflict(memberAccessor, annotationClass);
            if (annotationClass == typeof(ProblemFactPropertyAttribute))
            {
                problemFactMemberAccessorMap.Add(memberAccessor.GetName(), memberAccessor);
            }
            else if (annotationClass == typeof(ProblemFactCollectionProperty))
            {
                Type type = memberAccessor.GetType();

                problemFactCollectionMemberAccessorMap.Add(memberAccessor.GetName(), memberAccessor);
            }
            else
            {
                throw new Exception("Impossible situation with annotationClass (" + annotationClass + ").");
            }
        }


        private Type ExtractFactEntityOrScoreAnnotationClassOrAutoDiscover(MemberInfo member, List<Type> entityClassList)
        {
            Type annotationClass = ConfigUtils.ExtractAnnotationClass(member, typeof(ConstraintConfigurationProviderAttribute), typeof(ProblemFactPropertyAttribute),
                 typeof(ProblemFactCollectionProperty), typeof(PlanningEntityPropertyAttribute), typeof(PlanningEntityCollectionProperty), typeof(PlanningScoreAttribute));
            if (annotationClass == null)
            {
                Type type;
                if (autoDiscoverMemberType == AutoDiscoverMemberType.PROPERTY && member is PropertyInfo property)
                {
                    type = property.GetType();
                }
                else if (autoDiscoverMemberType == AutoDiscoverMemberType.GETTER && (member is MethodInfo method) && ReflectionHelper.IsGetterMethod(method))
                {
                    type = method.ReturnType;
                }
                else
                {
                    type = null;
                }
                if (type != null)
                {
                    if (typeof(API.Score.Score).IsAssignableFrom(type))
                    {
                        annotationClass = typeof(PlanningScoreAttribute);
                    }
                    else if (typeof(Collection<>).IsAssignableFrom(type) || type.IsArray)
                    {
                        Type elementType;
                        if (typeof(Collection<>).IsAssignableFrom(type))
                        {
                            //Type genericType = (member is FieldInfo f) ? f.GetGenericType()
                            //                          : ((MethodInfo)member).GetGenericReturnType();
                            Type genericType = null;
                            String memberName = member.Name;

                            elementType = ConfigUtils.ExtractCollectionGenericTypeParameterLeniently(
                                    "solutionClass", SolutionClass,
                                    type, genericType,
                                    null, member.Name) ?? (typeof(object));
                        }
                        else
                        {
                            elementType = null;
                            //elementType = type.GetComponentType();
                        }
                        if (entityClassList.Any(entityClass => entityClass.IsAssignableFrom(elementType)))
                        {
                            annotationClass = typeof(PlanningEntityCollectionProperty);
                        }
                        else if (Attribute.IsDefined(elementType, typeof(ConstraintConfigurationAttribute)))
                        {
                            throw new Exception("The autoDiscoverMemberT rray of that type.");
                        }
                        else
                        {
                            annotationClass = typeof(ProblemFactCollectionProperty);
                        }
                    }
                    else if (typeof(IDictionary<object, object>).IsAssignableFrom(type))
                    {
                        throw new Exception("The autoDiscoverMemberType (" + autoDiscoverMemberType
                                + ") does not yet support the member (" + member
                                + ") of type (" + type
                                + ") which is an implementation o.");
                    }
                    else if (entityClassList.Any(entityClass => entityClass.IsAssignableFrom(type)))
                    {
                        annotationClass = typeof(PlanningEntityPropertyAttribute);
                    }
                    else if (Attribute.IsDefined(type, typeof(ConstraintConfigurationAttribute)))
                    {
                        annotationClass = typeof(ConstraintConfigurationProviderAttribute);
                    }
                    else
                    {
                        annotationClass = typeof(ProblemFactPropertyAttribute);
                    }
                }
            }
            return annotationClass;
        }

        private void ProcessValueRangeProviderAnnotation(DescriptorPolicy descriptorPolicy, Type clazz, MemberInfo member)
        {
            if (Attribute.IsDefined(member, typeof(ValueRangeProviderAttribute)))
            {
                MemberAccessor memberAccessor = descriptorPolicy.MemberAccessorFactory.BuildAndCacheMemberAccessor(clazz, member,
                        MemberAccessorType.PROPERTY_OR_READ_METHOD, typeof(ValueRangeProviderAttribute), descriptorPolicy.DomainAccessType);
                descriptorPolicy.AddFromSolutionValueRangeProvider(memberAccessor);
            }
        }

        public EntityDescriptor GetEntityDescriptorStrict(Type entityClass)
        {
            return entityDescriptorMap[entityClass];
        }

        public ConstraintConfigurationDescriptor GetConstraintConfigurationDescriptor()
        {
            return constraintConfigurationDescriptor;
        }

        public MemberAccessor GetConstraintConfigurationMemberAccessor()
        {
            return constraintConfigurationMemberAccessor;
        }

        public LookUpStrategyResolver GetLookUpStrategyResolver()
        {
            return lookUpStrategyResolver;
        }

        public IEnumerable<EntityDescriptor> GetEntityDescriptors()
        {
            return entityDescriptorMap.Values.ToList();
        }

        public bool HasMovableEntities(ScoreDirector scoreDirector)
        {
            return ExtractAllEntitiesStream(scoreDirector.GetWorkingSolution())
                    .Any(entity => FindEntityDescriptorOrFail(entity.GetType()).IsMovable(scoreDirector, entity));
        }

        public EntityDescriptor FindEntityDescriptorOrFail(Type entitySubclass)
        {
            EntityDescriptor entityDescriptor = FindEntityDescriptor(entitySubclass);
            if (entityDescriptor == null)
            {
                throw new Exception("A planning entity is an instance of a class (is missing from your solver configuration.");
            }
            return entityDescriptor;
        }

        private IEnumerable<object> ExtractAllEntitiesStream(ISolution solution)
        {
            List<object> stream = new List<object>();
            foreach (var memberAccessor in entityMemberAccessorMap.Values)
            {
                var entity = ExtractMemberObject(memberAccessor, solution);
                if (entity != null)
                {
                    stream.Add(entity);
                }
            }
            foreach (var memberAccessor in entityCollectionMemberAccessorMap.Values)
            {
                var entityCollection = ExtractMemberCollectionOrArray(memberAccessor, solution, false);
                stream.AddRange((IEnumerable<object>)entityCollection);
            }
            return stream;
        }

        private object ExtractMemberObject(MemberAccessor memberAccessor, ISolution solution)
        {
            return memberAccessor.ExecuteGetter(solution);
        }

        private IList ExtractMemberCollectionOrArray(MemberAccessor memberAccessor, ISolution solution, bool isFact)
        {
            if (memberAccessor.GetClass().IsArray)
            {
                var arrayObject = (object[])memberAccessor.ExecuteGetter(solution);
                return arrayObject.ToList();
            }
            else
            {
                return (IList)memberAccessor.ExecuteGetter(solution);
            }
        }

        public IEnumerable<Type> GetEntityClassSet()
        {
            return entityDescriptorMap.Keys;
        }

        public Dictionary<string, MemberAccessor> GetEntityMemberAccessorMap()
        {
            return entityMemberAccessorMap;
        }

        public Dictionary<string, MemberAccessor> GetEntityCollectionMemberAccessorMap()
        {
            return entityCollectionMemberAccessorMap;
        }

        public void VisitEntitiesByEntityClass(ISolution solution, Type entityClass, Action<Object> visitor)
        {
            foreach (var entityMemberAccessor in entityMemberAccessorMap.Values)
            {
                Object entity = ExtractMemberObject(entityMemberAccessor, solution);
                if (entityClass.IsInstanceOfType(entity))
                {
                    visitor.Invoke(entity);
                }
            }
            foreach (var entityCollectionMemberAccessor in entityCollectionMemberAccessorMap.Values)
            {
                var optionalTypeParameter = ConfigUtils.ExtractCollectionGenericTypeParameterLeniently(
                         "solutionClass", entityCollectionMemberAccessor.GetDeclaringClass(),
                         entityCollectionMemberAccessor.GetType(),
                         entityCollectionMemberAccessor.GetGenericType(),
                         null,
                         entityCollectionMemberAccessor.GetName());
                bool collectionGuaranteedToContainOnlyGivenEntityType = optionalTypeParameter != null && entityClass.IsAssignableFrom(optionalTypeParameter);
                if (collectionGuaranteedToContainOnlyGivenEntityType)
                {
                    /*
                     * In a typical case typeParameter is specified and it is the expected entity or its superclass.
                     * Therefore we can simply apply the visitor on each element.
                     */
                    var entityCollection2 = ExtractMemberCollectionOrArray(entityCollectionMemberAccessor, solution, false);
                    foreach (var e in entityCollection2)
                        visitor(e);
                    continue;
                }
                // The collection now is either raw, or it is not of an entity type, such as perhaps a parent interface.
                bool collectionCouldPossiblyContainGivenEntityType = optionalTypeParameter?.IsAssignableFrom(entityClass) ?? true;
                if (!collectionCouldPossiblyContainGivenEntityType)
                {
                    // There is no way how this collection could possibly contain entities of the given type.
                    continue;
                }
                // We need to go over every collection member and check if it is an entity of the given type.
                var entityCollection = ExtractMemberCollectionOrArray(entityCollectionMemberAccessor, solution, false);
                foreach (var entity in entityCollection)
                {
                    if (entityClass.IsInstanceOfType(entity))
                    {
                        visitor.Invoke(entity);
                    }
                }
            }
        }

        public void VisitAllProblemFacts(ISolution solution, Action<object> visitor)
        {
            // Visits facts.
            foreach (var accessor in problemFactMemberAccessorMap.Values)
            {
                object obj = ExtractMemberObject(accessor, solution);
                if (obj != null)
                {
                    visitor.Invoke(obj);
                }
            }
            // Visits problem facts from problem fact collections.
            foreach (var accessor in problemFactCollectionMemberAccessorMap.Values)
            {
                var objects = ExtractMemberCollectionOrArray(accessor, solution, true);
                foreach (var obj in objects)
                {
                    visitor.Invoke(obj);
                }
            }
        }

        public void VisitAllEntities(ISolution solution, Action<object> visitor)
        {
            VisitAllEntities(solution, visitor, collection =>
            {
                foreach (var item in collection)
                {
                    visitor.Invoke(item);
                }
            });
        }


        public void VisitAllEntities(ISolution solution, Action<object> visitor, Action<IEnumerable> collectionVisitor)
        {
            foreach (var entityMemberAccessor in entityMemberAccessorMap.Values)
            {
                var entity = ExtractMemberObject(entityMemberAccessor, solution);
                if (entity != null)
                {
                    visitor.Invoke(entity);
                }
            }
            foreach (var entityCollectionMemberAccessor in entityCollectionMemberAccessorMap.Values)
            {
                var entityCollection = ExtractMemberCollectionOrArray(entityCollectionMemberAccessor, solution, false);
                collectionVisitor.Invoke(entityCollection);
            }
        }

        public int GetEntityCount(ISolution solution)
        {
            MutableInt entityCount = new MutableInt();
            VisitAllEntities(solution,
                    fact => entityCount.Increment(),
                    collection => entityCount.Add(collection.Count));
            return entityCount.Value;
        }

        private void VisitAllEntities(ISolution solution, Action<object> visitor, Action<IList> collectionVisitor)
        {
            foreach (var entityMemberAccessor in entityMemberAccessorMap.Values)
            {
                object entity = ExtractMemberObject(entityMemberAccessor, solution);
                if (entity != null)
                {
                    visitor.Invoke(entity);
                }
            }
            foreach (var entityCollectionMemberAccessor in entityCollectionMemberAccessorMap.Values)
            {
                var entityCollection = ExtractMemberCollectionOrArray(entityCollectionMemberAccessor, solution, false);
                collectionVisitor.Invoke(entityCollection);
            }
        }

        public bool HasEntityDescriptor(Type entitySubclass)
        {
            EntityDescriptor entityDescriptor = FindEntityDescriptor(entitySubclass);
            return entityDescriptor != null;
        }

        public int CountUninitialized(ISolution solution)
        {
            int uninitializedVariableCount = CountUninitializedVariables(solution);
            int uninitializedValueCount = CountUnassignedListVariableValues(solution);
            return uninitializedValueCount + uninitializedVariableCount;
        }

        private int CountUninitializedVariables(ISolution solution)
        {
            MutableInt result = new MutableInt();
            VisitAllEntities(solution, entity => result.Add(FindEntityDescriptorOrFail(entity.GetType()).CountUninitializedVariables(entity)));
            return result.Value;
        }

        private int CountUnassignedListVariableValues(ISolution solution)
        {
            int unassignedValueCount = 0;
            foreach (var listVariableDescriptor in ListVariableDescriptors)
            {
                unassignedValueCount += CountUnassignedListVariableValues(solution, listVariableDescriptor);
            }
            return unassignedValueCount;
        }

        private int CountUnassignedListVariableValues(ISolution solution, ListVariableDescriptor variableDescriptor)
        {
            long totalValueCount = variableDescriptor.GetValueCount(solution, null);
            MutableInt assignedValuesCount = new MutableInt();
            VisitAllEntities(solution,
                    entity =>
                    {
                        EntityDescriptor entityDescriptor = variableDescriptor.EntityDescriptor;
                        if (entityDescriptor.MatchesEntity(entity))
                        {
                            assignedValuesCount.Add(variableDescriptor.GetListSize(entity));
                        }
                    });
            // TODO maybe detect duplicates and elements that are outside the value range
            return (int)(totalValueCount - assignedValuesCount.Value);
        }

        internal void VisitAll(ISolution solution, Action<object> visitor)
        {
            VisitAllProblemFacts(solution, visitor);
            VisitAllEntities(solution, visitor);
        }
    }
}
