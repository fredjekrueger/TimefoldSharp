using TimefoldSharp.Core.API.Domain.Entity;
using TimefoldSharp.Core.API.Score;

namespace TimefoldSharp.Examples.EmployeeScheduling.EmployeeScheduling.Domain
{
    internal class ShiftPinningFilter : IPinningFilter
    {
        public bool Accept(ISolution solution, object shift)
        {
            EmployeeSchedule employeeSchedule = solution as EmployeeSchedule;

            ScheduleState scheduleState = employeeSchedule.ScheduleState;
            return !scheduleState.IsDraft((Shift)shift);
        }
    }
}
