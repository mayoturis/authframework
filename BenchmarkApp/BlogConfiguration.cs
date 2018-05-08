using System.Data.Entity;
using System.Data.Entity.Core.Common;
using EFCache;
using MySql.Data.Entity;

namespace BenchmarkApp
{
    public class BlogConfiguration : DbConfiguration
    {
        public BlogConfiguration()
        {
            var transactionHandler = new CacheTransactionHandler(new InMemoryCache());

            AddInterceptor(transactionHandler);

            var cachingPolicy = new CachingPolicy();

            Loaded +=
                (sender, args) => args.ReplaceService<DbProviderServices>(
                    (s, _) => new CachingProviderServices(s, transactionHandler, cachingPolicy));
        }
    }
}
