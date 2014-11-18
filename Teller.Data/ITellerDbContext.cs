namespace Teller.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    using Teller.Models;

    public interface ITellerDbContext
    {
        IDbSet<Comment> Comments { get; set; }

        IDbSet<CommentLike> CommentLikes { get; set; }

        IDbSet<Flag> Flags { get; set; }

        IDbSet<Genre> Genres { get; set; }

        IDbSet<Like> Likes { get; set; }

        IDbSet<Series> Series { get; set; }

        IDbSet<Story> Stories { get; set; }

        DbContext DbContext { get; }

        int SaveChanges();

        void Dispose();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        IDbSet<T> Set<T>() where T : class;
    }
}
