using TimefoldSharp.Core.API.Domain.Solution;
using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;

namespace TimefoldSharp.Core.Impl.Domain.Policy
{
    public class DescriptorPolicy
    {
        public Dictionary<string, SolutionCloner> GeneratedSolutionClonerMap { get; set; } = new Dictionary<string, SolutionCloner>();
        public MemberAccessorFactory MemberAccessorFactory { get; set; }
        private readonly HashSet<MemberAccessor> anonymousFromSolutionValueRangeProviderSet = new HashSet<MemberAccessor>();
        private readonly Dictionary<string, MemberAccessor> fromSolutionValueRangeProviderMap = new Dictionary<string, MemberAccessor>();
        private readonly HashSet<MemberAccessor> anonymousFromEntityValueRangeProviderSet = new HashSet<MemberAccessor>();
        private readonly Dictionary<string, MemberAccessor> fromEntityValueRangeProviderMap = new Dictionary<string, MemberAccessor>();
        public void AddFromSolutionValueRangeProvider(MemberAccessor memberAccessor)
        {
            String id = ExtractValueRangeProviderId(memberAccessor);
            if (id == null)
            {
                anonymousFromSolutionValueRangeProviderSet.Add(memberAccessor);
            }
            else
            {
                fromSolutionValueRangeProviderMap.Add(id, memberAccessor);
            }
        }

        public HashSet<MemberAccessor> GetAnonymousFromSolutionValueRangeProviderSet()
        {
            return anonymousFromSolutionValueRangeProviderSet;
        }


        public HashSet<MemberAccessor> GetAnonymousFromEntityValueRangeProviderSet()
        {
            return anonymousFromEntityValueRangeProviderSet;
        }

        public bool IsFromSolutionValueRangeProvider(MemberAccessor memberAccessor)
        {
            return fromSolutionValueRangeProviderMap.ContainsValue(memberAccessor)
                    || anonymousFromSolutionValueRangeProviderSet.Contains(memberAccessor);
        }

        public bool IsFromEntityValueRangeProvider(MemberAccessor memberAccessor)
        {
            return fromEntityValueRangeProviderMap.ContainsValue(memberAccessor)
                    || anonymousFromEntityValueRangeProviderSet.Contains(memberAccessor);
        }

        private string ExtractValueRangeProviderId(MemberAccessor memberAccessor)
        {
            ValueRangeProviderAttribute annotation = memberAccessor.GetAnnotation<ValueRangeProviderAttribute>(typeof(ValueRangeProviderAttribute));
            string id = annotation.Id;
            if (id == null || string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            //validateUniqueValueRangeProviderId(id, memberAccessor);
            return id;
        }

        public void AddFromEntityValueRangeProvider(MemberAccessor memberAccessor)
        {
            String id = ExtractValueRangeProviderId(memberAccessor);
            if (id == null)
            {
                anonymousFromEntityValueRangeProviderSet.Add(memberAccessor);
            }
            else
            {
                fromEntityValueRangeProviderMap.Add(id, memberAccessor);
            }
        }

        public MemberAccessor GetFromSolutionValueRangeProvider(string id)
        {
            return fromSolutionValueRangeProviderMap[id];
        }

        public bool HasFromSolutionValueRangeProvider(string id)
        {
            return fromSolutionValueRangeProviderMap.ContainsKey(id);
        }

        public bool HasFromEntityValueRangeProvider(string id)
        {
            return fromEntityValueRangeProviderMap.ContainsKey(id);
        }

        public MemberAccessor GetFromEntityValueRangeProvider(string id)
        {
            return fromEntityValueRangeProviderMap[id];
        }

        public List<string> GetValueRangeProviderIds()
        {
            var valueRangeProviderIds = new List<string>(fromSolutionValueRangeProviderMap.Count + fromEntityValueRangeProviderMap.Count);
            valueRangeProviderIds.AddRange(fromSolutionValueRangeProviderMap.Keys);
            valueRangeProviderIds.AddRange(fromEntityValueRangeProviderMap.Keys);
            return valueRangeProviderIds;
        }
    }
}
