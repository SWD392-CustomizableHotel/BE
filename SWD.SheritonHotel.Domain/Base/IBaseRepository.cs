using SWD.SheritonHotel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Base
{
    public interface IBaseRepository
    {

    }
    public interface IBaseRepository<TEntity> : IBaseRepository where TEntity : BaseEntity
    {
        Task<bool> Check(int id);
        Task<IList<TEntity>> GetAll(CancellationToken cancellationToken = default);
        Task<long> GetTotalCount();
        Task<TEntity> GetById(int id);
        Task<List<TEntity>> GetByIds(IList<int> ids);
        IQueryable<TEntity> GetQueryable(CancellationToken cancellationToken = default);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        void CheckCancellationToken(CancellationToken cancellationToken = default);
    }
}
