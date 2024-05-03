namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator
{
    public abstract class UpcomingSelectionIterator<S> : SelectionIterator<S>
    {
        protected bool upcomingCreated = false;
        protected bool hasUpcomingSelection = true;
        protected S upcomingSelection;

        public override bool MoveNext()
        {
            if (!upcomingCreated)
            {
                upcomingSelection = CreateUpcomingSelection();
                upcomingCreated = true;
            }
            return hasUpcomingSelection;
        }

        protected S NoUpcomingSelection()
        {
            hasUpcomingSelection = false;
            return default;
        }

        public override S Current
        {
            get
            {
                if (!hasUpcomingSelection)
                {
                    throw new Exception();
                }
                if (!upcomingCreated)
                {
                    upcomingSelection = CreateUpcomingSelection();
                }
                upcomingCreated = false;
                return upcomingSelection;
            }
        }

        public override string ToString()
        {
            if (!upcomingCreated)
            {
                return "Next upcoming (?)";
            }
            else if (!hasUpcomingSelection)
            {
                return "No next upcoming";
            }
            else
            {
                return "Next upcoming (" + upcomingSelection + ")";
            }
        }

        protected abstract S CreateUpcomingSelection();
    }
}
