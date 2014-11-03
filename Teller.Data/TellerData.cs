namespace Teller.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    using Teller.Data.Repositories;
    using Teller.Models;

    public class TellerData : ITellerData
    {
        private DbContext context;
        private IDictionary<Type, object> repositories;

        public TellerData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<AppUser> Users
        {
            get { return this.GetRepository<AppUser>(); }
        }

        public IRepository<Comment> Comments
        {
            get { return this.GetRepository<Comment>(); }
        }

        public IRepository<CommentLike> CommentLikes
        {
            get { return this.GetRepository<CommentLike>(); }
        }

        public IRepository<Genre> Genres
        {
            get { return this.GetRepository<Genre>(); }
        }

        public IRepository<Like> Likes
        {
            get { return this.GetRepository<Like>(); }
        }

        public IRepository<Series> Series
        {
            get { return this.GetRepository<Series>(); }
        }

        public IRepository<Story> Stories
        {
            get { return this.GetRepository<Story>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfRepository = typeof(T);
            if(!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepository = Activator.CreateInstance(typeof(Repository<T>), context);
                this.repositories.Add(typeOfRepository, newRepository);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}
