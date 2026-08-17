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

        // --------------------------------------------
        // --------------------------------------------
        // --------------------------------------------

        char[] upperCase = ['A', 'B', 'C'];
        char[] lowerCase = ['x', 'y', 'z'];

        IEnumerable<CharPair> joinQuery1 = 
            from upper in upperCase
            from lower in lowerCase
            select new CharPair(upper, lower);

        IEnumerable<CharPair> joinQuery2 = 
            from lower in lowerCase
            where lower != 'x'
            from upper in upperCase
            select new CharPair(upper, lower);

        Console.WriteLine("Cross join:");
        // Rest the mouse pointer on joinQuery1 to verify its type.
        foreach (var pair in joinQuery1)
        {
            Console.WriteLine($"{pair.UpperCase} is matched to {pair.LowerCase}");
        }

        Console.WriteLine("Filtered non-equijoin:");
        // Rest the mouse pointer over joinQuery2 to verify its type.
        foreach (var pair in joinQuery2)
        {
            Console.WriteLine($"{pair.LowerCase} is matched to {pair.UpperCase}");
        }
        
    }
    class CharPair(char upperCase, char lowerCase)
    {
        public char UpperCase {get; set;} = upperCase;
        public char LowerCase {get; set;} = lowerCase;
    }
}

