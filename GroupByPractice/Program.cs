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
           new Student {First="Cesar", Last="Garcia", ID=114, Scores= [72, 81, 65, 84]},
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
        
        PrintBooleanQuery(booleanGroupQuery); // print the query

    }
    static void PrintBooleanQuery(IEnumerable<IGrouping<bool, Student>>  booleanGroupQuery)
    {
        foreach (var studentGroup in booleanGroupQuery)
        {
            Console.WriteLine(studentGroup.Key == true ? "High averages" : "Low averages");
            foreach (var student in studentGroup)
            {
                Console.WriteLine("   {0}, {1}:{2}", student.Last, student.First, student.Scores.Average());
            }
        }
    }
}
