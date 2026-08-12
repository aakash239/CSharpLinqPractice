namespace PracticeLINQ;

class Program
{
    // cities dataset 
    record City(string Name, long Population);
    record Country(string Name, double Area, long Population, List<City> Cities);
    record Product(string Name, string Category);
 
    static readonly City[] cities = [
        new City("Tokyo", 37_833_000),
        new City("Delhi", 30_290_000),
        new City("Shanghai", 27_110_000),
        new City("São Paulo", 22_043_000),
        new City("Mumbai", 20_412_000),
        new City("Beijing", 20_384_000),
        new City("Cairo", 18_772_000),
        new City("Dhaka", 17_598_000),
        new City("Osaka", 19_281_000),
        new City("New York-Newark", 18_604_000),
        new City("Karachi", 16_094_000),
        new City("Chongqing", 15_872_000),
        new City("Istanbul", 15_029_000),
        new City("Buenos Aires", 15_024_000),
        new City("Kolkata", 14_850_000),
        new City("Lagos", 14_368_000),
        new City("Kinshasa", 14_342_000),
        new City("Manila", 13_923_000),
        new City("Rio de Janeiro", 13_374_000),
        new City("Tianjin", 13_215_000)
    ];

    static readonly Country[] countries = [
        new Country ("Vatican City", 0.44, 526, [new City("Vatican City", 826)]),
        new Country ("Monaco", 2.02, 38_000, [new City("Monte Carlo", 38_000)]),
        new Country ("Nauru", 21, 10_900, [new City("Yaren", 1_100)]),
        new Country ("Tuvalu", 26, 11_600, [new City("Funafuti", 6_200)]),
        new Country ("San Marino", 61, 33_900, [new City("San Marino", 4_500)]),
        new Country ("Liechtenstein", 160, 38_000, [new City("Vaduz", 5_200)]),
        new Country ("Marshall Islands", 181, 58_000, [new City("Majuro", 28_000)]),
        new Country ("Saint Kitts & Nevis", 261, 53_000, [new City("Basseterre", 13_000)])
    ];
    
    // Employee Dataset
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
        // Question 1
        Console.WriteLine("Write a query over cities that produces just the names (not full City objects) of every city with population over 15,000,000");
        IEnumerable<string> query = from city in cities
                    where city.Population > 15_000_000
                    select city.Name;

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
        System.Console.WriteLine("\n\n\n");



        // Question 2
        System.Console.WriteLine("Exercise: Group cities by first letter");
        var groups = from city in cities
                    group city by city.Name[0] into g
                    select g;

        foreach (var item in groups)
        {
            System.Console.WriteLine($"{item.Key} : {item.Count()} cities");
        }
        System.Console.WriteLine("\n\n\n");



        // Question 3
        Console.WriteLine("Write a query that produces each employee's name alongside their department name (not just DepartmentId) — for every employee.");
        var joins1 = from employee in employees
                    join department in departments on employee.DepartmentId equals department.Id
                    select new
                    {
                      Name = employee.Name,
                      department = department.Name  
                    };

        foreach (var item in joins1)
        {
            System.Console.WriteLine($"{item.Name}, {item.department}");
        }
        System.Console.WriteLine("\n\n\n");
        


        // Question 4
        System.Console.WriteLine("Write a query that produces, for every assignment row: employee name, project name, and hours per week. This requires joining assignments to employees (via EmployeeId) and to projects (via ProjectId) — two joins in the same query.");
        var joins2 = from employee in employees
                    join assignment in assignments on employee.Id equals assignment.EmployeeId
                    join project in projects on assignment.ProjectId equals project.Id
                    select new
                    {
                        name = employee.Name,
                        project = project.Name,
                        workingHours = assignment.HoursPerWeek
                    };

        foreach (var item in joins2)
        {
            System.Console.WriteLine(item);
        }
        System.Console.WriteLine("\n\n\n");

        // Question 4 b: 
        System.Console.WriteLine("Group the projects together");
        var employeeProjects = 
            from employee in employees
            join assignment in assignments on employee.Id equals assignment.EmployeeId
            join project in projects on assignment.ProjectId equals project.Id
            group new {project.Name, assignment.HoursPerWeek} by employee.Name into x
            select new
            {
                EmployeeName = x.Key,
                Projects = x.ToList()
            };

        foreach (var emp in employeeProjects)
        {
            Console.WriteLine($"{emp.EmployeeName}:");
            foreach (var p in emp.Projects)
            {
                Console.WriteLine($"  - {p.Name} ({p.HoursPerWeek} hrs/week)");
            }
        }
        System.Console.WriteLine("\n\n\n");
        
        
        // Question 4 c - 
        var employeeProjects2 = from employee in employees
                    join assignment in assignments on employee.Id equals assignment.EmployeeId into empAssignments
                    from ea in empAssignments.DefaultIfEmpty()
                    join project in projects on ea?.ProjectId equals project.Id into projMatch
                    from pm in projMatch.DefaultIfEmpty()
                    select new
                    {
                        employee.Name,
                        Project = pm?.Name ?? "No Project",
                        hours = ea?.HoursPerWeek ?? 0
                    };
                    

        foreach (var emp in employeeProjects2)
        {
            Console.WriteLine(emp);
        }
        System.Console.WriteLine("\n\n\n");

        // Question 5
        // System.Console.WriteLine("Get all employees with Salary > 80000, ordered by salary descending. Print name and salary.");

        // Get all employees with Salary > 80000, ordered by salary descending. Print name and salary.
        var query5 = from employee in employees
        where employee.Salary > 80_000
        orderby employee.Salary descending
        select new {employee.Name, employee.Salary}; 

        // Get all employees ordered by DepartmentId ascending, and within each department, by Salary descending.

        var query6 = from e in employees
            orderby e.DepartmentId, e.Salary descending
            select e;

        // Find the total salary cost per department (department name + sum of salaries of employees in it). (Hint: group by + Sum().)

        var query7 = from e in employees
            join d in departments on e.DepartmentId equals d.Id
            group e by d.Name into g
            select new
            {
                DepartmentName = g.Key,
                TotalSalary = g.Sum(e => e.Salary)
            };

        // Find only the departments where the average employee salary exceeds 80,000. Print department name and average salary.

        var query8 = from e in employees
            join d in departments on e.DepartmentId equals d.Id
            group e by d.Name into g
            where g.Average(e => e.Salary) > 80_000
            select new
            {
                DepartmentName = g.Key,
                AverageSalary = g.Average(e => e.Salary) 
            };
        // For each project, calculate the total hours per week committed across all assigned employees (project name + total hours).
        var query9 = from p in projects
            join a in assignments on p.Id equals a.ProjectId 
            group a by p.Name into g
            select new
            {
                projectName = g.Key,
                workTimeTotal = g.Sum(g => g.HoursPerWeek)
            };

        // For each department, find the single highest-paid employee (name + salary). (Hint: think about grouping, then picking the max within each group — there's more than one valid approach here.)

        var query10 = from e in employees
            join d in departments on e.DepartmentId equals d.Id
            group new {e.Name, e.Salary} by d.Name into g
            select new
            {
                DepartmentName = g.Key,
                TopEmployee = g.OrderByDescending(x => x.Salary).First()
            };


        // List all employees who are not leading any project (i.e., their Id doesn't appear as any Project.LeadEmployeeId)

        
        System.Console.WriteLine("\n\n\n");
        foreach (var item in query10)
        {
            System.Console.WriteLine(item);
        }

    }
}
