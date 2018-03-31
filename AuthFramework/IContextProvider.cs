using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace AuthFramework
{
    public interface IContextProvider
    {
        /// <summary>
        /// Creates context with already authenticated user
        /// </summary>
        /// <typeparam name="TContext">Type of the context</typeparam>
        /// <returns>Created context</returns>
        TContext CreateContext<TContext>() where TContext : DbContext;

        /// <summary>
        /// Creates context with connection string for given user
        /// </summary>
        /// <typeparam name="TContext">Type of the context</typeparam>
        /// <returns>Created context</returns>
        TContext CreateContext<TContext>(IUser user) where TContext : DbContext;
    }
}
