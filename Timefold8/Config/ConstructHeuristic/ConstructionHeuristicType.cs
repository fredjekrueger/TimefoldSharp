using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Config.Heuristics.Selector.Value;

namespace TimefoldSharp.Core.Config.ConstructHeuristic
{
    public enum ConstructionHeuristicType
    {
        /**
         * A specific form of {@link #ALLOCATE_ENTITY_FROM_QUEUE}.
         */
        FIRST_FIT,
        /**
         * A specific form of {@link #ALLOCATE_ENTITY_FROM_QUEUE}.
         */
        FIRST_FIT_DECREASING,
        /**
         * A specific form of {@link #ALLOCATE_ENTITY_FROM_QUEUE}.
         */
        WEAKEST_FIT,
        /**
         * A specific form of {@link #ALLOCATE_ENTITY_FROM_QUEUE}.
         */
        WEAKEST_FIT_DECREASING,
        /**
         * A specific form of {@link #ALLOCATE_ENTITY_FROM_QUEUE}.
         */
        STRONGEST_FIT,
        /**
         * A specific form of {@link #ALLOCATE_ENTITY_FROM_QUEUE}.
         */
        STRONGEST_FIT_DECREASING,
        /**
         * Put all entities in a queue.
         * Assign the first entity (from that queue) to the best value.
         * Repeat until all entities are assigned.
         */
        ALLOCATE_ENTITY_FROM_QUEUE,
        /**
         * Put all values in a round-robin queue.
         * Assign the best entity to the first value (from that queue).
         * Repeat until all entities are assigned.
         */
        ALLOCATE_TO_VALUE_FROM_QUEUE,
        /**
         * A specific form of {@link #ALLOCATE_FROM_POOL}.
         */
        CHEAPEST_INSERTION,
        /**
         * Put all entity-value combinations in a pool.
         * Assign the best entity to best value.
         * Repeat until all entities are assigned.
         */
        ALLOCATE_FROM_POOL
    }

    public static class ConstructionHeuristicTypeHelper
    {
        public static EntitySorterManner GetDefaultEntitySorterManner(ConstructionHeuristicType t)
        {
            switch (t)
            {
                case ConstructionHeuristicType.FIRST_FIT:
                case ConstructionHeuristicType.WEAKEST_FIT:
                case ConstructionHeuristicType.STRONGEST_FIT:
                    return EntitySorterManner.NONE;
                case ConstructionHeuristicType.FIRST_FIT_DECREASING:
                case ConstructionHeuristicType.WEAKEST_FIT_DECREASING:
                case ConstructionHeuristicType.STRONGEST_FIT_DECREASING:
                    return EntitySorterManner.DECREASING_DIFFICULTY;
                case ConstructionHeuristicType.ALLOCATE_ENTITY_FROM_QUEUE:
                case ConstructionHeuristicType.ALLOCATE_TO_VALUE_FROM_QUEUE:
                case ConstructionHeuristicType.CHEAPEST_INSERTION:
                case ConstructionHeuristicType.ALLOCATE_FROM_POOL:
                    return EntitySorterManner.DECREASING_DIFFICULTY_IF_AVAILABLE;
                default:
                    throw new Exception("The constructionHeuristicType is not implemented.");
            }
        }

        public static ValueSorterManner GetDefaultValueSorterManner(ConstructionHeuristicType v)
        {
            switch (v)
            {
                case ConstructionHeuristicType.FIRST_FIT:
                case ConstructionHeuristicType.FIRST_FIT_DECREASING:
                    return ValueSorterManner.NONE;
                case ConstructionHeuristicType.WEAKEST_FIT:
                case ConstructionHeuristicType.WEAKEST_FIT_DECREASING:
                    return ValueSorterManner.INCREASING_STRENGTH;
                case ConstructionHeuristicType.STRONGEST_FIT:
                case ConstructionHeuristicType.STRONGEST_FIT_DECREASING:
                    return ValueSorterManner.DECREASING_STRENGTH;
                case ConstructionHeuristicType.ALLOCATE_ENTITY_FROM_QUEUE:
                case ConstructionHeuristicType.ALLOCATE_TO_VALUE_FROM_QUEUE:
                case ConstructionHeuristicType.CHEAPEST_INSERTION:
                case ConstructionHeuristicType.ALLOCATE_FROM_POOL:
                    return ValueSorterManner.INCREASING_STRENGTH_IF_AVAILABLE;
                default:
                    throw new Exception("The constructionHeuristicType (" + v + ") is not implemented.");
            }
        }
    }
}
