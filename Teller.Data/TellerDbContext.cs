namespace Teller.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using Microsoft.AspNet.Identity.EntityFramework;
    
    using Teller.Data.Migrations;
    using Teller.Models;

    public class TellerDbContext : IdentityDbContext<AppUser>, ITellerDbContext
    {
        public TellerDbContext()
            : this("DefaultConnection")
        {
        }

        public TellerDbContext(string connectionString)
            : base(connectionString, throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TellerDbContext, Configuration>());
        }

        public virtual IDbSet<Comment> Comments { get; set; }

        public virtual IDbSet<CommentLike> CommentLikes { get; set; }

        public virtual IDbSet<Flag> Flags { get; set; }

        public virtual IDbSet<Genre> Genres { get; set; }

        public virtual IDbSet<Like> Likes { get; set; }

        public virtual IDbSet<Series> Series { get; set; }

        public virtual IDbSet<Story> Stories { get; set; }

        public DbContext DbContext
        {
            get
            {
                return this;
            }
        }

        public static TellerDbContext Create()
        {
            return new TellerDbContext();
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
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
