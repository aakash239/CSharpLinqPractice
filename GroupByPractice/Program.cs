namespace GroupByPractice;

class Program
{
    public class Student
    {
        public required string First { get; init; }
        public required string Last { get; init; }
        public required int ID { get; init; }
        public required List<int> Scores;
    }

    public static List<Student> GetStudents()
    {
        List<Student> students =
        [
           new Student {First="Svetlana", Last="Omelchenko", ID=111, Scores= [97, 72, 81, 60]},
           new Student {First="Claire", Last="O'Donnell", ID=112, Scores= [75, 84, 91, 39]},
           new Student {First="Sven", Last="Mortensen", ID=113, Scores= [99, 89, 91, 95]},
           new Student {First="Sven", Last="Garcia", ID=114, Scores= [72, 81, 65, 84]},
           new Student {First="Debra", Last="Garcia", ID=115, Scores= [97, 89, 85, 82]}
        ];

        return students;
    }

    static void Main(string[] args)
    {
        List<Student> students = GetStudents();

        // groupBy true or false
        IEnumerable<IGrouping<bool, Student>> booleanGroupQuery = 
        from student in students
            group student by student.Scores.Average() >= 80;
        
        PrintGroups(booleanGroupQuery, "Boolean grouping:", key => key ? "High averages" : "Low averages");

        // Grouping by numeric range
        var numericGroupQuery = 
            from student in students
            group student by ((int)student.Scores.Average())/10 into g
            orderby g.Key
            select g;

        PrintGroups(numericGroupQuery, "Numeric range grouping:", key => $"Students with an average between {key * 10} and {key * 10 + 10}");

        // grouping by wrt a composite key
        var compositeGroupQuery = 
            from student in students
            group student by new {student.First};

        PrintGroups(compositeGroupQuery, "Composite Query groups by student's First Name: ", key => $"Key: {key}");
        
    }

    static void PrintGroups<TKey>(IEnumerable<IGrouping<TKey,Student>>query, string message, Func<TKey, string> describeKey)
    {
        Console.WriteLine($"\n\n\n{message}");
        foreach (var item in query)
        {
            Console.WriteLine(describeKey(item.Key));
            foreach (var student in item)
            {
                Console.WriteLine($" {student.First} {student.Last}: {student.Scores.Average()}");
            }
        }
    }
}
