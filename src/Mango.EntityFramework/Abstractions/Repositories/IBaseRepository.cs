using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.EntityFramework.Abstractions.Repositories
{
    public interface IBaseRepository<TEntity> : IKeyRepository<TEntity,object>
        where TEntity : class, IEntity, new()
    {

    }
}
