namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index
{
    public class NoneIndexProperties : IndexProperties
    {
        public static NoneIndexProperties INSTANCE = new NoneIndexProperties();

        public object ToKey(int from, int to)
        {
            throw new Exception();
        }

        public object ToKey(int index)
        {
            throw new Exception("Impossible state: none index property requested");
        }

        public override string ToString()
        {
            return "[]";
        }
    }
}
