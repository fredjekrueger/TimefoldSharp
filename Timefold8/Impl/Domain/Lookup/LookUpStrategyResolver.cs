using System.Collections.Concurrent;
using System.Reflection;
using TimefoldSharp.Core.API.Domain.Solution;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Policy;

namespace TimefoldSharp.Core.Impl.Domain.Lookup
{
    public class LookUpStrategyResolver
    {

        private readonly LookUpStrategyType lookUpStrategyType;
        private readonly MemberAccessorFactory memberAccessorFactory;

        private readonly ConcurrentDictionary<Type, LookUpStrategy> decisionCache = new ConcurrentDictionary<Type, LookUpStrategy>();

        public LookUpStrategyResolver(DescriptorPolicy descriptorPolicy, LookUpStrategyType lookUpStrategyType)
        {
            this.lookUpStrategyType = lookUpStrategyType;
            this.memberAccessorFactory = descriptorPolicy.MemberAccessorFactory;

            decisionCache.AddOrUpdate(typeof(bool), new ImmutableLookUpStrategy(), (k, v) => new ImmutableLookUpStrategy());
            decisionCache.AddOrUpdate(typeof(byte), new ImmutableLookUpStrategy(), (k, v) => new ImmutableLookUpStrategy());
            decisionCache.AddOrUpdate(typeof(short), new ImmutableLookUpStrategy(), (k, v) => new ImmutableLookUpStrategy());
            decisionCache.AddOrUpdate(typeof(int), new ImmutableLookUpStrategy(), (k, v) => new ImmutableLookUpStrategy());
            decisionCache.AddOrUpdate(typeof(long), new ImmutableLookUpStrategy(), (k, v) => new ImmutableLookUpStrategy());
            decisionCache.AddOrUpdate(typeof(float), new ImmutableLookUpStrategy(), (k, v) => new ImmutableLookUpStrategy());
            decisionCache.AddOrUpdate(typeof(double), new ImmutableLookUpStrategy(), (k, v) => new ImmutableLookUpStrategy());
            decisionCache.AddOrUpdate(typeof(char), new ImmutableLookUpStrategy(), (k, v) => new ImmutableLookUpStrategy());
            decisionCache.AddOrUpdate(typeof(string), new ImmutableLookUpStrategy(), (k, v) => new ImmutableLookUpStrategy());
            decisionCache.AddOrUpdate(typeof(Guid), new ImmutableLookUpStrategy(), (k, v) => new ImmutableLookUpStrategy());
            decisionCache.AddOrUpdate(typeof(DateTime), new ImmutableLookUpStrategy(), (k, v) => new ImmutableLookUpStrategy());
            decisionCache.AddOrUpdate(typeof(TimeSpan), new ImmutableLookUpStrategy(), (k, v) => new ImmutableLookUpStrategy());
        }

        public LookUpStrategy DetermineLookUpStrategy(Object obj)
        {
            return decisionCache.GetOrAdd(obj.GetType(), objectClass =>
            {
                if (objectClass.IsEnum)
                {
                    return new ImmutableLookUpStrategy();
                }
                switch (lookUpStrategyType)
                {
                    case LookUpStrategyType.PLANNING_ID_OR_NONE:
                        MemberAccessor memberAccessor1 =
                                ConfigUtils.FindPlanningIdMemberAccessor(objectClass, memberAccessorFactory);
                        if (memberAccessor1 == null)
                        {
                            return new NoneLookUpStrategy();
                        }
                        return new PlanningIdLookUpStrategy(memberAccessor1);
                    case LookUpStrategyType.PLANNING_ID_OR_FAIL_FAST:
                        MemberAccessor memberAccessor2 =
                                ConfigUtils.FindPlanningIdMemberAccessor(objectClass, memberAccessorFactory);
                        if (memberAccessor2 == null)
                        {
                            throw new Exception("The class .");
                        }
                        return new PlanningIdLookUpStrategy(memberAccessor2);
                    case LookUpStrategyType.EQUALITY:
                        MethodInfo equalsMethod;
                        MethodInfo hashCodeMethod;
                        try
                        {
                            equalsMethod = objectClass.GetMethod("Equals", new Type[] { typeof(object) });
                            hashCodeMethod = objectClass.GetMethod("GetHashCode");
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Impossible state because equals() and hashCode() always exist.");
                        }
                        return new EqualsLookUpStrategy();
                    case LookUpStrategyType.NONE:
                        return new NoneLookUpStrategy();
                    default:
                        throw new Exception("The lookUpStrategyType (" + lookUpStrategyType
                                + ") is not implemented.");
                }
            });
        }
    }
}
