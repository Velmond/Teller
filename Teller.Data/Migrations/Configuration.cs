namespace Teller.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Text;

    using Microsoft.AspNet.Identity.EntityFramework;

    using Teller.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TellerDbContext>
    {
        private static Random rand = new Random();

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Teller.Data.TellerDbContext context)
        {
            SeedGenres(context);
            SeedUserRoles(context);
            //// SeedStories(context);
        }

        private static void SeedUserRoles(Teller.Data.TellerDbContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole() { Name = "User" });
                context.Roles.Add(new IdentityRole() { Name = "Banned" });
                context.Roles.Add(new IdentityRole() { Name = "Restricted" });
                context.Roles.Add(new IdentityRole() { Name = "Admin" });
                context.SaveChanges();
            }
        }

        private static void SeedGenres(Teller.Data.TellerDbContext context)
        {
            if (!context.Genres.Any())
            {
                context.Genres.Add(new Genre() { Name = "Adventure" });
                context.Genres.Add(new Genre() { Name = "Airport novel" });
                context.Genres.Add(new Genre() { Name = "Allegory" });
                context.Genres.Add(new Genre() { Name = "Bildungsroman" });
                context.Genres.Add(new Genre() { Name = "Black comedy" });
                context.Genres.Add(new Genre() { Name = "Blog" });
                context.Genres.Add(new Genre() { Name = "Comedy" });
                context.Genres.Add(new Genre() { Name = "Comedy-drama" });
                context.Genres.Add(new Genre() { Name = "Farce" });
                context.Genres.Add(new Genre() { Name = "Crime" });
                context.Genres.Add(new Genre() { Name = "Detective" });
                context.Genres.Add(new Genre() { Name = "Epic" });
                context.Genres.Add(new Genre() { Name = "Epistolary" });
                context.Genres.Add(new Genre() { Name = "Fantasy" });
                context.Genres.Add(new Genre() { Name = "Genre" });
                context.Genres.Add(new Genre() { Name = "Gothic" });
                context.Genres.Add(new Genre() { Name = "Horror" });
                context.Genres.Add(new Genre() { Name = "Melodrama" });
                context.Genres.Add(new Genre() { Name = "Mystery" });
                context.Genres.Add(new Genre() { Name = "Pastiche" });
                context.Genres.Add(new Genre() { Name = "Picaresque" });
                context.Genres.Add(new Genre() { Name = "Parody" });
                context.Genres.Add(new Genre() { Name = "Romance" });
                context.Genres.Add(new Genre() { Name = "Romantic comedy" });
                context.Genres.Add(new Genre() { Name = "Romp" });
                context.Genres.Add(new Genre() { Name = "Satire" });
                context.Genres.Add(new Genre() { Name = "Science fiction" });
                context.Genres.Add(new Genre() { Name = "Swashbuckler" });
                context.Genres.Add(new Genre() { Name = "Thriller" });
                context.Genres.Add(new Genre() { Name = "Tragedy" });
                context.Genres.Add(new Genre() { Name = "Tragicomedy" });
                context.Genres.Add(new Genre() { Name = "Travelogue" });
                context.SaveChanges();
            }
        }

        private void SeedStories(Teller.Data.TellerDbContext context)
        {
            if (!context.Stories.Any())
            {
                var genres = context.Genres.ToList();
                var users = new AppUser[]
                {
                    new AppUser() { UserName = "1Anonimous1" },
                    new AppUser() { UserName = "2Anonimous2" },
                    new AppUser() { UserName = "3Anonimous3" }
                };

                context.SaveChanges();

                for (int i = 0; i < 100; i++)
                {
                    var user = users[rand.Next(0, users.Length)];

                    var story = new Story()
                    {
                        Title = this.GenerateStoryTitle(),
                        Content = this.GenerateStoryContent(),
                        Author = user,
                        AuthorId = user.Id,
                        Genre = genres[rand.Next(0, genres.Count)],
                        GenreId = genres[rand.Next(0, genres.Count)].Id,
                        DatePublished = DateTime.Now.AddDays(rand.Next(-5, 5)),
                        PicturePath = ".."
                    };

                    context.Stories.Add(story);
                    context.SaveChanges();
                }
            }
        }

        private string GenerateStoryTitle()
        {
            var allowedSymbols = "qwertyuiopas dfghjklzxcvbnm QWERTYUIOPAS DFGHJKLZXCVBNM 1234567890 !?.,;: ";
            var titleLength = rand.Next(2, 100);
            var title = new StringBuilder();

            for (int i = 0; i < titleLength; i++)
            {
                title.Append(allowedSymbols[rand.Next(0, allowedSymbols.Length)]);
            }

            return title.ToString();
        }

        private string GenerateStoryContent()
        {
            var allowedSymbols = "qwertyuiopas dfghjklzxcvbnm QWERTYUIOPAS DFGHJKLZXCVBNM 1234567890 !?.,;: ";
            var contentLength = rand.Next(100, 5000);
            var content = new StringBuilder();

            for (int i = 0; i < contentLength; i++)
            {
                content.Append(allowedSymbols[rand.Next(0, allowedSymbols.Length)]);
            }

            return content.ToString();
        }
    }
}
