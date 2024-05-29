using TimefoldSharp.Core.API.Domain.Lookup;

namespace TimefoldSharp.Examples.EmployeeScheduling.EmployeeScheduling.Domain
{
    internal class Employee
    {
        [PlanningId]
        public string Name { get; set; }

        public HashSet<string> SkillSet { get; set; }

        public Employee()
        {

        }

        public Employee(string name, HashSet<string> skillSet)
        {
            this.Name = name;
            this.SkillSet = skillSet;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
