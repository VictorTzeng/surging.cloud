using Nest;
using Surging.Core.CPlatform.Runtime.Session;
using Surging.Core.Domain.Entities;
using Surging.Core.Domain.Entities.Auditing;
using Surging.Core.ElasticSearch;
using System;

namespace Surging.Core.Dapper.Filters.Action
{
    public class ModificationAuditDapperActionFilter<TEntity, TPrimaryKey> : DapperActionFilterBase, IAuditActionFilter<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        public void ExecuteFilter(TEntity entity)
        {
            var loginUser = NullSurgingSession.Instance;
            if (typeof(IModificationAudited).IsAssignableFrom(typeof(TEntity)) && loginUser != null)
            {

                var record = entity as IModificationAudited;
                record.LastModifierUserId = loginUser.UserId;
                record.LastModificationTime = DateTime.Now;

            }
            //if (typeof(IElasticSearch).IsAssignableFrom(typeof(TEntity)))
            //{
            //    ((IElasticSearch)entity).Version = ((IElasticSearch)entity).Version + 1;
            //}
        }
    }
}
