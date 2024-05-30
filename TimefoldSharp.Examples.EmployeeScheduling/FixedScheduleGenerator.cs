using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Examples.EmployeeScheduling.EmployeeScheduling.Domain;

namespace TimefoldSharp.Examples.EmployeeScheduling
{
    static internal class FixedScheduleGenerator
    {
        static DateTime START_DATE = GetNextOrSameMonday();
        static int INITIAL_ROSTER_LENGTH_IN_DAYS = 14;
        static string[] LOCATIONS = { "Ambulatory care", "Critical care", "Pediatric care" };
        static string[] REQUIRED_SKILLS = { "Doctor", "Nurse" };
        static string[] OPTIONAL_SKILLS = { "Anaesthetics", "Cardiology" };
        static TimeSpan MORNING_SHIFT_START_TIME = TimeSpan.FromHours(6);
        static TimeSpan DAY_SHIFT_START_TIME = TimeSpan.FromHours(9);
        static TimeSpan AFTERNOON_SHIFT_START_TIME = TimeSpan.FromHours(14);
        static TimeSpan SHIFT_LENGTH = TimeSpan.FromHours(8);
        static TimeSpan NIGHT_SHIFT_START_TIME = TimeSpan.FromHours(22);
        static TimeSpan[][] SHIFT_START_TIMES_COMBOS = [
          [ MORNING_SHIFT_START_TIME, AFTERNOON_SHIFT_START_TIME],
            [ MORNING_SHIFT_START_TIME, AFTERNOON_SHIFT_START_TIME, NIGHT_SHIFT_START_TIME],
            [ MORNING_SHIFT_START_TIME, DAY_SHIFT_START_TIME, AFTERNOON_SHIFT_START_TIME, NIGHT_SHIFT_START_TIME]];
        static string[] FIRST_NAMES = { "Amy", "Beth", "Chad", "Dan", "Elsa", "Flo", "Gus", "Hugo", "Ivy", "Jay" };
        static string[] LAST_NAMES = { "Cole", "Fox", "Green", "Jones", "King", "Li", "Poe", "Rye", "Smith", "Watt" };
        static Dictionary<string, List<TimeSpan>> locationToShiftStartTimeListMap = new Dictionary<string, List<TimeSpan>>();

        public static EmployeeSchedule GenerateFixedSchedule()
        {
            var schedule = new EmployeeSchedule();
            GenerateScheduleState(schedule);
            GenerateScheduleEmployeeListFixed(schedule);
            GenerateScheduleAvailablityAndShiftListFixed(schedule);

            return schedule;
        }

        private static void GenerateScheduleState(EmployeeSchedule schedule)
        {
            ScheduleState scheduleState = new ScheduleState();
            scheduleState.FirstDraftDate = START_DATE;
            scheduleState.DraftLength = INITIAL_ROSTER_LENGTH_IN_DAYS;
            scheduleState.PublishLength = 7;
            scheduleState.LastHistoricDate = START_DATE.AddDays(-7);
            scheduleState.TenantId = 1;

            schedule.ScheduleState = scheduleState;
        }

        private static DateTime GetNextOrSameMonday()
        {
            DateTime today = DateTime.Today;
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
            return today.AddDays(daysUntilMonday);
        }

        private static void GenerateScheduleEmployeeListFixed(EmployeeSchedule schedule)
        {
            int shiftTemplateIndex = 0;
            foreach (var location in LOCATIONS)
            {
                locationToShiftStartTimeListMap.Add(location, new List<TimeSpan>(SHIFT_START_TIMES_COMBOS[shiftTemplateIndex]));
                shiftTemplateIndex = (shiftTemplateIndex + 1) % SHIFT_START_TIMES_COMBOS.Length;
            }
            List<string> namePermutations = JoinAllCombinations(FIRST_NAMES, LAST_NAMES);
            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(new Employee(namePermutations[0], new HashSet<string>() { "Anaesthetics", "Nurse" }));
            employeeList.Add(new Employee(namePermutations[1], new HashSet<string>() { "Cardiology", "Nurse" }));
            employeeList.Add(new Employee(namePermutations[2], new HashSet<string>() { "Cardiology", "Anaesthetics", "Nurse" }));
            employeeList.Add(new Employee(namePermutations[3], new HashSet<string>() { "Cardiology", "Doctor" }));
            employeeList.Add(new Employee(namePermutations[4], new HashSet<string>() { "Cardiology", "Nurse" }));
            employeeList.Add(new Employee(namePermutations[5], new HashSet<string>() { "Anaesthetics", "Nurse" }));
            employeeList.Add(new Employee(namePermutations[6], new HashSet<string>() { "Cardiology", "Nurse" }));
            employeeList.Add(new Employee(namePermutations[7], new HashSet<string>() { "Anaesthetics", "Cardiology", "Doctor" }));
            employeeList.Add(new Employee(namePermutations[8], new HashSet<string>() { "Anaesthetics", "Nurse" }));
            employeeList.Add(new Employee(namePermutations[9], new HashSet<string>() { "Anaesthetics", "Doctor" }));
            employeeList.Add(new Employee(namePermutations[10], new HashSet<string>() { "Cardiology", "Doctor" }));
            employeeList.Add(new Employee(namePermutations[11], new HashSet<string>() { "Anaesthetics", "Cardiology", "Nurse" }));
            employeeList.Add(new Employee(namePermutations[12], new HashSet<string>() { "Cardiology", "Doctor" }));
            employeeList.Add(new Employee(namePermutations[13], new HashSet<string>() { "Anaesthetics", "Cardiology", "Doctor" }));
            employeeList.Add(new Employee(namePermutations[14], new HashSet<string>() { "Cardiology", "Nurse" }));

            schedule.EmployeeList = employeeList;
        }

        static private List<string> JoinAllCombinations(params string[][] partArrays)
        {
            int size = 1;
            foreach (var partArray in partArrays)
            {
                size *= partArray.Length;
            }
            List<string> outs = new List<string>(size);
            for (int i = 0; i < size; i++)
            {
                StringBuilder item = new StringBuilder();
                int sizePerIncrement = 1;
                foreach (var partArray in partArrays)
                {
                    item.Append(' ');
                    item.Append(partArray[(i / sizePerIncrement) % partArray.Length]);
                    sizePerIncrement *= partArray.Length;
                }
                item.Remove(0, 1);
                outs.Add(item.ToString());
            }
            return outs;
        }

        private static void GenerateScheduleAvailablityAndShiftListFixed(EmployeeSchedule schedule)
        {
            long id = 0;
            List<Availability> availablities = new List<Availability>();
            List<Shift> shifts = new List<Shift>();

            for (int i = 0; i < INITIAL_ROSTER_LENGTH_IN_DAYS; i++)
            {

                HashSet<Employee> employeesWithAvailabitiesOnDay = PickSubsetFixed(schedule.EmployeeList, 4, 3, 2, 1);
                DateTime date = START_DATE.AddDays(i);
                foreach (var employee in employeesWithAvailabitiesOnDay)
                {
                    AvailabilityType availabilityType = PickRandomFixed(Enum.GetValues(typeof(AvailabilityType)).Cast<AvailabilityType>().ToList());
                    availablities.Add(new Availability(employee, date, availabilityType, id++));
                }
                GenerateShiftsForDayFixed(date, shifts);
            }

            schedule.AvailabilityList = availablities;
            schedule.ShiftList = shifts;
        }

        static private HashSet<T> PickSubsetFixed<T>(List<T> sourceSet, params int[] distribution)
        {
            int probabilitySum = 0;
            foreach (var probability in distribution)
            {
                probabilitySum += probability;
            }
            int choice = GetFixedNumber(probabilitySum);
            int numOfItems = 0;
            while (choice >= distribution[numOfItems])
            {
                choice -= distribution[numOfItems];
                numOfItems++;
            }
            List<T> items = new List<T>(sourceSet);
            //items.Shuffle(random);
            return new HashSet<T>(items.GetRange(0, numOfItems + 1));
        }

        static private void GenerateShiftsForDayFixed(DateTime date, List<Shift> shifts)
        {
            foreach (var location in LOCATIONS)
            {
                List<TimeSpan> shiftStartTimeList = locationToShiftStartTimeListMap[location];
                foreach (var shiftStartTime in shiftStartTimeList)
                {
                    DateTime shiftStartDateTime = date.Add(shiftStartTime);
                    DateTime shiftEndDateTime = shiftStartDateTime.Add(SHIFT_LENGTH);
                    GenerateShiftForTimeslotFixed(shiftStartDateTime, shiftEndDateTime, location, shifts);
                }
            }
        }

        static private void GenerateShiftForTimeslotFixed(DateTime timeslotStart, DateTime timeslotEnd, string location, List<Shift> shifts)
        {
            int shiftCount = 1;

            if (GetFixedNumberD() > 0.9)
            {
                // generate an extra shift
                shiftCount++;
            }

            for (int i = 0; i < shiftCount; i++)
            {
                string requiredSkill;
                if (GetFixedNumberD() >= 0.5f)
                {
                    requiredSkill = PickRandomFixed(REQUIRED_SKILLS);
                }
                else
                {
                    requiredSkill = PickRandomFixed(OPTIONAL_SKILLS);
                }
                shifts.Add(new Shift(shiftID++, timeslotStart, timeslotEnd, location, requiredSkill));
            }
        }

        static long shiftID = 0;
        static double counterD = 0.0;
        static double GetFixedNumberD()
        {
            if (counterD >= 1.0)
                counterD = 0;
            counterD += 0.1f;
            return counterD;
        }

        static int counter = 1;
        static int GetFixedNumber(int max)
        {
            counter += 2;
            if (counter >= max)
                counter = 1;
            return counter;
        }

        static private T PickRandomFixed<T>(List<T> source)
        {
            return source[GetFixedNumber(source.Count)];
        }

        static private T PickRandomFixed<T>(T[] source)
        {
            return source[GetFixedNumber(source.Length)];
        }

    }
}
