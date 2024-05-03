namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.StepCountingHillClimbing
{
    public enum StepCountingHillClimbingType
    {
        /**
         * Every selected move is counted.
         */
        SELECTED_MOVE,
        /**
         * Every accepted move is counted.
         * <p>
         * Note: If {@link LocalSearchForagerConfig#getAcceptedCountLimit()} = 1,
         * then this behaves exactly the same as {link #STEP}.
         */
        ACCEPTED_MOVE,
        /**
         * Every step is counted. Every step was always an accepted move. This is the default.
         */
        STEP,
        /**
         * Every step that equals or improves the {@link Score} of the last step is counted.
         */
        EQUAL_OR_IMPROVING_STEP,
        /**
         * Every step that improves the {@link Score} of the last step is counted.
         */
        IMPROVING_STEP
    }
}
