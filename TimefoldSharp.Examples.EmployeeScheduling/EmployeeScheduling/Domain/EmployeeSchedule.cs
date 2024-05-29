using TimefoldSharp.Core.API.Domain.Solution;
using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Buildin.HardSoft;

namespace TimefoldSharp.Examples.EmployeeScheduling.EmployeeScheduling.Domain
{
    [PlanningSolution]
    internal class EmployeeSchedule : ISolution
    {
        [ProblemFactCollectionProperty]
        public List<Availability> AvailabilityList { get; set; }

        [ProblemFactCollectionProperty]
        [ValueRangeProvider]
        public List<Employee> EmployeeList { get; set; }

        [PlanningEntityCollectionProperty]
        public List<Shift> ShiftList { get; set; }

        [PlanningScore]
        public HardSoftScore Score { get; set; }

        public ScheduleState ScheduleState { get; set; }

        public EmployeeSchedule() { }

        public EmployeeSchedule(ScheduleState scheduleState, List<Availability> availabilityList, List<Employee> employeeList, List<Shift> shiftList)
        {
            this.ScheduleState = scheduleState;
            this.AvailabilityList = availabilityList;
            this.EmployeeList = employeeList;
            this.ShiftList = shiftList;
        }
    }
}
