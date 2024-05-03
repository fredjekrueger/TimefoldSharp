namespace TimefoldSharp.Core.Config.LocalSearch.Decider.Acceptor
{
    public enum AcceptorType
    {
        HILL_CLIMBING,
        ENTITY_TABU,
        VALUE_TABU,
        MOVE_TABU,
        UNDO_MOVE_TABU,
        SIMULATED_ANNEALING,
        LATE_ACCEPTANCE,
        GREAT_DELUGE,
        STEP_COUNTING_HILL_CLIMBING
    }
}
