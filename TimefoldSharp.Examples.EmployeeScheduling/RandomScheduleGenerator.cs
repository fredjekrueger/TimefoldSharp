using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Examples.EmployeeScheduling.EmployeeScheduling.Domain;

namespace TimefoldSharp.Examples.EmployeeScheduling
{
    static internal class RandomScheduleGenerator
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
        static Random random = new Random(0);
        static TimeSpan NIGHT_SHIFT_START_TIME = TimeSpan.FromHours(22);
        static TimeSpan[][] SHIFT_START_TIMES_COMBOS = [
          [ MORNING_SHIFT_START_TIME, AFTERNOON_SHIFT_START_TIME],
            [ MORNING_SHIFT_START_TIME, AFTERNOON_SHIFT_START_TIME, NIGHT_SHIFT_START_TIME],
            [ MORNING_SHIFT_START_TIME, DAY_SHIFT_START_TIME, AFTERNOON_SHIFT_START_TIME, NIGHT_SHIFT_START_TIME]];
        static string[] FIRST_NAMES = { "Amy", "Beth", "Chad", "Dan", "Elsa", "Flo", "Gus", "Hugo", "Ivy", "Jay" };
        static string[] LAST_NAMES = { "Cole", "Fox", "Green", "Jones", "King", "Li", "Poe", "Rye", "Smith", "Watt" };
        static Dictionary<string, List<TimeSpan>> locationToShiftStartTimeListMap = new Dictionary<string, List<TimeSpan>>();

        static long shiftID = 0;

        public static EmployeeSchedule GenerateSchedule()
        {
            var schedule = new EmployeeSchedule();
            GenerateScheduleState(schedule);
            GenerateScheduleEmployeeList(schedule);
            GenerateScheduleAvailablityAndShiftList(schedule);

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

        private static void GenerateScheduleAvailablityAndShiftList(EmployeeSchedule schedule)
        {
            long id = 0;
            List<Availability> availablities = new List<Availability>();
            List<Shift> shifts = new List<Shift>();
            for (int i = 0; i < INITIAL_ROSTER_LENGTH_IN_DAYS; i++)
            {
                HashSet<Employee> employeesWithAvailabitiesOnDay = PickSubset(schedule.EmployeeList, random, 4, 3, 2, 1);
                DateTime date = START_DATE.AddDays(i);
                foreach (var employee in employeesWithAvailabitiesOnDay)
                {
                    AvailabilityType availabilityType = PickRandom(Enum.GetValues(typeof(AvailabilityType)).Cast<AvailabilityType>().ToList(), random);
                    availablities.Add(new Availability(employee, date, availabilityType, id++));
                }

                GenerateShiftsForDay(date, random, shifts);
            }
            schedule.AvailabilityList = availablities;
            schedule.ShiftList = shifts;
        }

        static private void GenerateShiftsForDay(DateTime date, Random random, List<Shift> shifts)
        {
            foreach (var location in LOCATIONS)
            {
                List<TimeSpan> shiftStartTimeList = locationToShiftStartTimeListMap[location];
                foreach (var shiftStartTime in shiftStartTimeList)
                {
                    DateTime shiftStartDateTime = date.Add(shiftStartTime);
                    DateTime shiftEndDateTime = shiftStartDateTime.Add(SHIFT_LENGTH);
                    GenerateShiftForTimeslot(shiftStartDateTime, shiftEndDateTime, location, random, shifts);
                }
            }
        }

        static private void GenerateShiftForTimeslot(DateTime timeslotStart, DateTime timeslotEnd, string location, Random random, List<Shift> shifts)
        {
            int shiftCount = 1;

            if (random.NextDouble() > 0.9)
            {
                // generate an extra shift
                shiftCount++;
            }

            for (int i = 0; i < shiftCount; i++)
            {
                string requiredSkill;
                if (random.NextDouble() >= 0.5f)
                {
                    requiredSkill = PickRandom(REQUIRED_SKILLS, random);
                }
                else
                {
                    requiredSkill = PickRandom(OPTIONAL_SKILLS, random);
                }
                shifts.Add(new Shift(shiftID++, timeslotStart, timeslotEnd, location, requiredSkill));
            }
        }

        private static void GenerateScheduleEmployeeList(EmployeeSchedule schedule)
        {
            int shiftTemplateIndex = 0;
            foreach (var location in LOCATIONS)
            {
                locationToShiftStartTimeListMap.Add(location, new List<TimeSpan>(SHIFT_START_TIMES_COMBOS[shiftTemplateIndex]));
                shiftTemplateIndex = (shiftTemplateIndex + 1) % SHIFT_START_TIMES_COMBOS.Length;
            }

            List<string> namePermutations = JoinAllCombinations(FIRST_NAMES, LAST_NAMES);
            Random random = new Random(0);
            //namePermutations.Shuffle(random);

            List<Employee> employeeList = new List<Employee>();
            for (int i = 0; i < 15; i++)
            {
                HashSet<string> skills = PickSubset(new List<string>(OPTIONAL_SKILLS), random, 3, 1);
                skills.Add(PickRandom(REQUIRED_SKILLS, random));
                Employee employee = new Employee(namePermutations[i], skills);
                employeeList.Add(employee);
            }

            schedule.EmployeeList = employeeList;
        }

        static private HashSet<T> PickSubset<T>(List<T> sourceSet, Random random, params int[] distribution)
        {
            int probabilitySum = 0;
            foreach (var probability in distribution)
            {
                probabilitySum += probability;
            }
            int choice = random.Next(probabilitySum);
            int numOfItems = 0;
            while (choice >= distribution[numOfItems])
            {
                choice -= distribution[numOfItems];
                numOfItems++;
            }
            List<T> items = new List<T>(sourceSet);
            items.Shuffle(random);
            return new HashSet<T>(items.GetRange(0, numOfItems + 1));
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

        static private T PickRandom<T>(List<T> source, Random random)
        {
            return source[random.Next(source.Count)];
        }

        static private T PickRandom<T>(T[] source, Random random)
        {
            return source[random.Next(source.Length)];
        }

    }
}
