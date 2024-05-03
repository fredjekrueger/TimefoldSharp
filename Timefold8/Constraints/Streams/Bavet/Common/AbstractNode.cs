namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public abstract class AbstractNode
    {
        private long id;
        private long layerIndex = -1;

        public void SetId(long id)
        {
            this.id = id;
        }

        public abstract Propagator GetPropagator();

        public long GetLayerIndex()
        {
            if (layerIndex == -1)
            {
                throw new Exception(
                        "Impossible state: layer index for node (" + this + ") requested before being set.");
            }
            return layerIndex;
        }

        public void SetLayerIndex(long layerIndex)
        {
            if (layerIndex < 0)
            {
                throw new Exception("Impossible state: layer index (" + layerIndex + ") must be at least 0.");
            }
            this.layerIndex = layerIndex;
        }
    }
}
