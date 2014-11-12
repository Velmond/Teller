namespace Teller.Data
{
    using Teller.Data.Repositories;
    using Teller.Models;

    public interface ITellerData
    {
        ITellerDbContext Context { get; }

        IRepository<AppUser> Users { get; }

        IRepository<Comment> Comments { get; }

        IRepository<CommentLike> CommentLikes { get; }

        IRepository<Genre> Genres { get; }

        IRepository<Like> Likes { get; }

        IRepository<Series> Series { get; }

        IRepository<Story> Stories { get; }

        int SaveChanges();
    }
}
