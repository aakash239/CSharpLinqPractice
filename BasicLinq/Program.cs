namespace BasicLinq;

class Program
{
    static void Main(string[] args)
    {
        int[] scores = [97, 92, 81, 60];

        // this is the query expression defined
        IEnumerable<int> scoreQuery = 
            from score in scores
            where score > 80
            select score; // query syntax

        // Method syntax: regular method calls chained together.
        IEnumerable<int> scoreQueryMethodSyntax = scores.Where(score => score > 80);

        foreach (var i in scoreQuery) // query in query syntax is executed here
        {
            Console.Write(i + " ");
        }

        foreach (var i in scoreQueryMethodSyntax) // query in method syntax is executed here
        {
            Console.Write(i + " ");
        }

        // compiler converts(rewrites) the query syntax to method syntax before compiling further

        /* 
        some methods have no query-syntax equivalent and exist only as methods
        - switch up entirely to method syntax
        - mix both write the filtering part in query syntax and wrap the whloe thing in parenthesis and call .count() on the result
        */


        List<Fruit> fruits = [
            new Fruit("apple", 1),
            new Fruit("mango", 3),
            new Fruit("banana", 2)];
        
        var query = fruits.Select(x => x).OrderBy(x => x.name).Reverse();


        foreach (var item in query)
        {
            System.Console.WriteLine(item.name);
        }
    }

}

class Fruit(string name, int id)
{
    public string name {get; set;}= name ;
    public int id {get; set;}= id;
}
