using TimefoldSharp.Core.API.Score.Buildin.HardSoft;
using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Schooltimetabling.Domain;

namespace TimefoldSharp.Schooltimetabling.Solver
{
    public class TimeTableConstraintProvider : ConstraintProvider
    {
        public List<Constraint> DefineConstraints(ConstraintFactory constraintFactory)
        {
            return new List<Constraint>() {
                // Hard constraints
                RoomConflict(constraintFactory),
                TeacherConflict(constraintFactory),
                StudentGroupConflict(constraintFactory),
                // Soft constraints
                StudentGroupSubjectVariety(constraintFactory),
                TeacherRoomStability(constraintFactory),
                TeacherTimeEfficiency(constraintFactory),

        };
        }

        Constraint StudentGroupSubjectVariety(ConstraintFactory constraintFactory)
        {
            // A student group dislikes sequential lessons on the same subject.
            return constraintFactory
                    .ForEach<Lesson>(typeof(Lesson))
                    .Join(typeof(Lesson), Joiners.Equal<Lesson>(l => l.Subject),
                                        Joiners.Equal<Lesson>(l => l.StudentGroup),
                                        Joiners.Equal<Lesson>(lesson => lesson.Timeslot.DayOfWeek))

                 .Filter((lesson1, lesson2) =>
                 {
                     var between = lesson1.Timeslot.EndTime - lesson2.Timeslot.StartTime;
                     return !(between.TotalSeconds < 0) && between.CompareTo(TimeSpan.FromMinutes(30)) <= 0;
                 })
                .Penalize(HardSoftScore.ONE_SOFT)
                .AsConstraint("Student group subject variety");
        }

        Constraint TeacherTimeEfficiency(ConstraintFactory constraintFactory)
        {
            // A teacher prefers to teach sequential lessons and dislikes gaps between lessons.
            return constraintFactory
                    .ForEach<Lesson>(typeof(Lesson))
                .Join(typeof(Lesson), Joiners.Equal<Lesson>(l => l.Teacher),
                                        Joiners.Equal<Lesson>((lesson) => lesson.Timeslot.DayOfWeek))
                .Filter((lesson1, lesson2) =>
                {
                    var between = lesson1.Timeslot.EndTime - lesson2.Timeslot.StartTime;
                    return !(between.TotalSeconds < 0) && between.CompareTo(TimeSpan.FromMinutes(30)) <= 0;
                })
                .Reward(HardSoftScore.ONE_SOFT)
                .AsConstraint("Teacher time efficiency");
        }

        Constraint TeacherRoomStability(ConstraintFactory constraintFactory)
        {
            // A teacher prefers to teach in a single room.
            return constraintFactory
                    .ForEachUniquePair(
                        Joiners.Equal<Lesson>(l => l.Teacher))
                .Filter((lesson1, lesson2) => lesson1.Room != lesson2.Room)
                .Penalize(HardSoftScore.ONE_SOFT)
                .AsConstraint("Teacher room stability");
        }

        Constraint RoomConflict(ConstraintFactory constraintFactory)
        {
            // A room can accommodate at most one lesson at the same time.
            return constraintFactory
                    .ForEachUniquePair
                (
                    Joiners.Equal<Lesson>(l => l.Timeslot),
                    Joiners.Equal<Lesson>(l => l.Room))
                 .Penalize(HardSoftScore.ONE_HARD)
                .AsConstraint("Room conflict");
        }

        Constraint TeacherConflict(ConstraintFactory constraintFactory)
        {
            // A teacher can teach at most one lesson at the same time.
            return constraintFactory
                    .ForEachUniquePair(
                        Joiners.Equal<Lesson>(l => l.Timeslot),
                        Joiners.Equal<Lesson>(l => l.Teacher))
                .Penalize(HardSoftScore.ONE_HARD)
                .AsConstraint("Teacher conflict");
        }

        Constraint StudentGroupConflict(ConstraintFactory constraintFactory)
        {
            // A student can attend at most one lesson at the same time.
            return constraintFactory
                    .ForEachUniquePair(
                       Joiners.Equal<Lesson>(l => l.Timeslot),
                        Joiners.Equal<Lesson>(l => l.StudentGroup))
                .Penalize(HardSoftScore.ONE_HARD)
                .AsConstraint("Student group conflict");
        }


    }
}
