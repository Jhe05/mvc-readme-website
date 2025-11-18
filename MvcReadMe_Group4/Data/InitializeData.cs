using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcReadMe_Group4.Data;
using MvcReadMe_Group4.Models;

namespace MvcReadMe_Group4.Data
{
    public static class InitializeData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcReadMe_Group4Context(
                serviceProvider.GetRequiredService<DbContextOptions<MvcReadMe_Group4Context>>()))
            {


                // Check if we already have data
                if (context.Books.Any())
                {
                    return;   // database has been seeded
                }
                else
                {
                    // Clear existing data
                    context.BookReads.RemoveRange(context.BookReads);
                    context.Books.RemoveRange(context.Books);
                    context.Users.RemoveRange(context.Users);
                    context.SaveChanges();

                }


                // Add books
                var books = new List<Book>
                {
                    new Book
                    {
                        Title = "Python Crash Course",
                        Author = "Eric Matthes",
                        Category = "Programming",
                        Description = "A hands-on, project-based introduction to programming with Python.",
                        ISBN = "9781593279288",
                        FilePath = "/assets/books/python-crash-course.pdf",
                        CoverImagePath = "/assets/img/covers/python-crash-course-cover.jpg",
                        NumberOfReads = 945
                    },
                    new Book
                    {
                        Title = "Atomic Habits",
                        Author = "James Clear",
                        Category = "Self-Help",
                        Description = "An easy & proven way to build good habits and break bad ones.",
                        ISBN = "9780735211292",
                        FilePath = "/assets/books/atomic-habits.pdf",
                        CoverImagePath = "/assets/img/covers/atomic-habits-cover.jpg",
                        NumberOfReads = 987
                    },
                    new Book
                    {
                        Title = "The Midnight Library",
                        Author = "Matt Haig",
                        Category = "Fiction",
                        Description = "A woman finds herself in a library between life and death, where every book represents a different life she could have lived.",
                        ISBN = "9780525559474",
                        FilePath = "/assets/books/the-midnight-library.pdf",
                        CoverImagePath = "/assets/img/covers/the-midnight-library-cover.jpg",
                        NumberOfReads = 889
                    },
                    new Book
                    {
                        Title = "Data Science from Scratch",
                        Author = "Joel Grus",
                        Category = "Programming",
                        Description = "Learn data science fundamentals with Python, including statistics, machine learning, and data visualization.",
                        ISBN = "9781492041139",
                        FilePath = "/assets/books/data-science-from-scratch.pdf",
                        CoverImagePath = "/assets/img/covers/data-science-from-scratch-cover.jpg",
                        NumberOfReads = 856
                    },
                    new Book
                    {
                        Title = "Ikigai",
                        Author = "Hector Garcia & Francesc Miralles",
                        Category = "Self-Help",
                        Description = "The Japanese secret to a long and happy life.",
                        ISBN = "9780143130727",
                        FilePath = "/assets/books/ikigai.pdf",
                        CoverImagePath = "/assets/img/covers/ikigai-cover.jpg",
                        NumberOfReads = 823
                    },
                    new Book
                    {
                        Title = "Introduction to Algorithms",
                        Author = "Thomas H. Cormen",
                        Category = "Programming",
                        Description = "A comprehensive introduction to algorithms and their analysis.",
                        ISBN = "9780262033848",
                        FilePath = "/assets/books/introduction-to-algorithms.pdf",
                        CoverImagePath = "/assets/img/covers/introduction-to-algorithms-cover.jpg",
                        NumberOfReads = 412
                    },
                    new Book
                    {
                        Title = "Ego is the Enemy",
                        Author = "Ryan Holiday",
                        Category = "Self-Help",
                        Description = "A powerful exploration of how ego can be the biggest obstacle to success.",
                        ISBN = "9781591846352",
                        FilePath = "/assets/books/ego-is-the-enemy.pdf",
                        CoverImagePath = "/assets/img/covers/ego-is-the-enemy-cover.jpg",
                        NumberOfReads = 278
                    },
                    new Book
                    {
                        Title = "Algorithms to Live By",
                        Author = "Brian Christian & Tom Griffiths",
                        Category = "Self-Help",
                        Description = "A fascinating exploration of how computer algorithms can be applied to our everyday lives.",
                        ISBN = "9781627790369",
                        FilePath = "/assets/books/algorithms-to-live-by.pdf",
                        CoverImagePath = "/assets/img/covers/algorithms-to-live-by-cover.jpg",
                        NumberOfReads = 234
                    },
                    new Book
                    {
                        Title = "The Mountain is You",
                        Author = "Brianna Wiest",
                        Category = "Self-Help",
                        Description = "A transformative guide to self-sabotage and how to overcome it.",
                        ISBN = "9781949759222",
                        FilePath = "/assets/books/the-mountain-is-you.pdf",
                        CoverImagePath = "/assets/img/covers/the-mountain-is-you-cover.jpg",
                        NumberOfReads = 167
                    },
                    new Book
                    {
                        Title = "Philosophy 101",
                        Author = "Paul Kleinman",
                        Category = "Non-Fiction",
                        Description = "From Plato and Socrates to ethics and metaphysics, an essential primer on philosophical thought.",
                        ISBN = "9781440541763",
                        FilePath = "/assets/books/philosophy-101.pdf",
                        CoverImagePath = "/assets/img/covers/philosophy-101-cover.jpg",
                        NumberOfReads = 912
                    },
                    new Book
                    {
                        Title = "A Gentle Reminder",
                        Author = "Bianca Sparacino",
                        Category = "Self-Help",
                        Description = "A collection of poetry and prose about healing, growth, and self-discovery.",
                        ISBN = "9781734447000",
                        FilePath = "/assets/books/a-gentle-reminder.pdf",
                        CoverImagePath = "/assets/img/covers/a-gentle-reminder-cover.jpg",
                        NumberOfReads = 189
                    },
                    new Book
                    {
                        Title = "The Psychology of Money",
                        Author = "Morgan Housel",
                        Category = "Business",
                        Description = "Timeless lessons on wealth, greed, and happiness.",
                        ISBN = "9780857197689",
                        FilePath = "/assets/books/the-psychology-of-money.pdf",
                        CoverImagePath = "/assets/img/covers/the-psychology-of-money-cover.jpg",
                        NumberOfReads = 790
                    },
                    new Book
                    {
                        Title = "Grokking Deep Learning",
                        Author = "Andrew Trask",
                        Category = "Programming",
                        Description = "A practical guide to deep learning that teaches you how to build neural networks from scratch.",
                        ISBN = "9781617293702",
                        FilePath = "/assets/books/grokking-deep-learning.pdf",
                        CoverImagePath = "/assets/img/covers/grokking-deep-learning-cover.jpg",
                        NumberOfReads = 289
                    },
                    new Book
                    {
                        Title = "The Alchemist",
                        Author = "Paulo Coelho",
                        Category = "Fiction",
                        Description = "A philosophical novel about following your dreams and listening to your heart.",
                        ISBN = "9780062315007",
                        FilePath = "/assets/books/the-alchemist.pdf",
                        CoverImagePath = "/assets/img/covers/the-alchemist-cover.jpg",
                        NumberOfReads = 789
                    },
                    new Book
                    {
                        Title = "Sapiens",
                        Author = "Yuval Noah Harari",
                        Category = "Non-Fiction",
                        Description = "A brief history of humankind from ancient humans to the present.",
                        ISBN = "9780062316097",
                        FilePath = "/assets/books/sapiens.pdf",
                        CoverImagePath = "/assets/img/covers/sapiens-cover.jpg",
                        NumberOfReads = 567
                    },
                    new Book
                    {
                        Title = "Rich Dad Poor Dad",
                        Author = "Robert T. Kiyosaki",
                        Category = "Business",
                        Description = "What the rich teach their kids about money that the poor and middle class do not.",
                        ISBN = "9781612680194",
                        FilePath = "/assets/books/rich-dad-poor-dad.pdf",
                        CoverImagePath = "/assets/img/covers/rich-dad-poor-dad-cover.jpg",
                        NumberOfReads = 892
                    },
                    new Book
                    {
                        Title = "Zero to One",
                        Author = "Peter Thiel",
                        Category = "Business",
                        Description = "Notes on startups, or how to build the future.",
                        ISBN = "9780804139298",
                        FilePath = "/assets/books/zero-to-one.pdf",
                        CoverImagePath = "/assets/img/covers/zero-to-one-cover.jpg",
                        NumberOfReads = 345
                    },
                    new Book
                    {
                        Title = "Think and Grow Rich",
                        Author = "Napoleon Hill",
                        Category = "Business",
                        Description = "A classic guide to personal achievement and wealth building.",
                        ISBN = "9781585424337",
                        FilePath = "/assets/books/think-and-grow-rich.pdf",
                        CoverImagePath = "/assets/img/covers/think-and-grow-rich-cover.jpg",
                        NumberOfReads = 789
                    },
                    new Book
                    {
                        Title = "Cracking the Coding Interview",
                        Author = "Gayle Laakmann McDowell",
                        Category = "Programming",
                        Description = "A comprehensive guide to technical interviews with 189 programming questions and solutions.",
                        ISBN = "9780984782857",
                        FilePath = "/assets/books/cracking-the-coding-interview.pdf",
                        CoverImagePath = "/assets/img/covers/cracking-the-coding-interview-cover.jpg",
                        NumberOfReads = 678
                    },
                    new Book
                    {
                        Title = "ASP.NET Core in Action",
                        Author = "Andrew Lock",
                        Category = "Programming",
                        Description = "A comprehensive guide to building web applications with ASP.NET Core.",
                        ISBN = "9781617294617",
                        FilePath = "/assets/books/asp-net-core-in-action.pdf",
                        CoverImagePath = "/assets/img/covers/asp-net-core-in-action-cover.jpg",
                        NumberOfReads = 178
                    },
                    new Book
                    {
                        Title = "Memory Man",
                        Author = "David Baldacci",
                        Category = "Fiction",
                        Description = "A thrilling mystery featuring Amos Decker, a detective with a perfect memory.",
                        ISBN = "9781455586384",
                        FilePath = "/assets/books/memory-man.pdf",
                        CoverImagePath = "/assets/img/covers/memory-man-cover.jpg",
                        NumberOfReads = 312
                    },
                    new Book
                    {
                        Title = "The Hobbit",
                        Author = "J.R.R. Tolkien",
                        Category = "Fiction",
                        Description = "A young hobbit's journey to win a treasure guarded by a dragon, and the friends he makes along the way.",
                        ISBN = "9780345339683",
                        FilePath = "/assets/books/the-hobbit.pdf",
                        CoverImagePath = "/assets/img/covers/the-hobbit-cover.jpg",
                        NumberOfReads = 654
                    },
                    new Book
                    {
                        Title = "The Compound Effect",
                        Author = "Darren Hardy",
                        Category = "Business",
                        Description = "A practical guide to achieving success through small, consistent actions.",
                        ISBN = "9781593157241",
                        FilePath = "/assets/books/the-compound-effect.pdf",
                        CoverImagePath = "/assets/img/covers/the-compound-effect-cover.jpg",
                        NumberOfReads = 412
                    },
                    new Book
                    {
                        Title = "The Intelligent Investor",
                        Author = "Benjamin Graham",
                        Category = "Business",
                        Description = "The definitive book on value investing.",
                        ISBN = "9780060555665",
                        FilePath = "/assets/books/the-intelligent-investor.pdf",
                        CoverImagePath = "/assets/img/covers/the-intelligent-investor-cover.jpg",
                        NumberOfReads = 678
                    },
                    new Book
                    {
                        Title = "The Art of War",
                        Author = "Sun Tzu",
                        Category = "Non-Fiction",
                        Description = "An ancient Chinese military treatise on strategy and tactics.",
                        ISBN = "9780140439199",
                        FilePath = "/assets/books/the-art-of-war.pdf",
                        CoverImagePath = "/assets/img/covers/art-of-war-cover.jpg",
                        NumberOfReads = 456
                    },
                    new Book
                    {
                        Title = "48 Laws of Power",
                        Author = "Robert Greene",
                        Category = "Non-Fiction",
                        Description = "A practical guide to understanding power dynamics and human nature.",
                        ISBN = "9780140280197",
                        FilePath = "/assets/books/the-48-laws-of-power.pdf",
                        CoverImagePath = "/assets/img/covers/the-48-laws-of-power-cover.jpg",
                        NumberOfReads = 789
                    },
                    new Book
                    {
                        Title = "Sherlock Holmes",
                        Author = "Arthur Conan Doyle",
                        Category = "Fiction",
                        Description = "The complete collection of Sherlock Holmes stories featuring the world's most famous detective.",
                        ISBN = "9780140439070",
                        FilePath = "/assets/books/sherlock-holmes.pdf",
                        CoverImagePath = "/assets/img/covers/sherlock-holmes-cover.jpg",
                        NumberOfReads = 567
                    },
                    new Book
                    {
                        Title = "Man's Search for Meaning",
                        Author = "Viktor E. Frankl",
                        Category = "Non-Fiction",
                        Description = "A psychiatrist's personal experience of life in Nazi concentration camps and the search for meaning in suffering.",
                        ISBN = "9780807014295",
                        FilePath = "/assets/books/man-search-for-meaning.pdf",
                        CoverImagePath = "/assets/img/covers/man-search-for-meaning-cover.jpg",
                        NumberOfReads = 241
                    },
                    new Book
                    {
                        Title = "The Little Prince",
                        Author = "Antoine de Saint-Exupéry",
                        Category = "Fiction",
                        Description = "A poetic tale about a young prince who visits various planets in space.",
                        ISBN = "9780156013987",
                        FilePath = "/assets/books/the-little-prince.pdf",
                        CoverImagePath = "/assets/img/covers/the-little-prince-cover.jpg",
                        NumberOfReads = 456
                    },
                    new Book
                    {
                        Title = "The History Book",
                        Author = "DK",
                        Category = "Non-Fiction",
                        Description = "A comprehensive guide to world history from ancient times to the present day.",
                        ISBN = "9781465445100",
                        FilePath = "/assets/books/the-history-book.pdf",
                        CoverImagePath = "/assets/img/covers/the-history-book-cover.jpg",
                        NumberOfReads = 234
                    }
                };
                context.Books.AddRange(books);
                context.SaveChanges();


                // Add users
                var users = new List<User>
                {
                    new User { UserName = "Admin", Password = "admin_123", Role = "Admin" },
                    new User { UserName = "nicko_712", Password = "tralaleo23" },
                    new User { UserName = "jheris456", Password = "crocodillo45" },
                    new User { UserName = "reinald.42", Password = "bananini789" },
                    new User { UserName = "pythonic712", Password = "skibidi99" },
                    new User { UserName = "tech_guru_99", Password = "potatohead11" },
                    new User { UserName = "coderx_77", Password = "zombietaco21" },
                    new User { UserName = "skywalker001", Password = "sharknado42", Role = "User" },
                    new User { UserName = "dreamer_88", Password = "spongebobrocks23" },
                    new User { UserName = "pixelwizard_12", Password = "clownfish@201" },
                    new User { UserName = "ninja_42", Password = "fanumtax77" },
                    new User { UserName = "binarybee99", Password = "mechacode32" },
                    new User { UserName = "cloudybrain", Password = "drizzledev88" },
                    new User { UserName = "gadgetqueen", Password = "robochic11" },
                    new User { UserName = "bytebrawler", Password = "matrixrush45" },
                    new User { UserName = "cosmicdevv", Password = "plutocoder69" }
                };
                context.Users.AddRange(users);
                context.SaveChanges();


                // Add BookReads with fixed sample numbers for past 7 days
                var bookReads = new List<BookRead>();
                var today = DateTime.Today;

                // Fixed read counts for each day, where the latest and current day can increment
                var dailyReads = new[] { 58, 49, 35, 42, 23, 35, 28 };

                for (int i = 0; i < 7; i++)
                {
                    bookReads.Add(new BookRead
                    {
                        ReadDate = today.AddDays(-i),
                        ReadCount = dailyReads[i],
                        // Associate seeded reads with the first seeded book to
                        // satisfy the non-nullable BookId foreign key.
                        BookId = books.Count > 0 ? books[0].Id : 0
                    });
                }
                context.BookReads.AddRange(bookReads);
                context.SaveChanges();
            }
        }
    }
}