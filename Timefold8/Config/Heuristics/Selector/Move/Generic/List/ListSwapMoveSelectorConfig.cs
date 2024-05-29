namespace TimefoldSharp.Core.Config.Heuristics.Selector.Move.Generic.List
{
    public class ListSwapMoveSelectorConfig : MoveSelectorConfig<ListSwapMoveSelectorConfig>
    {

        public MoveSelectorConfigImpl MoveSelectorConfigImpl { get; set; } = new MoveSelectorConfigImpl();

        public ListSwapMoveSelectorConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public ListSwapMoveSelectorConfig Inherit(ListSwapMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }
    }
}
