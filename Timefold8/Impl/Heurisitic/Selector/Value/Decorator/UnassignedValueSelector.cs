namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public sealed class UnassignedValueSelector : AbstractInverseEntityFilteringValueSelector
    {
        public UnassignedValueSelector(EntityIndependentValueSelector childValueSelector)
            : base(childValueSelector)
        {
        }

        public override bool IsCountable()
        {
            throw new NotImplementedException();
        }

        public override bool IsNeverEnding()
        {
            throw new NotImplementedException();
        }
    }
}
