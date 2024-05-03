using TimefoldSharp.Core.Config.Util;

namespace TimefoldSharp.Core.Config.Solver.Termination
{
    public enum TerminationCompositionStyleEnum { AND, OR }

    public class TerminationConfig : AbstractConfig<TerminationConfig>
    {
        public Type TerminationClass { get; set; } = null;
        public List<TerminationConfig> TerminationConfigList = null;
        public TerminationCompositionStyleEnum? TerminationCompositionStyle { get; set; }
        public string BestScoreLimit { get; set; }
        public bool? BestScoreFeasible { get; set; }
        public int? StepCountLimit { get; set; }
        public int? UnimprovedStepCountLimit { get; set; }
        public long? ScoreCalculationCountLimit { get; set; }


        public TerminationConfig()
        {
            TerminationCompositionStyle = Termination.TerminationCompositionStyleEnum.OR;
        }

        public TerminationConfig CopyConfig()
        {
            return new TerminationConfig().Inherit(this);
        }

        long? millisecondsSpentLimit = null;
        long? secondsSpentLimit = null;
        long? minutesSpentLimit = null;
        long? hoursSpentLimit = null;
        long? daysSpentLimit = null;

        public TimeSpan? SpentLimit { get; set; } = null;

        public long? MillisecondsSpentLimit => millisecondsSpentLimit;
        public long? SecondsSpentLimit => secondsSpentLimit;
        public long? MinutesSpentLimit => minutesSpentLimit;
        public long? HoursSpentLimit => hoursSpentLimit;
        public long? DaysSpentLimit => daysSpentLimit;

        public string UnimprovedScoreDifferenceThreshold { get; set; } = null;

        public TimeSpan? UnimprovedSpentLimit { get; set; }
        public long? UnimprovedMillisecondsSpentLimit { get; set; }
        public long? UnimprovedSecondsSpentLimit { get; set; }
        public long? UnimprovedMinutesSpentLimit { get; set; }
        public long? UnimprovedHoursSpentLimit { get; set; }
        public long? UnimprovedDaysSpentLimit { get; set; }

        private bool TimeSpentLimitIsSet()
        {
            return DaysSpentLimit != null
                    || HoursSpentLimit != null
                    || MinutesSpentLimit != null
                    || SecondsSpentLimit != null
                    || MillisecondsSpentLimit != null
                    || SpentLimit != null;
        }

        private bool UnimprovedTimeSpentLimitIsSet()
        {
            return UnimprovedDaysSpentLimit != null
                    || UnimprovedHoursSpentLimit != null
                    || UnimprovedMinutesSpentLimit != null
                    || UnimprovedSecondsSpentLimit != null
                    || UnimprovedMillisecondsSpentLimit != null
                    || UnimprovedSpentLimit != null;
        }

        public TerminationConfig Inherit(TerminationConfig inheritedConfig)
        {
            if (!TimeSpentLimitIsSet())
            {
                InheritTimeSpentLimit(inheritedConfig);
            }
            if (!UnimprovedTimeSpentLimitIsSet())
            {
                InheritUnimprovedTimeSpentLimit(inheritedConfig);
            }
            TerminationClass = ConfigUtils.InheritOverwritableProperty(TerminationClass, inheritedConfig.TerminationClass);
            TerminationCompositionStyle = ConfigUtils.InheritOverwritableProperty(TerminationCompositionStyle, inheritedConfig.TerminationCompositionStyle);
            UnimprovedScoreDifferenceThreshold = ConfigUtils.InheritOverwritableProperty(UnimprovedScoreDifferenceThreshold, inheritedConfig.UnimprovedScoreDifferenceThreshold);
            BestScoreLimit = ConfigUtils.InheritOverwritableProperty(BestScoreLimit, inheritedConfig.BestScoreLimit);
            BestScoreFeasible = ConfigUtils.InheritOverwritableProperty(BestScoreFeasible, inheritedConfig.BestScoreFeasible);
            StepCountLimit = ConfigUtils.InheritOverwritableProperty(StepCountLimit, inheritedConfig.StepCountLimit);
            UnimprovedStepCountLimit = ConfigUtils.InheritOverwritableProperty(UnimprovedStepCountLimit, inheritedConfig.UnimprovedStepCountLimit);
            ScoreCalculationCountLimit = ConfigUtils.InheritOverwritableProperty(ScoreCalculationCountLimit, inheritedConfig.ScoreCalculationCountLimit);
            TerminationConfigList = ConfigUtils.InheritMergeableListConfig(TerminationConfigList, inheritedConfig.TerminationConfigList);
            return this;
        }

        private TerminationConfig InheritTimeSpentLimit(TerminationConfig parent)
        {
            SpentLimit = ConfigUtils.InheritOverwritableProperty(SpentLimit, parent.SpentLimit);
            millisecondsSpentLimit = ConfigUtils.InheritOverwritableProperty(millisecondsSpentLimit, parent.MillisecondsSpentLimit);
            secondsSpentLimit = ConfigUtils.InheritOverwritableProperty(secondsSpentLimit, parent.SecondsSpentLimit);
            minutesSpentLimit = ConfigUtils.InheritOverwritableProperty(minutesSpentLimit, parent.MinutesSpentLimit);
            hoursSpentLimit = ConfigUtils.InheritOverwritableProperty(hoursSpentLimit, parent.HoursSpentLimit);
            daysSpentLimit = ConfigUtils.InheritOverwritableProperty(daysSpentLimit, parent.DaysSpentLimit);
            return this;
        }

        private TerminationConfig InheritUnimprovedTimeSpentLimit(TerminationConfig parent)
        {
            UnimprovedSpentLimit = ConfigUtils.InheritOverwritableProperty(UnimprovedSpentLimit, parent.UnimprovedSpentLimit);
            UnimprovedMillisecondsSpentLimit = ConfigUtils.InheritOverwritableProperty(UnimprovedMillisecondsSpentLimit, parent.UnimprovedMillisecondsSpentLimit);
            UnimprovedSecondsSpentLimit = ConfigUtils.InheritOverwritableProperty(UnimprovedSecondsSpentLimit, parent.UnimprovedSecondsSpentLimit);
            UnimprovedMinutesSpentLimit = ConfigUtils.InheritOverwritableProperty(UnimprovedMinutesSpentLimit, parent.UnimprovedMinutesSpentLimit);
            UnimprovedHoursSpentLimit = ConfigUtils.InheritOverwritableProperty(UnimprovedHoursSpentLimit, parent.UnimprovedHoursSpentLimit);
            UnimprovedDaysSpentLimit = ConfigUtils.InheritOverwritableProperty(UnimprovedDaysSpentLimit, parent.UnimprovedDaysSpentLimit);
            return this;
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            classVisitor.Invoke(TerminationClass);
            if (TerminationConfigList != null)
            {
                TerminationConfigList.ForEach(tc => tc.VisitReferencedClasses(classVisitor));
            }
        }

        public bool IsConfigured()
        {
            return TerminationClass != null ||
                    TimeSpentLimitIsSet() ||
                    UnimprovedTimeSpentLimitIsSet() ||
                    BestScoreLimit != null ||
                    BestScoreFeasible != null ||
                    StepCountLimit != null ||
                    UnimprovedStepCountLimit != null ||
                    ScoreCalculationCountLimit != null ||
                    IsTerminationListConfigured();
        }

        private bool IsTerminationListConfigured()
        {
            if (TerminationConfigList == null || TerminationCompositionStyle == null)
            {
                return false;
            }

            switch (TerminationCompositionStyle)
            {
                case TerminationCompositionStyleEnum.AND:
                    return TerminationConfigList.All(t => t.IsConfigured());
                case TerminationCompositionStyleEnum.OR:
                    return TerminationConfigList.Any(t => t.IsConfigured());
                default:
                    throw new Exception("Unhandled case (" + TerminationCompositionStyle + ").");
            }
        }

        public double? CalculateTimeMillisSpentLimit()
        {
            if (millisecondsSpentLimit == null && secondsSpentLimit == null
                    && minutesSpentLimit == null && hoursSpentLimit == null && daysSpentLimit == null)
            {
                if (SpentLimit != null)
                {
                    return SpentLimit.Value.TotalMilliseconds;
                }
                return null;
            }
            if (SpentLimit != null)
            {
                throw new Exception("The termination spentLimit (" + SpentLimit
                        + ") cannot be combined with millisecondsSpentLimit (" + millisecondsSpentLimit
                        + "), secondsSpentLimit (" + secondsSpentLimit
                        + "), minutesSpentLimit (" + minutesSpentLimit
                        + "), hoursSpentLimit (" + hoursSpentLimit
                        + ") or daysSpentLimit (" + daysSpentLimit + ").");
            }
            long timeMillisSpentLimit = 0L
                    + RequireNonNegative(millisecondsSpentLimit, "millisecondsSpentLimit")
                    + RequireNonNegative(secondsSpentLimit, "secondsSpentLimit") * 1_000L
                    + RequireNonNegative(minutesSpentLimit, "minutesSpentLimit") * 60_000L
                    + RequireNonNegative(hoursSpentLimit, "hoursSpentLimit") * 3_600_000L
                    + RequireNonNegative(daysSpentLimit, "daysSpentLimit") * 86_400_000L;
            return timeMillisSpentLimit;
        }

        internal double? CalculateUnimprovedTimeMillisSpentLimit()
        {
            if (UnimprovedMillisecondsSpentLimit == null && UnimprovedSecondsSpentLimit == null
                && UnimprovedMinutesSpentLimit == null && UnimprovedHoursSpentLimit == null)
            {
                if (UnimprovedSpentLimit != null)
                {
                    return UnimprovedSpentLimit.Value.TotalMilliseconds;
                }
                return null;
            }
            if (UnimprovedSpentLimit != null)
            {
                throw new Exception("The termination unimprovedSpentLimit (" + UnimprovedSpentLimit
                        + ") cannot be combined with unimprovedMillisecondsSpentLimit (" + UnimprovedMillisecondsSpentLimit
                        + "), unimprovedSecondsSpentLimit (" + UnimprovedSecondsSpentLimit
                        + "), unimprovedMinutesSpentLimit (" + UnimprovedMinutesSpentLimit
                        + "), unimprovedHoursSpentLimit (" + UnimprovedHoursSpentLimit + ").");
            }
            long unimprovedTimeMillisSpentLimit = 0L
                    + RequireNonNegative(UnimprovedMillisecondsSpentLimit, "unimprovedMillisecondsSpentLimit")
                    + RequireNonNegative(UnimprovedSecondsSpentLimit, "unimprovedSecondsSpentLimit") * 1000L
                    + RequireNonNegative(UnimprovedMinutesSpentLimit, "unimprovedMinutesSpentLimit") * 60_000L
                    + RequireNonNegative(UnimprovedHoursSpentLimit, "unimprovedHoursSpentLimit") * 3_600_000L
                    + RequireNonNegative(UnimprovedDaysSpentLimit, "unimprovedDaysSpentLimit") * 86_400_000L;
            return unimprovedTimeMillisSpentLimit;
        }

        private long RequireNonNegative(long? param, string name)
        {
            if (param == null)
            {
                return 0L; // Makes adding a null param a NOP.
            }
            else if (param < 0L)
            {
                string msg = string.Format("The termination %s (%d) cannot be negative.", name, param);
                throw new Exception(msg);
            }
            else
            {
                return param.Value;
            }
        }
    }
}
