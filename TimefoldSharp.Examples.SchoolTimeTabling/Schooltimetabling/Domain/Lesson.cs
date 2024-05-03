using TimefoldSharp.Core.API.Domain.Entity;
using TimefoldSharp.Core.API.Domain.Lookup;
using TimefoldSharp.Core.API.Domain.Variable;

namespace TimefoldSharp.Schooltimetabling.Domain
{
    [PlanningEntity]
    public class Lesson
    {
        [PlanningId]
        public long ID { get; set; }

        public string Subject { get; set; }
        public string Teacher { get; set; }
        public string StudentGroup { get; set; }

        [PlanningVariable]
        public Timeslot Timeslot { get; set; }
        [PlanningVariable]
        public Room Room { get; set; }

        public Lesson()
        {
        }


        public Lesson(long id, string subject, string teacher, string studentGroup)
        {
            this.ID = id;
            this.Subject = subject;
            this.Teacher = teacher;
            this.StudentGroup = studentGroup;
        }

        public Lesson(long id, string subject, string teacher, string studentGroup, Timeslot timeslot, Room room) : this(id, subject, teacher, studentGroup)
        {
            this.Timeslot = timeslot;
            this.Room = room;
        }

        public override string ToString()
        {
            return Subject + "(" + ID + ")";
        }
    }
}
