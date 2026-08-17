namespace LINQAndCollections;

class Program
{
    readonly static string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\files\"));

    public record Student(string FirstName, string LastName, int[] ExamScores, int ID = 0);
    static void Main(string[] args)
    {
        string[] fileA = File.ReadAllLines(Path.Combine(projectDir, "names1.txt"));    

        string[] fileB = File.ReadAllLines(Path.Combine(projectDir, "names2.txt"));

        // to compare two lists of strings and output those lines that are in first collection, but not in the second.
        var differenceQuery = fileA.Except(fileB);
        OutputQueryResultsWhenList(differenceQuery, "The following lines are in names1.txt but not in names2.txt");

        // Concatenate and remove duplicate names based on default string comparer.
        var uniqueNamesQuery = fileA.Union(fileB).OrderBy(s => s);
        OutputQueryResultsWhenList(uniqueNamesQuery, "Simple concatenate and sort. Duplicates are preserved:");

        
        // Find the names that occur in both files (based on default string comparer).
        var commonNamesQuery = fileA.Intersect(fileB);
        OutputQueryResultsWhenList(commonNamesQuery, "Merge based on intersect:");

        // Find the matching fields in each list. Merge the two
        // results by using Concat, and then
        // sort using the default string comparer.
        string nameMatch = "Garcia";

        var tempQuery1 = from name in fileA
                        let n = name.Split(',')
                        where n[0] == nameMatch
                        select name;
                        
        var tempQuery2 = from name2 in fileA
                        let n = name2.Split(',')
                        where n[0] == nameMatch
                        select name2;

        var nameMatchQuery = tempQuery1.Concat(tempQuery2).OrderBy(s => s);
        OutputQueryResultsWhenList(nameMatchQuery, $"Concat based on partial name match:  {nameMatch}");


        // Practice handling csv files
        string[] names = File.ReadAllLines(Path.Combine(projectDir, "names.csv"));
        string[] scores = File.ReadAllLines(Path.Combine(projectDir, "scores.csv"));

        // Merge the data sources using a named type.
        IEnumerable<Student> queryNamesScores = 
            from nameLine in names
            let splitName = nameLine.Split(',')
            from scoreLine in scores
            let splitScoreLine = scoreLine.Split(',')
            where Convert.ToInt32(splitName[2]) == Convert.ToInt32(splitScoreLine[0])
            select new Student
            (
                FirstName: splitName[0],
                LastName: splitName[1],
                ID: Convert.ToInt32(splitName[2]),
                ExamScores: (from scoreAsText in splitScoreLine.Skip(1)
                select Convert.ToInt32(scoreAsText)).ToArray()
            );
        OutputQueryResultsWhenList(queryNamesScores, "Display each student's name and exam score average.");

                // A simple data source.
        int[] numbers = [5, 4, 1, 3, 9, 8, 6, 7, 2, 0];

        // Create the query.
        // lowNums is an IEnumerable<int>
        var lowNums = from num in numbers
            where num < 5
            select num + 10;

        // Execute the query.
        foreach (int i in lowNums)
        {
            Console.Write(i + " ");
        }
        // How to query an ArrayList with LINQ
        // var query = from Student s in arrList ...
        // typecast arraylist object to Student for dynamic runtime
    }

    static void OutputQueryResultsWhenList<T>(IEnumerable<T> query, string message)
    {
        Console.WriteLine("\n" + message);
        foreach (T item in query)
        {
            Console.WriteLine(item);
        }
    }

    static void OutputQueryResults<T>(T queryOutput, string message)
    {
        Console.WriteLine("\n" + message);
        Console.WriteLine(queryOutput);
        
    }
}
