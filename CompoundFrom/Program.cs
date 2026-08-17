namespace CompoundFrom;

class CompoundFrom
{
    // The element type of the data source.
    public class Student
    {
        public required string LastName { get; init; }
        public required List<int> Scores {get; init;}
    }

    static void Main()
    {
        List<Student> students =
        [
           new Student {LastName="Omelchenko", Scores= [97, 72, 81, 60]},
           new Student {LastName="O'Donnell", Scores= [75, 84, 91, 39]},
           new Student {LastName="Mortensen", Scores= [88, 94, 65, 85]},
           new Student {LastName="Garcia", Scores= [97, 89, 85, 82]},
           new Student {LastName="Beebe", Scores= [35, 72, 91, 70]}
        ];

        // Use a compound from to access the inner sequence within each element.
        var scoreQuery = from student in students
                         from score in student.Scores
                            where score > 90
                            select new { Last = student.LastName, score };

        // Execute the queries.
        Console.WriteLine("scoreQuery:");
        foreach (var student in scoreQuery)
        {
            Console.WriteLine($"{student.Last} Score: {student.score}");
        }
    }
}

