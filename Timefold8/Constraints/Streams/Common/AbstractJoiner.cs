using TimefoldSharp.Core.API.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Common
{
    public interface IJoiner
    {
        int GetJoinerCount();
        JoinerType GetJoinerType(int index);
    }

    public abstract class AbstractJoiner<Right_, Property_> : IJoiner
    {
        protected readonly Func<Right_, Property_>[] rightMappings;
        protected readonly JoinerType[] joinerTypes;

        protected AbstractJoiner(Func<Right_, Property_> rightMapping, JoinerType joinerType) :
            this(new Func<Right_, Property_>[] { rightMapping }, new JoinerType[] { joinerType })
        {
        }

        public Func<Right_, Property_> GetRightMapping(int index)
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

        protected AbstractJoiner(Func<Right_, Property_>[] rightMappings, JoinerType[] joinerTypes)
        {
            this.rightMappings = rightMappings;
            this.joinerTypes = joinerTypes;
        }
    }
}
