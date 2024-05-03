using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Config.Heuristics.Selector.Value;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Mimic;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Mimic;
using TimefoldSharp.Core.Impl.Score.Trend;
using TimefoldSharp.Core.Impl.Solver;

namespace TimefoldSharp.Core.Impl.Heurisitic
{
    public class HeuristicConfigPolicy
    {

        private readonly string logIndentation;
        private readonly Dictionary<string, EntityMimicRecorder> entityMimicRecorderMap = new Dictionary<string, EntityMimicRecorder>();
        private readonly ValueSorterManner valueSorterManner;
        private readonly bool reinitializeVariableFilterEnabled;
        private readonly EnvironmentMode? environmentMode;
        private readonly SolutionDescriptor solutionDescriptor;

        private readonly Dictionary<string, ValueMimicRecorder> valueMimicRecorderMap = new Dictionary<string, ValueMimicRecorder>();

        public HeuristicConfigPolicy(Builder builder)
        {
            this.BuilderInfo = builder;
        }

        public ValueSorterManner GetValueSorterManner()
        {
            return valueSorterManner;
        }

        public EnvironmentMode? GetEnvironmentMode()
        {
            return environmentMode;
        }

        public bool IsReinitializeVariableFilterEnabled()
        {
            return reinitializeVariableFilterEnabled;
        }

        public Domain.Score.Definition.ScoreDefinition GetScoreDefinition()
        {
            return solutionDescriptor.GetScoreDefinition();
        }

        public Builder CloneBuilder()
        {
            return new Builder(BuilderInfo.EnvironmentMode, BuilderInfo.MoveThreadCount, BuilderInfo.MoveThreadBufferSize, BuilderInfo.ThreadFactoryClass, BuilderInfo.InitializingScoreTrend,
                    BuilderInfo.SolutionDescriptor, BuilderInfo.ClassInstanceCache).WithLogIndentation(BuilderInfo.LogIndentation);
        }
        public String GetLogIndentation()
        {
            return logIndentation;
        }

        internal void AddEntityMimicRecorder(string id, MimicRecordingEntitySelector mimicRecordingEntitySelector)
        {
            if (entityMimicRecorderMap.ContainsKey(id))
            {
                throw new Exception("Multiple  Maybe specify a variable name for the mimicking selector in situations with multiple variables on the same entity?");
            }
            entityMimicRecorderMap.Add(id, mimicRecordingEntitySelector);
        }



        public void AddValueMimicRecorder(String id, ValueMimicRecorder mimicRecordingValueSelector)
        {
            if (valueMimicRecorderMap.ContainsKey(id))
            {
                throw new Exception("Multiple Maybe specify a variable name for the mimicking selector in situations with multiple variables on the same entity?");
            }
            valueMimicRecorderMap.Add(id, mimicRecordingValueSelector);
        }

        public ValueMimicRecorder GetValueMimicRecorder(String id)
        {
            ValueMimicRecorder item = null;

            valueMimicRecorderMap.TryGetValue(id, out item);

            return item;
        }

        public EntityMimicRecorder GetEntityMimicRecorder(string id)
        {
            EntityMimicRecorder item = null;
            entityMimicRecorderMap.TryGetValue(id, out item);
            return item;
        }

        public HeuristicConfigPolicy CreatePhaseConfigPolicy()
        {
            return CloneBuilder().Build();
        }

        public Builder BuilderInfo { get; set; }

        public class Builder
        {
            public EnvironmentMode EnvironmentMode { get; set; }
            public int? MoveThreadCount { get; set; }
            public int? MoveThreadBufferSize { get; set; }
            public Type ThreadFactoryClass { get; set; }
            public InitializingScoreTrend InitializingScoreTrend { get; set; }
            public SolutionDescriptor SolutionDescriptor { get; set; }
            public ClassInstanceCache ClassInstanceCache { get; set; }

            public string LogIndentation = "";
            public EntitySorterManner EntitySorterManner = EntitySorterManner.NONE;
            public ValueSorterManner ValueSorterManner = ValueSorterManner.NONE;
            public bool ReinitializeVariableFilterEnabled = false;
            public bool InitializedChainedValueFilterEnabled = false;
            public bool UnassignedValuesAllowed = false;

            public Builder(EnvironmentMode environmentMode, int? moveThreadCount, int? moveThreadBufferSize,
                 Type threadFactoryClass, InitializingScoreTrend initializingScoreTrend,
                 SolutionDescriptor solutionDescriptor, ClassInstanceCache classInstanceCache)
            {
                this.EnvironmentMode = environmentMode;
                this.MoveThreadCount = moveThreadCount;
                this.MoveThreadBufferSize = moveThreadBufferSize;
                this.ThreadFactoryClass = threadFactoryClass;
                this.InitializingScoreTrend = initializingScoreTrend;
                this.SolutionDescriptor = solutionDescriptor;
                this.ClassInstanceCache = classInstanceCache;
            }

            public Builder WithLogIndentation(String logIndentation)
            {
                this.LogIndentation = logIndentation;
                return this;
            }

            public Builder WithEntitySorterManner(EntitySorterManner entitySorterManner)
            {
                this.EntitySorterManner = entitySorterManner;
                return this;
            }

            public Builder WithValueSorterManner(ValueSorterManner valueSorterManner)
            {
                this.ValueSorterManner = valueSorterManner;
                return this;
            }

            public Domain.Score.Definition.ScoreDefinition GetScoreDefinition()
            {
                return SolutionDescriptor.GetScoreDefinition();
            }

            public Builder WithReinitializeVariableFilterEnabled(bool reinitializeVariableFilterEnabled)
            {
                this.ReinitializeVariableFilterEnabled = reinitializeVariableFilterEnabled;
                return this;
            }

            public Builder WithInitializedChainedValueFilterEnabled(bool initializedChainedValueFilterEnabled)
            {
                this.InitializedChainedValueFilterEnabled = initializedChainedValueFilterEnabled;
                return this;
            }

            public Builder WithUnassignedValuesAllowed(bool unassignedValuesAllowed)
            {
                this.UnassignedValuesAllowed = unassignedValuesAllowed;
                return this;
            }

            public HeuristicConfigPolicy Build()
            {
                return new HeuristicConfigPolicy(this);
            }

            public EntitySorterManner GetEntitySorterManner()
            {
                return EntitySorterManner;
            }

            public ClassInstanceCache GetClassInstanceCache()
            {
                return ClassInstanceCache;
            }

            public InitializingScoreTrend GetInitializingScoreTrend()
            {
                return InitializingScoreTrend;
            }
        }
    }
}
