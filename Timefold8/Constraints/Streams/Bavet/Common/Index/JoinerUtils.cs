using TimefoldSharp.Core.Constraints.Streams.Common;
using TimefoldSharp.Core.Constraints.Streams.Common.Bi;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index
{
    public static class JoinerUtils
    {

        public static Func<Right_, IndexProperties> CombineRightMappings<Right_, Property_>(AbstractJoiner<Right_, Property_> joiner)
        {
            int joinerCount = joiner.GetJoinerCount();
            switch (joinerCount)
            {
                case 0:
                    return (Right_ right) => NoneIndexProperties.INSTANCE;
                case 1:
                    var mapping = joiner.GetRightMapping(0);
                    return (Right_ right) => new SingleIndexProperties(mapping.Invoke(right));
                case 2:
                    var mappingX = joiner.GetRightMapping(0);
                    var mappingY = joiner.GetRightMapping(1);
                    return (Right_ right) => new TwoIndexProperties(mappingX.Invoke(right), mappingY.Invoke(right));
                case 3:
                    var mapping1 = joiner.GetRightMapping(0);
                    var mapping2 = joiner.GetRightMapping(1);
                    var mapping3 = joiner.GetRightMapping(2);
                    return (Right_ right) => new ThreeIndexProperties(mapping1.Invoke(right), mapping2.Invoke(right), mapping3.Invoke(right));
                default:
                    return (Right_ right) => {
                        object[] mappings = new Object[joinerCount];
                        for (int i = 0; i < joinerCount; i++)
                        {
                            mappings[i] = joiner.GetRightMapping(i).Invoke(right);
                        }
                        return new ManyIndexProperties(mappings);
                    };
            }
        }


        public static Func<A, IndexProperties> CombineLeftMappings<A, B, Property_>(DefaultBiJoiner<A, B, Property_> joiner)
        {
            int joinerCount = joiner.GetJoinerCount();
            switch (joinerCount)
            {
                case 0:
                    return (a) => NoneIndexProperties.INSTANCE;
                case 1:
                    var mapping = joiner.GetLeftMapping(0);
                    return (A a) => new SingleIndexProperties(mapping.Invoke(a));
                case 2:
                    var mappingX = joiner.GetLeftMapping(0);
                    var mappingY = joiner.GetLeftMapping(1);
                    return (A a) => new TwoIndexProperties(mappingX.Invoke(a), mappingY.Invoke(a));
                case 3:
                    var mapping2 = joiner.GetLeftMapping(1);
                    var mapping3 = joiner.GetLeftMapping(2);
                    var mapping1 = joiner.GetLeftMapping(0);
                    return (A a) => new ThreeIndexProperties(mapping1.Invoke(a), mapping2.Invoke(a), mapping3.Invoke(a));
                default:
                     return (A a) => {
                         object[] mappings = new Object[joinerCount];
                         for (int i = 0; i < joinerCount; i++)
                         {
                             mappings[i] = joiner.GetLeftMapping(i).Invoke(a);
                         }
                         return new ManyIndexProperties(mappings);
                     };
                    throw new NotImplementedException();
            }
        }
    }
}

