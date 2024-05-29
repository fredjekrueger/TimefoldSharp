using TimefoldSharp.Core.API.Score.Buildin.HardSoft;
using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Examples.EmployeeScheduling.EmployeeScheduling.Domain;

namespace TimefoldSharp.Examples.EmployeeScheduling.EmployeeScheduling.Solver
{
    internal class EmployeeSchedulingConstraintProvider : ConstraintProvider
    {

        private static int GetMinuteOverlap(Shift shift1, Shift shift2)
        {
            // The overlap of two timeslot occurs in the range common to both timeslots.
            // Both timeslots are active after the higher of their two start times,
            // and before the lower of their two end times.
            DateTime shift1Start = shift1.Start;
            DateTime shift1End = shift1.End;
            DateTime shift2Start = shift2.Start;
            DateTime shift2End = shift2.End;

            DateTime start = shift1Start.CompareTo(shift2Start) > 0 ? shift1Start : shift2Start;
            DateTime end = shift1End.CompareTo(shift2End) < 0 ? shift1End : shift2End;
            return (int)(end - start).TotalMinutes;
        }

        private static int GetShiftDurationInMinutes(Shift shift)
        {
            return (int)(shift.End - shift.Start).TotalMinutes;
        }

        public List<Constraint> DefineConstraints(ConstraintFactory constraintFactory)
        {
            return new List<Constraint>()
            {
                RequiredSkill(constraintFactory),
                NoOverlappingShifts(constraintFactory),
                AtLeast10HoursBetweenTwoShifts(constraintFactory),
                OneShiftPerDay(constraintFactory),
                UnavailableEmployee(constraintFactory),
                DesiredDayForEmployee(constraintFactory),
                UndesiredDayForEmployee(constraintFactory)
            };
        }
        Constraint RequiredSkill(ConstraintFactory constraintFactory)
        {
            return constraintFactory.ForEach<Shift>(typeof(Shift))
                .Filter(shift => !shift.Employee.SkillSet.Contains(shift.RequiredSkill))
                .Penalize(HardSoftScore.ONE_HARD)
                .AsConstraint("Missing required skill");
        }


        Constraint NoOverlappingShifts(ConstraintFactory constraintFactory)
        {
            return constraintFactory.ForEachUniquePair(typeof(Shift), Joiners.Equal<Shift>(s => s.Employee),
                        Joiners.Overlapping<Shift>(s => s.Start, s => s.End))
                .Penalize(HardSoftScore.ONE_HARD, GetMinuteOverlap)
                .AsConstraint("Overlapping shift");
        }

        Constraint AtLeast10HoursBetweenTwoShifts(ConstraintFactory constraintFactory)
        {
            return constraintFactory.ForEachUniquePair(typeof(Shift), Joiners.Equal<Shift>(s => s.Employee),
                            Joiners.LessThanOrEqual<Shift, Shift>(s => s.End, s => s.Start))
                    .Filter((firstShift, secondShift) => (secondShift.Start - firstShift.End).TotalHours < 10)
                    .Penalize(HardSoftScore.ONE_HARD, (firstShift, secondShift) =>
                    {
                        var breakLength = (int)(secondShift.Start - firstShift.End).TotalMinutes;
                        return (10 * 60) - breakLength;
                    })
                    .AsConstraint("At least 10 hours between 2 shifts");
        }

        Constraint OneShiftPerDay(ConstraintFactory constraintFactory)
        {
            return constraintFactory.ForEachUniquePair(typeof(Shift), Joiners.Equal<Shift>(s => s.Employee),
                        Joiners.Equal<Shift>(shift => shift.Start.Date))
                .Penalize(HardSoftScore.ONE_HARD)
                .AsConstraint("Max one shift per day");
        }

        Constraint UnavailableEmployee(ConstraintFactory constraintFactory)
        {
            return constraintFactory.ForEach<Shift>(typeof(Shift))
                .Join(typeof(Availability), Joiners.Equal<Shift, Availability>(shift => shift.Start.Date, a => a.Date),
                                Joiners.Equal<Shift, Availability>(s => s.Employee, a => a.Employee))
                        .Filter((shift, availability) => availability.AvailabilityType == AvailabilityType.UNAVAILABLE)
                        .Penalize(HardSoftScore.ONE_HARD, (shift, availability) => GetShiftDurationInMinutes(shift))
                        .AsConstraint("Unavailable employee");
        }

        Constraint DesiredDayForEmployee(ConstraintFactory constraintFactory)
        {
            return constraintFactory.ForEach<Shift>(typeof(Shift))
                .Join(typeof(Availability), Joiners.Equal<Shift, Availability>(shift => shift.Start.Date, a => a.Date),
                                Joiners.Equal<Shift, Availability>(s => s.Employee, a => a.Employee))
                        .Filter((shift, availability) => availability.AvailabilityType == AvailabilityType.DESIRED)
                        .Reward(HardSoftScore.ONE_SOFT, (shift, availability) => GetShiftDurationInMinutes(shift))
                        .AsConstraint("Desired day for employee");
        }

        Constraint UndesiredDayForEmployee(ConstraintFactory constraintFactory)
        {
            return constraintFactory.ForEach<Shift>(typeof(Shift))
                        .Join(typeof(Availability), Joiners.Equal<Shift, Availability>(shift => shift.Start.Date, a => a.Date),
                                 Joiners.Equal<Shift, Availability>(s => s.Employee, a => a.Employee))
                        .Filter((shift, availability) => availability.AvailabilityType == AvailabilityType.UNDESIRED)
                        .Penalize(HardSoftScore.ONE_SOFT, (shift, availability) => GetShiftDurationInMinutes(shift))
                        .AsConstraint("Undesired day for employee");
        }
    }
}
