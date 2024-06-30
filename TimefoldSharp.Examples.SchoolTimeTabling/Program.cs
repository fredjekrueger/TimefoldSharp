using Serilog;
using TimefoldSharp.Core.API.Solver;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Schooltimetabling.Domain;
using TimefoldSharp.Schooltimetabling.Solver;

internal class Program
{
    static void Main(string[] args)
    {
        CreateLogger();

        var config = new SolverConfig()
        .WithSolutionClass(typeof(TimeTable))
        .WithEntityClasses(typeof(Lesson))
        .WithConstraintProviderClass(typeof(TimeTableConstraintProvider))
        .WithTerminationSpentLimit(TimeSpan.FromSeconds(2));

        SolverFactory solverFactory = SolverFactory.Create(config);
        TimeTable problem = GenerateDemoData();

        Solver solver = solverFactory.BuildSolver();
        TimeTable solution = (TimeTable)solver.Solve(problem);

        PrintTimetable(solution);
        Log.Information(solver.GetBestScore().ToString());
        Console.ReadLine();
    }

    private static void CreateLogger()
    {
        Log.Logger = new LoggerConfiguration()
                            // add console as logging target
                            .WriteTo.Console()
                            // set default minimum level
                            .MinimumLevel.Debug()
                            .CreateLogger();
    }

    private static void PrintTimetable(TimeTable timeTable)
    {
        string info = Environment.NewLine;
        List<Room> roomList = timeTable.RoomList;
        List<Lesson> lessonList = timeTable.LessonList;
        Dictionary<Timeslot, Dictionary<Room, List<Lesson>>> lessonMap = lessonList
                .Where(l => l.Timeslot != null && l.Room != null).GroupBy(l => l.Timeslot)
        .ToDictionary(group => group.Key,
              group => group.GroupBy(lesson => lesson.Room)
                             .ToDictionary(innerGroup => innerGroup.Key,
                                           innerGroup => innerGroup.ToList()));

        info += "|            | " + roomList.Select(room => String.Format("{0,-10}", room.GetName())).Aggregate((current, next) => current + " | " + next) + " |" + Environment.NewLine;
        info += "|" + string.Concat(Enumerable.Repeat("|------------|", roomList.Count + 1)) + Environment.NewLine;
        foreach (var timeslot in timeTable.TimeslotList)
        {
            List<List<Lesson>> cellList = roomList.Select(room =>
            {
                Dictionary<Room, List<Lesson>> byRoomMap;
                lessonMap.TryGetValue(timeslot, out byRoomMap);
                if (byRoomMap == null)
                {
                    return new List<Lesson>();
                }
                List<Lesson> cellLessonList;
                byRoomMap.TryGetValue(room, out cellLessonList);
                return cellLessonList ?? new List<Lesson>();
            }).ToList();

            if (cellList.Count > 0)
            {
                info += ("| " + String.Format("{0,-10}", timeslot.DayOfWeek.ToString().Substring(0, 3) + " " + timeslot.StartTime.Hour + ":" + timeslot.StartTime.Minute) + " | "
    + cellList.Select(cellLessonList => String.Format("{0,-10}",
    cellLessonList.Any() ? cellLessonList.Select(lesson => lesson.Subject).Aggregate((current, next) => current + ", " + next) : ""))
        .Aggregate((current, next) => current + " | " + next) + " |") + Environment.NewLine;
                {
                    info += ("|            | "
                        + cellList.Select(cellLessonList => String.Format("{0,-10}",
                               cellLessonList.Any() ? cellLessonList.Select(lesson => lesson.Teacher).Aggregate((current, next) => current + ", " + next) : ""))
                                .Aggregate((current, next) => current + " | " + next)
                        + " |") + Environment.NewLine;
                }
                info += ("|            | "
                + cellList.Select(cellLessonList => String.Format("{0,-10}",
                        cellLessonList.Any() ? cellLessonList.Select(Lesson => Lesson.StudentGroup).Aggregate((current, next) => current + ", " + next) : ""))
                        .Aggregate((current, next) => current + " | " + next)
                + " |") + Environment.NewLine;
            }
            info += "|" + string.Concat(Enumerable.Repeat("|", roomList.Count + 1)) + Environment.NewLine;
        }
        List<Lesson> unassignedLessons = lessonList.Where(l => l.Timeslot == null || l.Room == null)
                .ToList();
        if (unassignedLessons.Any())
        {
            info += Environment.NewLine;
            info += ("Unassigned lessons") + Environment.NewLine;
            foreach (var lesson in unassignedLessons)
            {
                info += "  " + lesson.Subject + " - " + lesson.Teacher + " - " + lesson.StudentGroup + Environment.NewLine;
            }
        }
        Log.Information(info);
    }

    public static TimeTable GenerateDemoData()
    {
        List<Timeslot> timeslotList = new List<Timeslot>
            {
                new Timeslot(DayOfWeek.Monday, new DateTime(2023, 1, 1, 8, 30, 0), new DateTime(2023, 1, 1, 9, 30, 0)),
                new Timeslot(DayOfWeek.Monday, new DateTime(2023, 1, 1, 9, 30, 0), new DateTime(2023, 1, 1, 10, 30, 0)),
                new Timeslot(DayOfWeek.Monday, new DateTime(2023, 1, 1, 10, 30, 0), new DateTime(2023, 1, 1, 11, 30, 0)),
                new Timeslot(DayOfWeek.Monday, new DateTime(2023, 1, 1, 13, 30, 0), new DateTime(2023, 1, 1, 14, 30, 0)),
                new Timeslot(DayOfWeek.Monday, new DateTime(2023, 1, 1, 14, 30, 0), new DateTime(2023, 1, 1, 15, 30, 0)),
                new Timeslot(DayOfWeek.Tuesday, new DateTime(2023, 1, 1, 8, 30, 0), new DateTime(2023, 1, 1, 9, 30, 0)),
                new Timeslot(DayOfWeek.Tuesday, new DateTime(2023, 1, 1, 9, 30, 0), new DateTime(2023, 1, 1, 10, 30, 0)),
                new Timeslot(DayOfWeek.Tuesday, new DateTime(2023, 1, 1, 10, 30, 0), new DateTime(2023, 1, 1, 11, 30, 0)),
                new Timeslot(DayOfWeek.Tuesday, new DateTime(2023, 1, 1, 13, 30, 0), new DateTime(2023, 1, 1, 14, 30, 0)),
                new Timeslot(DayOfWeek.Tuesday, new DateTime(2023, 1, 1, 14, 30, 0), new DateTime(2023, 1, 1, 15, 30, 0))
            };

        List<Room> roomList = new List<Room>()
            {
                new Room("Room A"),
                new Room("Room B"),
                new Room("Room C")
            };

        List<Lesson> lessonList = new List<Lesson>();
        long id = 0;
        lessonList.Add(new Lesson(id++, "Math", "A. Turing", "9th grade"));
        lessonList.Add(new Lesson(id++, "Math", "A. Turing", "9th grade"));
        lessonList.Add(new Lesson(id++, "Physics", "M. Curie", "9th grade"));
        lessonList.Add(new Lesson(id++, "Chemistry", "M. Curie", "9th grade"));
        lessonList.Add(new Lesson(id++, "Biology", "C. Darwin", "9th grade"));
        lessonList.Add(new Lesson(id++, "History", "I. Jones", "9th grade"));
        lessonList.Add(new Lesson(id++, "English", "I. Jones", "9th grade"));
        lessonList.Add(new Lesson(id++, "English", "I. Jones", "9th grade"));
        lessonList.Add(new Lesson(id++, "Spanish", "P. Cruz", "9th grade"));
        lessonList.Add(new Lesson(id++, "Spanish", "P. Cruz", "9th grade"));
        lessonList.Add(new Lesson(id++, "Math", "A. Turing", "10th grade"));
        lessonList.Add(new Lesson(id++, "Math", "A. Turing", "10th grade"));
        lessonList.Add(new Lesson(id++, "Math", "A. Turing", "10th grade"));
        lessonList.Add(new Lesson(id++, "Physics", "M. Curie", "10th grade"));
        lessonList.Add(new Lesson(id++, "Chemistry", "M. Curie", "10th grade"));
        lessonList.Add(new Lesson(id++, "French", "M. Curie", "10th grade"));
        lessonList.Add(new Lesson(id++, "Geography", "C. Darwin", "10th grade"));
        lessonList.Add(new Lesson(id++, "History", "I. Jones", "10th grade"));
        lessonList.Add(new Lesson(id++, "English", "P. Cruz", "10th grade"));
        lessonList.Add(new Lesson(id++, "Spanish", "P. Cruz", "10th grade"));

        return new TimeTable(timeslotList, roomList, lessonList);
    }
}