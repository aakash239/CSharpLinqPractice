namespace CitiesCountryData;

record City(string Name, long Population);
record Country(string Name, double Area, long Population, List<City> Cities);
record Product(string Name, string Category);


class Program
{
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
    static void Main(string[] args)
    {
        // // Query syntax
        // IEnumerable<City> queryMajorCities =
        //     from city in cities
        //     where city.Population > 30_000_000
        //     select city;

        // // Execute the query to produce the results
        // foreach (City city in queryMajorCities)
        // {
        //     Console.WriteLine(city);
        // }    

        // // Method-based syntax
        // IEnumerable<City> queryMajorCities2 = cities.Where(c => c.Population > 30_000_000);

        // foreach (City city in queryMajorCities2)
        // {
        //     Console.WriteLine(city);
        // }

        // int[] scores = [90, 71, 82, 93, 75, 82];

        // var highestScore = (
        //     from score in scores
        //     select score
        // ).Max();

        // // Or split the expression
        // IEnumerable<int> scoreQuery = 
        //     from score in scores
        //     select score;

        // var highScore = scoreQuery.Max();

        // IEnumerable<City> largeCities = (
        //     from city in cities
        //     where city.Population > 30_000_000
        //     select city
        // );

        // foreach (City city in largeCities)
        // {
        //     Console.WriteLine(city);
        // }

        // //groupClause
        // var queryCounryGroups = from country in countries
        //                         group country by country.Name[0];

        // foreach (IGrouping<char, Country> country in queryCounryGroups)
        // {
        //     Console.WriteLine($"{country.Key}");
        //     foreach (var c in country)
        //     {
        //         Console.WriteLine($"{c.Name}, {c.Area}, {c.Population}");
        //     }
        // }

        // // requirement of adding a var type when select clause projects a sequence of anonymous types that contains only a subset of the field in the original element. 
        // var queryNameAndPop =
        //     from country in countries
        //     select new
        //     {
        //         Name = country.Name,
        //         Pop = country.Population
        //     };
        
        // foreach (var item in queryNameAndPop)
        // {
        //     System.Console.WriteLine(item);
        // }

        // // use into keyword in a select or group clause to create a temporary identifier that stores a query

        // var percentileQuery = 
        //     from country in countries
        //     let popInThousands = (int)country.Population / 1_000
        //     group country by popInThousands into countryGroup
        //     where countryGroup.Key >= 20
        //     select countryGroup;

        // foreach (var grouping in percentileQuery)
        // {
        //     Console.WriteLine(grouping.Key);
        //     foreach (var country in grouping)
        //     {
        //         Console.WriteLine(country.Name + ":" + country.Population);
        //     }
        // }

        // // where clause is used to filter source data based on one or more predicates
        // // orderby to sort asc or desc

        // IEnumerable<City> queryCityPop =
        //     from city in cities
        //     where city.Population is < 15_000_000 and > 10_000_000
        //     orderby city.Population
        //     select city;

        // Join clause: combine two data sources based on an equality comparision between specified keys in each element
    //     // just an example
    //     var categoryQuery =
            // from cat in categories
            // join prod in products on cat equals prod.Category
            // select new
            // {
            //     Category = cat,
            //     Name = prod.Name
            // };

        // the let clause allows us to store the result of an expression, such as a method call, in a new variable.
        // useful when we want to use a calculation again in that query

        var denseCountries =
            from country in countries
            let density = country.Population / country.Area
            where density > 100_000
            select new { country.Name, Density = density };
        // let is used for creating a field that will be used again while into is used to continue query after groupby so we can select on it.
    }
}
