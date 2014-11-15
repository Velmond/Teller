﻿namespace Teller.Web.Controllers.Base
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Teller.Data.UnitsOfWork;
    using Teller.Models;

    [HandleError]
    public abstract class BaseController : Controller
    {
        public BaseController(ITellerData data)
        {
            this.Data = data;
        }

        public ITellerData Data { get; set; }

        public AppUser UserProfile { get; set; }

        [NonAction]
        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            var identity = System.Web.HttpContext.Current.User.Identity;
            if (identity.IsAuthenticated)
            {
                this.UserProfile = this.Data.Users.All().SingleOrDefault(u => u.UserName == identity.Name);
            }

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}