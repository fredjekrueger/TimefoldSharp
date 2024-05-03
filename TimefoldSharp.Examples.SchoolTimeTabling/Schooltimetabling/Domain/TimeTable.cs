using TimefoldSharp.Core.API.Domain.Solution;
using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Buildin.HardSoft;

namespace TimefoldSharp.Schooltimetabling.Domain
{
    [PlanningSolution]
    public class TimeTable : ISolution
    {
        [ProblemFactCollectionProperty]
        [ValueRangeProvider]
        public List<Timeslot> TimeslotList { get; set; }
        [ProblemFactCollectionProperty]
        [ValueRangeProvider]
        public List<Room> RoomList { get; set; }
        [PlanningEntityCollectionProperty]
        public List<Lesson> LessonList { get; set; }

        [PlanningScore]
        public HardSoftScore Score { get; set; }

        // No-arg constructor required for Timefold
        public TimeTable()
        {
        }

        public TimeTable(List<Timeslot> timeslotList, List<Room> roomList, List<Lesson> lessonList)
        {
            this.TimeslotList = timeslotList;
            this.RoomList = roomList;
            this.LessonList = lessonList;
        }
    }
}