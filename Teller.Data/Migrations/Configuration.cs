namespace Teller.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    
    using Teller.Common;
    using Teller.Common.DataGenerators;
    using Teller.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TellerDbContext>
    {
        private IRandomGenerator random;
        private UserManager<AppUser> userManager;

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            this.random = new RandomGenerator();
        }

        protected override void Seed(TellerDbContext context)
        {
            this.userManager = new UserManager<AppUser>(new UserStore<AppUser>(context));
            this.SeedGenres(context);
            this.SeedRoles(context);
            this.SeedUsers(context);
            this.SeedStories(context);
        }

        private void SeedRoles(TellerDbContext context)
        {
            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole(GlobalConstants.AdministratorRoleName));
            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole(GlobalConstants.DefaultUserRoleName));
            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole(GlobalConstants.BannedRoleName));
            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole(GlobalConstants.RestrictedRoleName));
            context.SaveChanges();
        }

        private void SeedGenres(TellerDbContext context)
        {
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Adventure" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Airport novel" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Allegory" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Bildungsroman" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Black comedy" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Blog" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Comedy" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Comedy-drama" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Farce" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Crime" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Detective" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Epic" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Epistolary" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Fantasy" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Genre" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Gothic" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Horror" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Melodrama" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Mystery" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Pastiche" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Picaresque" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Parody" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Romance" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Romantic comedy" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Romp" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Satire" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Science fiction" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Swashbuckler" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Thriller" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Tragedy" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Tragicomedy" });
            context.Genres.AddOrUpdate(x => x.Name, new Genre() { Name = "Travelogue" });
            context.SaveChanges();
        }

        private void SeedUsers(TellerDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            for (int i = 0; i < 10; i++)
            {
                var username = this.random.RandomString(6, 16);
                var email = string.Format("{0}@{1}.{2}", username, this.random.RandomString(3, 5), this.random.RandomString(2, 3));

                var user = new AppUser
                {
                    Email = email,
                    UserName = username,
                    RegisteredOn = random.RandomDate(DateTime.Now.AddDays(-15), DateTime.Now.AddDays(-5))
                };

                this.userManager.Create(user, "qweqwe");

                user.UserInfo = new UserInfo()
                {
                    AvatarPath = GlobalConstants.DefaultUserAvatarPicturePath,
                    Motto = this.random.RandomString(10, 100),
                    Description = this.random.RandomString(100, 1000)
                };

                user.UserInfo.LinkedProfiles = new LinkedProfiles();

                context.SaveChanges();

                this.userManager.AddToRole(user.Id, GlobalConstants.DefaultUserRoleName);
            }

            var adminUser = new AppUser
            {
                Email = "admin@admin.com",
                UserName = "Administrator",
                RegisteredOn = random.RandomDate(DateTime.Now.AddDays(-15), DateTime.Now.AddDays(-5))
            };

            this.userManager.Create(adminUser, "qweqwe");

            this.userManager.AddToRole(adminUser.Id, GlobalConstants.AdministratorRoleName);
        }

        private void SeedStories(TellerDbContext context)
        {
            if (context.Stories.Any())
            {
                return;
            }

            var genresIds = context.Genres.Select(g => g.Id).ToList();
            var usersIds = context.Users.Select(g => g.Id).ToList();

            for (int i = 0; i < usersIds.Count; i++)
            {
                var userId = usersIds[i];
                var storiesCount = this.random.RandomNumber(0, 100);

                for (int j = 0; j < storiesCount; j++)
                {
                    this.GenerateUserSeries(context, userId);

                    var userSeriesIds = context.Users
                        .SingleOrDefault(u => u.Id == userId)
                        .Series
                        .Select(s => s.Id)
                        .ToList();

                    var story = new Story()
                    {
                        Title = random.RandomString(5, 99),
                        Content = random.RandomString(100, 2000),
                        AuthorId = userId,
                        GenreId = genresIds[random.RandomNumber(0, genresIds.Count - 1)],
                        DatePublished = random.RandomDate(DateTime.Now.AddDays(-5), DateTime.Now.AddDays(5)),
                        PicturePath = GlobalConstants.DefaultStoryPicturePath,
                        ViewsCount = random.RandomNumber(0, 1000)
                    };

                    if (userSeriesIds.Count == 0)
                    {
                        story.SeriesId = null;
                    }
                    else
                    {
                        story.SeriesId = userSeriesIds[random.RandomNumber(0, userSeriesIds.Count - 1)];
                    }

                    context.Stories.Add(story);
                    context.SaveChanges();

                    this.GenerateComments(context, story.Id);
                    this.GenerateLikes(context, story.Id);
                }

                context.SaveChanges();
            }
        }

        private void GenerateUserSeries(TellerDbContext context, string userId)
        {
            var genresIds = context.Genres.Select(g => g.Id).ToList();
            var seriesCount = this.random.RandomNumber(0, 10);

            for (int i = 0; i < seriesCount; i++)
            {
                var series = new Series()
                {
                    Title = random.RandomString(10, 50),
                    AuthorId = userId,
                    GenreId = genresIds[random.RandomNumber(0, genresIds.Count - 1)]
                };

                context.Series.Add(series);
            }

            context.SaveChanges();
        }

        private void GenerateLikes(TellerDbContext context, int stortId)
        {
            var userIds = context.Users.Select(u => u.Id).ToList();
            var likesCount = this.random.RandomNumber(0, userIds.Count - 1);

            for (int i = 0; i < likesCount; i++)
            {
                var userId = userIds[this.random.RandomNumber(0, userIds.Count - 1)];

                var like = new Like()
                {
                    AuthorId = userId,
                    Value = (this.random.RandomNumber(0, 100) % 4 == 0 ? true : false),
                    StoryId = stortId
                };

                userIds.Remove(userId);
                context.Likes.Add(like);
            }

            context.SaveChanges();
        }

        private void GenerateComments(TellerDbContext context, int storyId)
        {
            var userIds = context.Users.Select(u => u.Id).ToList();
            var commentsCount = this.random.RandomNumber(3, 8);

            for (int i = 0; i < commentsCount; i++)
            {
                var comment = new Comment()
                {
                    AuthorId = userIds[this.random.RandomNumber(0, userIds.Count - 1)],
                    Content = this.random.RandomString(100, 600),
                    IsFlagged = (this.random.RandomNumber(0, 100) % 4 == 0 ? true : false),
                    Published = this.random.RandomDate(DateTime.Now.AddDays(-5), DateTime.Now.AddDays(5)),
                    StoryId = storyId
                };

                context.Comments.Add(comment);
                context.SaveChanges();

                this.GenerateCommentLikes(context, comment.Id);
            }
        }

        private void GenerateCommentLikes(TellerDbContext context, int commentId)
        {
            var userIds = context.Users.Select(u => u.Id).ToList();
            var likesCount = this.random.RandomNumber(0, userIds.Count - 1);

            for (int i = 0; i < likesCount; i++)
            {
                var userId = userIds[this.random.RandomNumber(0, userIds.Count - 1)];
                var commentLike = new CommentLike()
                {
                    AuthorId = userId,
                    Value = (this.random.RandomNumber(0, 100) % 4 == 0 ? true : false),
                    CommentId = commentId
                };

                userIds.Remove(userId);
                context.CommentLikes.Add(commentLike);
            }

            context.SaveChanges();
        }

        ////private void SeedStoriesWithComments(TellerDbContext context)
        ////{
        ////    if (!context.Stories.Any())
        ////    {
        ////        var genres = context.Genres.ToList();
        ////        var users = new AppUser[]
        ////        {
        ////            new AppUser() { UserName = "1Anonimous1" },
        ////            new AppUser() { UserName = "2Anonimous2" },
        ////            new AppUser() { UserName = "3Anonimous3" }
        ////        };
        ////        context.SaveChanges();
        ////        for (int i = 0; i < 100; i++)
        ////        {
        ////            var user = users[this.random.RandomNumber(0, users.Length - 1)];
        ////            var story = new Story()
        ////            {
        ////                Title = this.GenerateStoryTitle(),
        ////                Content = this.GenerateStoryContent(),
        ////                Author = user,
        ////                AuthorId = user.Id,
        ////                Genre = genres[this.random.RandomNumber(0, genres.Count - 1)],
        ////                GenreId = genres[this.random.RandomNumber(0, genres.Count - 1)].Id,
        ////                DatePublished = this.random.RandomDate(DateTime.Now.AddDays(-5), DateTime.Now.AddDays(5)),
        ////                PicturePath = GlobalConstants.DefaultStoryPicturePath
        ////            };
        ////            context.Stories.Add(story);
        ////            context.SaveChanges();
        ////        }
        ////    }
        ////}

        ////private string GenerateStoryTitle()
        ////{
        ////    var allowedSymbols = "qwertyuiopas dfghjklzxcvbnm QWERTYUIOPAS DFGHJKLZXCVBNM 1234567890 !?.,;: ";
        ////    var titleLength = this.random.RandomNumber(2, 100);
        ////    var title = new StringBuilder();
        ////    for (int i = 0; i < titleLength; i++)
        ////    {
        ////        title.Append(allowedSymbols[this.random.RandomNumber(0, allowedSymbols.Length)]);
        ////    }
        ////    return title.ToString();
        ////}

        ////private string GenerateStoryContent()
        ////{
        ////    var allowedSymbols = "qwertyuiopas dfghjklzxcvbnm QWERTYUIOPAS DFGHJKLZXCVBNM 1234567890 !?.,;: ";
        ////    var contentLength = this.random.RandomNumber(100, 5000);
        ////    var content = new StringBuilder();
        ////    for (int i = 0; i < contentLength; i++)
        ////    {
        ////        content.Append(allowedSymbols[this.random.RandomNumber(0, allowedSymbols.Length)]);
        ////    }
        ////    return content.ToString();
        ////}
    }
}
