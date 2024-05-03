namespace TimefoldSharp.Core.Config.Solver
{
    public enum EnvironmentMode
    {
        FULL_ASSERT = 0, NON_INTRUSIVE_FULL_ASSERT = 1, FAST_ASSERT = 2, REPRODUCIBLE = 4, NON_REPRODUCIBLE = 8
    }

    public static class EnvironmentModeEnumHelper
    {
        public static bool IsAsserted(EnvironmentMode environmentMode_)
        {
            return environmentMode_ == EnvironmentMode.FAST_ASSERT || environmentMode_ == EnvironmentMode.NON_INTRUSIVE_FULL_ASSERT || environmentMode_ == EnvironmentMode.FULL_ASSERT;
        }

        public static bool IsIntrusiveFastAsserted(EnvironmentMode environmentMode_)
        {
            if (!IsAsserted(environmentMode_))
            {
                return false;
            }
            return environmentMode_ != EnvironmentMode.NON_INTRUSIVE_FULL_ASSERT;
        }

        public static bool IsNonIntrusiveFullAsserted(EnvironmentMode environmentMode_)
        {
            if (!IsAsserted(environmentMode_))
            {
                return false;
            }
            return environmentMode_ != EnvironmentMode.FAST_ASSERT;
        }
    }
}
