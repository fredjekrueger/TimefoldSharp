using System.Reflection;
using TimefoldSharp.Core.API.Domain.Solution;
using TimefoldSharp.Core.API.Score.Buildin.HardSoft;
using TimefoldSharp.Core.API.Score.Buildin.Simple;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Policy;
using TimefoldSharp.Core.Impl.Score.Buidin;
using static TimefoldSharp.Core.API.Domain.Solution.PlanningScoreAttribute;
using static TimefoldSharp.Core.Impl.Domain.Common.Accessor.MemberAccessorFactory;

namespace TimefoldSharp.Core.Impl.Domain.Score.Descriptor
{
    public class ScoreDescriptor
    {
        public Definition.ScoreDefinition ScoreDefinition { get; set; }
        private readonly MemberAccessor scoreMemberAccessor;

        private ScoreDescriptor(MemberAccessor scoreMemberAccessor, Definition.ScoreDefinition scoreDefinition)
        {
            this.scoreMemberAccessor = scoreMemberAccessor;
            this.ScoreDefinition = scoreDefinition;
        }

        public void SetScore(object solution, API.Score.Score score)
        {
            scoreMemberAccessor.ExecuteSetter(solution, score);
        }

        public static ScoreDescriptor BuildScoreDescriptor(DescriptorPolicy descriptorPolicy, MemberInfo member, Type solutionClass)
        {
            MemberAccessor scoreMemberAccessor = BuildScoreMemberAccessor(descriptorPolicy, member, solutionClass);
            Type scoreType = ExtractScoreType(scoreMemberAccessor, solutionClass);
            PlanningScoreAttribute annotation = ExtractPlanningScoreAnnotation(scoreMemberAccessor);
            Definition.ScoreDefinition scoreDefinition = BuildScoreDefinition(solutionClass, scoreMemberAccessor, scoreType, annotation);
            return new ScoreDescriptor(scoreMemberAccessor, scoreDefinition);
        }

        public API.Score.Score GetScore(Object solution)
        {
            return (API.Score.Score)scoreMemberAccessor.ExecuteGetter(solution);
        }

        private static Definition.ScoreDefinition BuildScoreDefinition(Type solutionClass, MemberAccessor scoreMemberAccessor, Type scoreType, PlanningScoreAttribute annotation)
        {
            Type scoreDefinitionClass = annotation.ScoreDefinitionClass;
            int bendableHardLevelsSize = annotation.BendableHardLevelsSize;
            int bendableSoftLevelsSize = annotation.BendableSoftLevelsSize;
            if (scoreDefinitionClass != typeof(PlanningScoreAttribute.NullScoreDefinition))
            {
                if (bendableHardLevelsSize != NO_LEVEL_SIZE || bendableSoftLevelsSize != NO_LEVEL_SIZE)
                {
                    throw new Exception("The solutionClass (" + solutionClass + ").");
                }
                return ConfigUtils.NewInstance<Definition.ScoreDefinition>(scoreDefinitionClass);
            }
            //if (!typeof(IBendableScore<AbstractScore>).IsAssignableFrom(scoreType))
            if (true)
            {
                if (bendableHardLevelsSize != NO_LEVEL_SIZE || bendableSoftLevelsSize != NO_LEVEL_SIZE)
                {
                    throw new Exception("The solutionClass ).");
                }
                if (scoreType == typeof(SimpleScore))
                {
                    return new SimpleScoreDefinition();
                }
                /*else if (scoreType == typeof(SimpleLongScore))
                {
                    return new SimpleLongScoreDefinition();
                }
                else if (scoreType == typeof(SimpleBigDecimalScore))
                {
                    return new SimpleBigDecimalScoreDefinition();
                }*/
                else if (scoreType == typeof(HardSoftScore))
                {
                    return new HardSoftScoreDefinition();
                }
                /*else if (scoreType == typeof(HardSoftLongScore))
                 {
                     return new HardSoftLongScoreDefinition();
                 }
                 else if (scoreType == typeof(HardSoftBigDecimalScore))
                 {
                     return new HardSoftBigDecimalScoreDefinition();
                 }
                 else if (scoreType == typeof(HardMediumSoftScore))
                 {
                     return new HardMediumSoftScoreDefinition();
                 }
                 else if (scoreType == typeof(HardMediumSoftLongScore))
                 {
                     return new HardMediumSoftLongScoreDefinition();
                 }
                 else if (scoreType == typeof(HardMediumSoftBigDecimalScore))
                 {
                     return new HardMediumSoftBigDecimalScoreDefinition();
                 }*/
                else
                {
                    throw new Exception("The solutionClass ( + soation.");
                }
            }
            else
            {
                /*if (bendableHardLevelsSize == NO_LEVEL_SIZE || bendableSoftLevelsSize == NO_LEVEL_SIZE)
                {
                    throw new Exception("The solutionCla).");
                }
                if (scoreType == typeof(BendableScore))
                {
                    return new BendableScoreDefinition(bendableHardLevelsSize, bendableSoftLevelsSize);
                }
                else if (scoreType == typeof(BendableLongScore))
                {
                    return new BendableLongScoreDefinition(bendableHardLevelsSize, bendableSoftLevelsSize);
                }
                else if (scoreType == typeof(BendableBigDecimalScore))
                {
                    return new BendableBigDecimalScoreDefinition(bendableHardLevelsSize, bendableSoftLevelsSize);
                }
                else
                {
                    throw new Exception("The solutionClas implementation.\n"
                            + "  If you intend to use a custom implementation,"
                            + " maybe set a scoreDefinition in the annotation.");
                }*/
            }
        }

        private static PlanningScoreAttribute ExtractPlanningScoreAnnotation(MemberAccessor scoreMemberAccessor)
        {
            PlanningScoreAttribute annotation = scoreMemberAccessor.GetAnnotation<PlanningScoreAttribute>(typeof(PlanningScoreAttribute));
            if (annotation != null)
            {
                return annotation;
            }
            throw new Exception();
            // The member was auto-discovered.
            /*try {
                return ScoreDescriptor.class.getDeclaredField("PLANNING_SCORE").getAnnotation(typeof(PlanningScoreAttribute));
            } catch (NoSuchFieldException e) {
        throw new IllegalStateException("Impossible situation: the field (PLANNING_SCORE) must exist.", e);*/
        }


        private static Type ExtractScoreType(MemberAccessor scoreMemberAccessor, Type solutionClass)
        {
            Type memberType = scoreMemberAccessor.GetClass();
            return memberType;
        }

        private static MemberAccessor BuildScoreMemberAccessor(DescriptorPolicy descriptorPolicy, MemberInfo member, Type solutionClass)
        {
            return descriptorPolicy.MemberAccessorFactory.BuildAndCacheMemberAccessor(solutionClass,
                    member,
                    MemberAccessorType.PROPERTY_OR_GETTER_METHOD_WITH_SETTER,
                    typeof(PlanningScoreAttribute));
        }
    }
}
