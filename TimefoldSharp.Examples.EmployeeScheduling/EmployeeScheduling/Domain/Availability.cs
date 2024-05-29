using TimefoldSharp.Core.API.Domain.Lookup;

namespace TimefoldSharp.Examples.EmployeeScheduling.EmployeeScheduling.Domain
{
    internal class Availability
    {
        [PlanningId]
        long id { get; set; }


        public Employee Employee { get; set; }

        public DateTime Date { get; set; }

        public AvailabilityType AvailabilityType { get; set; }

        public Availability()
        {
        }

        public Availability(Employee employee, DateTime date, AvailabilityType availabilityType, long id)
        {
            this.Employee = employee;
            this.Date = date;
            this.AvailabilityType = availabilityType;
            this.id = id;
        }

        public override string ToString()
        {
            return AvailabilityType + "(" + Employee + ", " + Date + ")";
        }
    }


    public enum AvailabilityType
    {
        DESIRED,
        UNDESIRED,
        UNAVAILABLE
    }
}
