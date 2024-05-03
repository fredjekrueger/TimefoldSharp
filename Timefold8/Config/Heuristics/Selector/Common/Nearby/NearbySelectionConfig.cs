namespace TimefoldSharp.Core.Config.Heuristics.Selector.Common.Nearby
{
    public class NearbySelectionConfig : SelectorConfig<NearbySelectionConfig>
    {
        public NearbySelectionConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public NearbySelectionConfig Inherit(NearbySelectionConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        internal void ValidateNearby(SelectionCacheType resolvedCacheType, SelectionOrder? resolvedSelectionOrder)
        {
            return;
        }
    }
}
