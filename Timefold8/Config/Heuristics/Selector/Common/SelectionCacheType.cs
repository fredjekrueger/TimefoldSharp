namespace TimefoldSharp.Core.Config.Heuristics.Selector.Common
{
    public enum SelectionCacheType
    {
        JUST_IN_TIME = 0,
        /**
         * When the step is started.
         */
        STEP = 1,
        /**
         * When the phase is started.
         */
        PHASE = 2,
        /**
         * When the solver is started.
         */
        SOLVER = 3
    }

    public static class SelectionCacheTypeHelper
    {
        public static SelectionCacheType Resolve(SelectionCacheType? cacheType, SelectionCacheType minimumCacheType)
        {
            if (cacheType == null)
            {
                return SelectionCacheType.JUST_IN_TIME;
            }
            return cacheType.Value;
        }

        public static bool IsCached(SelectionCacheType type)
        {
            switch (type)
            {
                case SelectionCacheType.JUST_IN_TIME:
                    return false;
                case SelectionCacheType.STEP:
                case SelectionCacheType.PHASE:
                case SelectionCacheType.SOLVER:
                    return true;
                default:
                    throw new Exception("The cacheType (" + type + ") is not implemented.");
            }
        }

        internal static SelectionCacheType Max(SelectionCacheType a, SelectionCacheType b)
        {
            if ((int)a >= (int)b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }
    }
}
