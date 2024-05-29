using TimefoldSharp.Core.API.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Common
{
    public interface IJoiner
    {
        int GetJoinerCount();
        JoinerType GetJoinerType(int index);
    }

    public abstract class AbstractJoiner<Right_> : IJoiner
    {
        protected readonly Func<Right_, object>[] rightMappings;
        protected readonly JoinerType[] joinerTypes;

        protected AbstractJoiner(Func<Right_, object> rightMapping, JoinerType joinerType) :
            this(new Func<Right_, object>[] { rightMapping }, new JoinerType[] { joinerType })
        {
        }

        public Func<Right_, object> GetRightMapping(int index)
        {
            return rightMappings[index];
        }

        public JoinerType GetJoinerType(int index)
        {
            return joinerTypes[index];
        }

        public int GetJoinerCount()
        {
            return joinerTypes.Length;
        }

        protected AbstractJoiner(Func<Right_, object>[] rightMappings, JoinerType[] joinerTypes)
        {
            this.rightMappings = rightMappings;
            this.joinerTypes = joinerTypes;
        }
    }
}
