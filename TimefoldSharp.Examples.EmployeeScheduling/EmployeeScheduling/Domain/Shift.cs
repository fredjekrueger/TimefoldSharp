using TimefoldSharp.Core.API.Domain.Entity;
using TimefoldSharp.Core.API.Domain.Lookup;
using TimefoldSharp.Core.API.Domain.Variable;

namespace TimefoldSharp.Examples.EmployeeScheduling.EmployeeScheduling.Domain
{
    [PlanningEntity(PinningFilter = typeof(ShiftPinningFilter))]
    internal class Shift
    {
        [PlanningId]
        public long Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Location { get; set; }
        public string RequiredSkill { get; set; }

        [PlanningVariable]
        public Employee Employee { get; set; }

        public Shift()
        {
        }

        public Shift(long id, DateTime start, DateTime end, string location, string requiredSkill) : this(id, start, end, location, requiredSkill, null)
        {
        }

        public Shift(long id, DateTime start, DateTime end, string location, string requiredSkill, Employee employee)
        {
            this.Id = id;
            this.Start = start;
            this.End = end;
            this.Location = location;
            this.RequiredSkill = requiredSkill;
            this.Employee = employee;
        }


        public override string ToString()
        {
            return Location + " " + Start + "-" + End;
        }
    }
}