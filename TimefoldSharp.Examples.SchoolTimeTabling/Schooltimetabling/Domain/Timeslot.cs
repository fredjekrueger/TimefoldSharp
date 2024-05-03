namespace TimefoldSharp.Schooltimetabling.Domain
{
    public class Timeslot
    {
        public DayOfWeek DayOfWeek { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }



        public Timeslot(DayOfWeek dayOfWeek, DateTime startTime, DateTime endTime)
        {
            this.DayOfWeek = dayOfWeek;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public Timeslot(DayOfWeek dayOfWeek, DateTime startTime) : this(dayOfWeek, startTime, startTime.AddMinutes(50))
        {

        }

        public override string ToString()
        {
            return DayOfWeek + " " + StartTime;
        }
    }
}