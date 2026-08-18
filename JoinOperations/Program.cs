using System.Runtime.ConstrainedExecution;

namespace JoinOperations;

record Author(int Id, string Name, string Country);
record Book(int Id, string Title, int AuthorId, int Year, string Genre);
record Review(int BookId, string Reviewer, int Rating); // 1-5
record Award(string Country, int Year, string AwardName);
class Program
{
    static readonly Author[] authors = [
        new Author(1, "Haruki Murakami", "Japan"),
        new Author(2, "Chimamanda Ngozi Adichie", "Nigeria"),
        new Author(3, "Kazuo Ishiguro", "UK"),
        new Author(4, "Isabel Allende", "Chile"),
        new Author(5, "Yukio Mishima", "Japan")   // no books in our dataset
    ];
    static readonly Book[] books = [
        new Book(101, "Norwegian Wood", 1, 1987, "Fiction"),
        new Book(102, "Kafka on the Shore", 1, 2002, "Fiction"),
        new Book(103, "Half of a Yellow Sun", 2, 2006, "Historical Fiction"),
        new Book(104, "Americanah", 2, 2013, "Fiction"),
        new Book(105, "Never Let Me Go", 3, 2005, "Sci-Fi"),
        new Book(106, "The Remains of the Day", 3, 1989, "Fiction"),
        new Book(107, "The House of the Spirits", 4, 1982, "Magical Realism"),
        new Book(108, "Orphan Train", 99, 2013, "Fiction")   // AuthorId 99 doesn't exist!
    ];
    static readonly Review[] reviews = [
        new Review(101, "Alice", 5),
        new Review(101, "Bob", 4),
        new Review(102, "Carol", 5),
        new Review(103, "Dave", 4),
        new Review(103, "Eve", 3),
        new Review(105, "Frank", 5),
        new Review(106, "Grace", 4),
        new Review(107, "Heidi", 5)
        // Book 104 and 108 have no reviews at all
    ];
    static readonly Award[] awards = [
    new Award("Japan", 1987, "Tanizaki Prize"),
    new Award("Japan", 2002, "Yomiuri Prize"),
    new Award("UK", 1989, "Booker Prize"),
    new Award("Nigeria", 2013, "NLNG Prize"),
    new Award("Chile", 1982, "Best of the Decade")
];
    static void Main(string[] args)
    {
        // simple join in method Syntax
        var plainJoins = books.Join(
            authors,
            book => book.AuthorId,
            author => author.Id,
            (book, author) =>new {book.Title, AuthorName = author.Name}
        );
        /* outerSequence.Join(
            innerSequence,       // the second collection
            outerKeySelector,    // key from each item in outerSequence
            innerKeySelector,    // key from each item in innerSequence
            resultSelector        // how to combine a matched pair into one output item
        )*/
        PrintIEnumerableQuery(plainJoins);

        // left-join equivalent in method syntax
        var leftjoinResult = books.GroupJoin(
            authors,
            book => book.AuthorId,
            authors => authors.Id,
            (book, authorGroup) => new {book, authorGroup}
        ).SelectMany(
            x => x.authorGroup.DefaultIfEmpty(),
            (x, author) => new {x.book.Title, AuthorName = author == null ? "Unknown" : author.Name}
        );
        
        PrintIEnumerableQuery(leftjoinResult);

        // Authors with no books should appear
        var authorBooks = authors.GroupJoin(
            books,
            author => author.Id,
            book => book.AuthorId,
            (author, bookGroup) => new{author, bookGroup}
        ).SelectMany(
            x => x.bookGroup.DefaultIfEmpty(),
            (x, books) => new {Author = x.author.Name, BookName = books == null ? "No Books Written" : books.Title}
        );
        PrintIEnumerableQuery(authorBooks);

        // self Joins
        var bookPairs = books.Join(
            books,
            outerBook => outerBook.AuthorId,
            InnerBook => InnerBook.AuthorId,
            (outerBook, InnerBook) => new{outerBook, InnerBook}
        ).Where(x => x.outerBook.Id < x.InnerBook.Id).Select(x => new {Author = x.InnerBook.AuthorId, Book1 = x.outerBook.Title, Book2 = x.InnerBook.Title});
        PrintIEnumerableQuery(bookPairs);
    
        var compositeJoins = books.Join(authors, book => book.AuthorId, author => author.Id, (x, y)=> new{x.Title, y.Country, x.Year}).Join(
            awards,
            book => new {book.Country, book.Year},
            award => new {award.Country, award.Year},
            (book, award) => new {book.Title, award.AwardName}
        );
    }

    static void PrintIEnumerableQuery<T>(IEnumerable<T> query)
    {
        Console.WriteLine("\n\n\n");
        foreach (T item in query)
        {
            Console.WriteLine(item);
        }
    }
}
