namespace Teller.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Teller.Data.Migrations;
    using Teller.Models;


    public class TellerDbContext : IdentityDbContext<AppUser>
    {
        public TellerDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TellerDbContext, Configuration>());
        }

        public IDbSet<Comment> Comments { get; set; }

        public IDbSet<CommentLike> CommentLikes { get; set; }

        public IDbSet<Flag> Flags { get; set; }

        public IDbSet<Genre> Genres { get; set; }

        public IDbSet<Like> Likes { get; set; }

        public IDbSet<Series> Series { get; set; }

        public IDbSet<Story> Stories { get; set; }

        public static TellerDbContext Create()
        {
            return new TellerDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
              .HasMany(u => u.Favourites)
              .WithMany(s => s.FavouritedBy)
              .Map(
               m =>
               {
                   m.MapLeftKey("UserId");
                   m.MapRightKey("StoryId");
                   m.ToTable("FavouriteStories");
               });

            modelBuilder.Entity<AppUser>()
              .HasMany(u => u.ReadLater)
              .WithMany(s => s.ToBeReadBy)
              .Map(
               m =>
               {
                   m.MapLeftKey("UserId");
                   m.MapRightKey("StoryId");
                   m.ToTable("ReadLaterStories");
               });

            modelBuilder.Entity<AppUser>()
              .HasMany(u => u.Subscribers)
              .WithMany(u => u.SubscribedTo)
              .Map(
               m =>
               {
                   m.MapLeftKey("UserId");
                   m.MapRightKey("SubscriberId");
                   m.ToTable("UserSubscripitons");
               });

            base.OnModelCreating(modelBuilder);
        }
    }
}
