using TimefoldSharp.Core.Config.Solver.Termination;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Impl.Domain.Score.Definition;
using TimefoldSharp.Core.Impl.Heurisitic;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public class TerminationFactory
    {
        public static TerminationFactory Create(TerminationConfig terminationConfig)
        {
            return new TerminationFactory(terminationConfig);
        }

        private readonly TerminationConfig terminationConfig;
        private TerminationFactory(TerminationConfig terminationConfig)
        {
            this.terminationConfig = terminationConfig;
        }

        public Termination BuildTermination(HeuristicConfigPolicy configPolicy, Termination chainedTermination)
        {
            Termination termination = BuildTermination(configPolicy);
            if (termination == null)
            {
                return chainedTermination;
            }
            return new OrCompositeTermination(new List<Termination>() { chainedTermination, termination });
        }

        public Termination BuildTermination(HeuristicConfigPolicy configPolicy)
        {
            List<Termination> terminationList = new List<Termination>();
            if (terminationConfig.TerminationClass != null)
            {
                Termination termination =
                        ConfigUtils.NewInstance<Termination>(terminationConfig.TerminationClass);
                terminationList.Add(termination);
            }

            terminationList.AddRange(BuildTimeBasedTermination(configPolicy));

            if (terminationConfig.BestScoreLimit != null)
            {
                ScoreDefinition scoreDefinition = configPolicy.BuilderInfo.GetScoreDefinition();
                API.Score.Score bestScoreLimit_ = scoreDefinition.ParseScore(terminationConfig.BestScoreLimit);
                double[] timeGradientWeightNumbers = Enumerable.Repeat(0.5, scoreDefinition.GetLevelsSize() - 1).ToArray();
                terminationList.Add(new BestScoreTermination(scoreDefinition, bestScoreLimit_, timeGradientWeightNumbers));
            }
            if (terminationConfig.BestScoreFeasible != null)
            {
                ScoreDefinition scoreDefinition = configPolicy.BuilderInfo.GetScoreDefinition();
                if (!terminationConfig.BestScoreFeasible.Value)
                {
                    throw new Exception("The termination bestScoreFeasible ("
                            + terminationConfig.BestScoreFeasible + ") cannot be false.");
                }
                int feasibleLevelsSize = scoreDefinition.GetFeasibleLevelsSize();
                if (feasibleLevelsSize < 1)
                {
                    throw new Exception("The termination with bestScoreFeasible ("
                            + terminationConfig.BestScoreFeasible
                            + ") can only be used with a score type that has at least 1 feasible level but the scoreDefinition ("
                            + scoreDefinition + ") has feasibleLevelsSize (" + feasibleLevelsSize + "), which is less than 1.");
                }
                double[] timeGradientWeightFeasibleNumbers = Enumerable.Repeat(0.5, feasibleLevelsSize - 1).ToArray();
                terminationList.Add(new BestScoreFeasibleTermination(scoreDefinition, timeGradientWeightFeasibleNumbers));
            }
            if (terminationConfig.StepCountLimit != null)
            {
                terminationList.Add(new StepCountTermination(terminationConfig.StepCountLimit.Value));
            }
            if (terminationConfig.ScoreCalculationCountLimit != null)
            {
                terminationList.Add(new ScoreCalculationCountTermination(terminationConfig.ScoreCalculationCountLimit.Value));
            }
            if (terminationConfig.UnimprovedStepCountLimit != null)
            {
                terminationList.Add(new UnimprovedStepCountTermination(terminationConfig.UnimprovedStepCountLimit.Value));
            }

            terminationList.AddRange(BuildInnerTermination(configPolicy));
            return BuildTerminationFromList(terminationList);
        }

        protected Termination BuildTerminationFromList(List<Termination> terminationList)
        {
            if (terminationList.Count == 0)
            {
                return null;
            }
            else if (terminationList.Count() == 1)
            {
                return terminationList[0];
            }
            else
            {
                throw new NotImplementedException();

                /* AbstractCompositeTermination compositeTermination;
                 if (terminationConfig.getTerminationCompositionStyle() == null
                         || terminationConfig.getTerminationCompositionStyle() == TerminationCompositionStyle.OR)
                 {
                     compositeTermination = new OrCompositeTermination<>(terminationList);
                 }
                 else if (terminationConfig.getTerminationCompositionStyle() == TerminationCompositionStyle.AND)
                 {
                     compositeTermination = new AndCompositeTermination<>(terminationList);
                 }
                 else
                 {
                     throw new Exception("The terminationCompositionStyle ("
                             + terminationConfig.TerminationCompositionStyle + ") is not implemented.");
                 }
                 return compositeTermination;*/
            }
        }

        protected List<Termination> BuildInnerTermination(HeuristicConfigPolicy configPolicy)
        {
            if (terminationConfig.TerminationConfigList == null || terminationConfig.TerminationConfigList.Count == 0)
            {
                return new List<Termination>();
            }

            return terminationConfig.TerminationConfigList.Select(config => TerminationFactory.Create(config).BuildTermination(configPolicy))
        .Where(x => x != null)
        .ToList();
        }

        protected List<Termination> BuildTimeBasedTermination(HeuristicConfigPolicy configPolicy)
        {
            List<Termination> terminationList = new List<Termination>();
            var timeMillisSpentLimit = terminationConfig.CalculateTimeMillisSpentLimit();
            if (timeMillisSpentLimit != null)
            {
                terminationList.Add(new TimeMillisSpentTermination(timeMillisSpentLimit.Value));
            }
            var unimprovedTimeMillisSpentLimit = terminationConfig.CalculateUnimprovedTimeMillisSpentLimit();
            if (unimprovedTimeMillisSpentLimit != null)
            {
                if (terminationConfig.UnimprovedScoreDifferenceThreshold == null)
                {
                    terminationList.Add(new UnimprovedTimeMillisSpentTermination(unimprovedTimeMillisSpentLimit.Value));
                }
                else
                {
                    ScoreDefinition scoreDefinition = configPolicy.BuilderInfo.GetScoreDefinition();
                    API.Score.Score unimprovedScoreDifferenceThreshold_ =
                            scoreDefinition.ParseScore(terminationConfig.UnimprovedScoreDifferenceThreshold);
                    if (scoreDefinition.IsNegativeOrZero(unimprovedScoreDifferenceThreshold_))
                    {
                        throw new Exception("The unimprovedScoreDifferenceThreshold ("
                                + terminationConfig.UnimprovedScoreDifferenceThreshold + ") must be positive.");

                    }
                    terminationList.Add(new UnimprovedTimeMillisSpentScoreDifferenceThresholdTermination(
                            unimprovedTimeMillisSpentLimit.Value, unimprovedScoreDifferenceThreshold_));
                }
            }
            else if (terminationConfig.UnimprovedScoreDifferenceThreshold != null)
            {
                throw new Exception("The unimprovedScoreDifferenceThreshold ("
                        + terminationConfig.UnimprovedScoreDifferenceThreshold
                        + ") can only be used if an unimproved*SpentLimit ("
                        + unimprovedTimeMillisSpentLimit + ") is used too.");
            }

            return terminationList;
        }
    }
}
