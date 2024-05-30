using Serilog;
using System.Text;
using TimefoldSharp.Core.API.Solver;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Examples.EmployeeScheduling;
using TimefoldSharp.Examples.EmployeeScheduling.EmployeeScheduling.Domain;
using TimefoldSharp.Examples.EmployeeScheduling.EmployeeScheduling.Solver;

internal class Program
{
    static void Main(string[] args)
    {
        CreateLogger();

        var config = new SolverConfig()
       .WithSolutionClass(typeof(EmployeeSchedule))
       .WithEntityClasses(typeof(Shift))
       .WithConstraintProviderClass(typeof(EmployeeSchedulingConstraintProvider))
       .WithTerminationSpentLimit(TimeSpan.FromSeconds(5));

        SolverFactory solverFactory = SolverFactory.Create(config);
        EmployeeSchedule problem = GenerateDemoData();

        Solver solver = solverFactory.BuildSolver();
        EmployeeSchedule solution = (EmployeeSchedule)solver.Solve(problem);

        PrintSchedule(solution);
        Log.Information(solver.GetBestScore().ToString());
        Console.ReadLine();
    }

    static bool generateFixedSchedule = false;
    private static EmployeeSchedule GenerateDemoData()
    {
        if (generateFixedSchedule)
        {
            return FixedScheduleGenerator.GenerateFixedSchedule();
        }
        else
        {
            return RandomScheduleGenerator.GenerateSchedule();
        }
    }

    private static void PrintSchedule(EmployeeSchedule solution)
    {
        Console.WriteLine("Availablities: " + string.Join(Environment.NewLine, solution.AvailabilityList));
        Console.WriteLine("Shifts: " + string.Join(Environment.NewLine, solution.ShiftList));
    }

    private static void CreateLogger()
    {
        Log.Logger = new LoggerConfiguration()
                            // add console as logging target
                            .WriteTo.Console()
                            // set default minimum level
                            .MinimumLevel.Debug()
                            .CreateLogger();
    }
}
