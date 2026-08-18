using System.Runtime.ConstrainedExecution;

namespace JoinOperations;
// Book Dataset
record Author(int Id, string Name, string Country);
record Book(int Id, string Title, int AuthorId, int Year, string Genre);
record Review(int BookId, string Reviewer, int Rating); // 1-5
record Award(string Country, int Year, string AwardName);

// Employee Dataset
record Department(int Id, string Name, string Location);
record Employee(int Id, string Name, int DepartmentId, decimal Salary, int YearsOfService, string Title);
record Project(int Id, string Name, int LeadEmployeeId, int BudgetThousands, DateTime Deadline);
record Assignment(int EmployeeId, int ProjectId, int HoursPerWeek);

// main class
class Program
{
    // book dataset populated data
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
    
    // employee populated data 
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
        // simple join in method Syntax
        var plainJoins = books.Join(
            authors,
            book => book.AuthorId,
            author => author.Id,
            (book, author) =>new {book.Title, AuthorName = author.Name}
        );
        printQuery(plainJoins, "Plain inner join: book title + author name");
        
        
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
        printQuery(leftjoinResult, "Left join: books with author (Unknown if unmatched)");

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
        printQuery(authorBooks, "Left join: authors with book (No Books Written if none)");

        // self Joins
        var bookPairs = books.Join(
            books,
            outerBook => outerBook.AuthorId,
            InnerBook => InnerBook.AuthorId,
            (outerBook, InnerBook) => new{outerBook, InnerBook}
        ).Where(x => x.outerBook.Id < x.InnerBook.Id).Select(x => new {Author = x.InnerBook.AuthorId, Book1 = x.outerBook.Title, Book2 = x.InnerBook.Title});
        printQuery(bookPairs, "Self-join: pairs of books by the same author");

    
        var compositeJoins = books.Join(authors, book => book.AuthorId, author => author.Id, (x, y)=> new{x.Title, y.Country, x.Year}).Join(
            awards,
            book => new {book.Country, book.Year},
            award => new {award.Country, award.Year},
            (book, award) => new {book.Title, award.AwardName}
        );
        printQuery(compositeJoins, "Composite key join: book/author matched to award by country+year");


        // Questions for testing Method syntax Proficiency

        // List every book title along with its average review rating. Books with no reviews should still appear, showing 0 (or some clear placeholder) instead of crashing or being omitted.
        var query1 = books.GroupJoin(
            reviews,
            book => book.Id,
            review => review.BookId,
            (book, bookReview) => new {
                book.Title, 
                Rating = bookReview.Any() ? bookReview.Average(r => r.Rating) : 0}
        );
        printQuery(query1, "Q1: book titles with average rating");
    
        // 2. Find all countries that have written authors but no books published before 1990 by any of their authors. (Think about what operator lets you check "no book in this set satisfies X.")
        var query2 = authors.GroupJoin(
            books,
            author => author.Id,
            book => book.AuthorId,
            (author, booksWritten) => new {
                author.Country, Before1990 = booksWritten.Any(x => x.Year < 1990)  
            }
        ).GroupBy(x => x.Country)
        .Where(x => x.All(a => a.Before1990 == false))
        .Select(x => x.Key);
        printQuery(query2, "Q2: countries with no pre-1990 books");


        // 3. Using Union, Intersect, or Except (pick correctly): find the set of genres that exist in books but do NOT appear in any book with a review rating of 5. (i.e., genres that never got a top rating.)
        var query3 = books
            .GroupJoin(
            reviews,
            book => book.Id,
            review => review.BookId,
            (book, reviewGroup) => new {
                book.Genre, 
                StarRating5 = reviewGroup.Any(x => x.Rating == 5)
            })
            .GroupBy(x => x.Genre)
            .Where(x => x.All(a => a.StarRating5 == false))
            .Select(x => x.Key);
        printQuery(query3, "Q3 (GroupBy version): genres never rated 5 stars");

        var genresWithFiveStars = books.Join(
            reviews, 
            b => b.Id,
            r => r.BookId, 
            (b,r) => new {b.Genre, r.Rating})
            .Where(x => x.Rating == 5)
            .Select(x => x.Genre)
            .Distinct();
        var query3b = books.Select(b => b.Genre).Distinct().Except(genresWithFiveStars);
        printQuery(query3b, "Q3: genres never rated 5 stars");

        // 4. For each department, list the names of employees ordered by YearsOfService descending, but only include the top 2 per department. (Hint: needs grouping + a partitioning operator inside each group.)

        // query syntax
        var query4a = 
            from employee in employees
            join department in departments on employee.DepartmentId equals department.Id 
            group employee by department.Name into deptGroup
            select new
            {
                DepartmentName = deptGroup.Key,
                TopEmployees = deptGroup
                .OrderByDescending(e => e.YearsOfService)
                .Take(2)
                .Select(e => e.Name)
                .ToList()
            };
       
        PrintGroupedQuery(
            query4a, 
            "Q4a: tp 2 per department(query syntax)",
            x => $"Department: {x.DepartmentName}",
            x => x.TopEmployees
        );

        // method syntax
        var query4b = employees.Join(
            departments,
            employee => employee.DepartmentId,
            department => department.Id,
            (employee, department) => new
            {
                employee.Name, dept = department.Name, employee.YearsOfService
            }
        )
        .GroupBy(x => x.dept)
        .Select(g => new
        {
            DepartmentName = g.Key,
            TopEmployees = g
                .OrderByDescending(x => x.YearsOfService)
                .Take(2)
                .Select(x => x.Name)
                .ToList()
        });

        PrintGroupedQuery(
            query4a, 
            "Q4b: top 2 per department (method syntax)",
            x => $"Department: {x.DepartmentName}",
            x => x.TopEmployees
        );

        // printGrouped write afterwards

        // 5. Find the single book with the highest average review rating company-wide (whole object, not just the number). Handle the case of no reviews sensibly.
        var query5 = books.GroupJoin(
            reviews,
            book => book.Id,
            review => review.BookId,
            (book, reviewGroup) => new {book, AvgRating = reviewGroup.Any() ? reviewGroup.Average(r => r.Rating) : 0}
        ).OrderByDescending(b => b.AvgRating)
        .FirstOrDefault();

        Console.WriteLine($"\n\nQ5: highest-rated book overall\n{query5}");

        // 6. Using SelectMany, produce a flat list of every (ReviewerName, BookTitle) pair across all reviews — i.e., flatten reviews out with their book's title attached, without using a join at all (there's a way to do this with SelectMany referencing a lookup instead).
        var query6 = books.GroupJoin(
            reviews,
            book => book.Id,
            review => review.BookId,
            (book, reviewGroup) => new {reviewsGiven = reviewGroup, book.Title}
        ).SelectMany(
            x => x.reviewsGiven.DefaultIfEmpty(),
            (x, raters) => new {x.Title, bookReviewer = raters == null ? "": raters.Reviewer}
        );

        printQuery(query6, "Q6: reviewer/book pairs via SelectMany");

        // 7. Find all pairs of authors from the same country (self-join on authors), excluding pairing an author with themselves and excluding duplicate mirror pairs.

        var query7 = authors.Join(
            authors,
            author1 => author1.Country,
            author2 => author2.Country,
            (author1, author2) => new {
                author2,
                author1
            }           
        ).Where(x => x.author1.Id > x.author2.Id)
        .Select(x => new {
            author1 = x.author1.Name,
            author2 = x.author2.Name,
            x.author1.Country
        });

        printQuery(query7, "Q7: all pair of authors from same country");

        // For employees, find how many distinct Title values exist company-wide, and print each distinct title once.
        var query8 = (from employee in employees
            select employee.Title).Distinct();
        printQuery(query8, "Q8: distinct employee titles");
        
    }

    static void printQuery<T>(IEnumerable<T> query, string message)
    {
        Console.WriteLine($"\n\n\n {message}");
        foreach (T item in query)
        {
            Console.WriteLine(item);
        }
    }
    static void PrintGroupedQuery<TSource, TKey, TItem>(
        IEnumerable<TSource> query,
        string message,
        Func<TSource, TKey> keySelector,
        Func<TSource, IEnumerable<TItem>> itemsSelector)
    {
        Console.WriteLine($"\n\n\n{message}");
        foreach (var entry in query)
        {
            Console.WriteLine(keySelector(entry));
            foreach (var item in itemsSelector(entry))
            {
                Console.WriteLine($"   {item}");
            }
        }
    }

}
