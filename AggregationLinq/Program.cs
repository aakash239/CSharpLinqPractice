namespace AggregationLinq;

class Program
{
    record Department(int Id, string Name, string Location);

    record Employee(int Id, string Name, int DepartmentId, decimal Salary, int YearsOfService, string Title);

    record Project(int Id, string Name, int LeadEmployeeId, int BudgetThousands, DateTime Deadline);

    record Assignment(int EmployeeId, int ProjectId, int HoursPerWeek);

    static readonly Department[] departments = [
        new Department(1, "Engineering", "Bangalore"),
        new Department(2, "Sales",       "Mumbai"),
        new Department(3, "Marketing",   "Delhi"),
        new Department(4, "HR",          "Bangalore"),
        new Department(5, "Finance",     "Mumbai")
    ];

    static readonly Employee[] employees = [
        new Employee(1, "Aarav Sharma",   1, 95000, 4, "Senior Engineer"),
        new Employee(2, "Priya Nair",     1, 78000, 2, "Engineer"),
        new Employee(3, "Rohan Gupta",    1, 120000, 7, "Staff Engineer"),
        new Employee(4, "Ishita Verma",   2, 65000, 3, "Sales Executive"),
        new Employee(5, "Kabir Singh",    2, 82000, 5, "Sales Manager"),
        new Employee(6, "Ananya Iyer",    3, 71000, 2, "Marketing Specialist"),
        new Employee(7, "Vikram Rao",     3, 88000, 6, "Marketing Lead"),
        new Employee(8, "Diya Patel",     4, 60000, 1, "HR Coordinator"),
        new Employee(9, "Arjun Mehta",    5, 105000, 8, "Finance Manager"),
        new Employee(10, "Sneha Reddy",   1, 99000, 3, "Senior Engineer"),
        new Employee(11, "Karan Malhotra",5, 73000, 2, "Financial Analyst"),
        new Employee(12, "Meera Joshi",   2, 91000, 4, "Sales Manager")
    ];

    static readonly Project[] projects = [
        new Project(101, "Phoenix",     3, 500, new DateTime(2026, 12, 1)),
        new Project(102, "Aurora",      1, 300, new DateTime(2026, 9, 15)),
        new Project(103, "Nebula",      9, 750, new DateTime(2027, 2, 1)),
        new Project(104, "Zenith",      5, 200, new DateTime(2026, 10, 30)),
        new Project(105, "Orion",       7, 150, new DateTime(2026, 11, 20))
    ];

    static readonly Assignment[] assignments = [
        new Assignment(1, 101, 20),
        new Assignment(1, 102, 15),
        new Assignment(2, 102, 30),
        new Assignment(3, 101, 25),
        new Assignment(3, 103, 10),
        new Assignment(4, 104, 40),
        new Assignment(5, 104, 20),
        new Assignment(9, 103, 35),
        new Assignment(10, 101, 15),
        new Assignment(10, 102, 15),
        new Assignment(7, 105, 30),
        new Assignment(12, 104, 20)
    ];
 
    static void Main(string[] args)
    {
        // Count how many employees have YearsOfService >= 5. (No where clause needed — Count itself can take a condition.)
        var query1 = (from e in employees
            select e).Count(e => e.YearsOfService >= 5);

        // Find the highest salary in the entire company, and separately, the lowest. (Just the number, not the employee.)
        var query2 = (from e in employees
            select e.Salary).Max();
        var query3 = (from e in employees
            select e.Salary).Min();

        // Check: does any employee have YearsOfService > 10? (Boolean result.)
        var query4 = (from e in employees
            select e.YearsOfService > 10).Any();

        // Check: are all projects budgeted at more than 100 (thousand)?
        var query5 = (from p in projects
            select p.BudgetThousands).All(p => p > 100);

        // Find the total combined salary of the entire company (no grouping — just one number).
        var query6 = (from e in employees
        select e.Salary).Sum();

        // Find the average YearsOfService across all employees.
        var query7 = (from e in employees
        select e.YearsOfService).Average();

        // Use Aggregate (not Sum) to manually compute the total salary of all employees — i.e., reduce the whole employees sequence down to a single running total, starting from 0.
        var query9 = (from e in employees select e.Salary).Aggregate(0.0M, (runningTotal,currentSalary) => runningTotal + currentSalary);

        // Get a distinct list of all Location values across departments (some locations repeat, like Bangalore and Mumbai).
        var query10 = (from d in departments
            select d.Location).Distinct();
        Console.WriteLine(query9);
        // foreach (var item in query1)
        // {
            
        // }
    }
}
