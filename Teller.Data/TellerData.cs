namespace Teller.Data
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNet.Identity.EntityFramework;
    
    using Teller.Data.Repositories;
    using Teller.Models;

    public class TellerData : ITellerData
    {
        private ITellerDbContext context;
        private IDictionary<Type, object> repositories;

        public TellerData(ITellerDbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public ITellerDbContext Context
        {
            get
            {
                return this.context;
            }
        }

        public IRepository<AppUser> Users
        {
            get { return this.GetRepository<AppUser>(); }
        }

        public IRepository<IdentityRole> Roles
        {
            get { return this.GetRepository<IdentityRole>(); }
        }

        public IRepository<Comment> Comments
        {
            get { return this.GetRepository<Comment>(); }
        }

        public IRepository<CommentLike> CommentLikes
        {
            get { return this.GetRepository<CommentLike>(); }
        }

        public IRepository<Flag> Flags
        {
            get { return this.GetRepository<Flag>(); }
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

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                }
            }
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfRepository = typeof(T);
            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepository = Activator.CreateInstance(typeof(Repository<T>), this.context);
                this.repositories.Add(typeOfRepository, newRepository);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}
