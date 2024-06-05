using AutoMapper;
using DbContext;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Domain.Base;
using SWD.SheritonHotel.Domain.Entities;
using System.Linq.Expressions;

namespace SWD.SheritonHotel.Data.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public BaseRepository(ApplicationDbContext context, IMapper mapper) : this(context)
        {
            _context = context;
            _mapper = mapper;
        }
        protected DbSet<TEntity> DbSet
        {
            get
            {
                var dbSet = GetDbSet<TEntity>();
                return dbSet;
            }
        }
        #region Add(TEntity) + AddRange(IEnumerable<TEntity>)
        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            if (entities.Any())
            {
                DbSet.AddRange(entities);
            }
        }
        #endregion
        #region Check(int) + CheckCancellationToken(CancellationToken)
        public async Task<bool> Check(int id)
        {
            return await DbSet.AnyAsync(t => t.Equals(id));
        }

        public void CheckCancellationToken(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException("Request was cancelled");
        }
        #endregion
        #region Delete(TEntity) + DeleteRange(IEnumerable<TEntity>)
        public void Delete(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
        }
        #endregion
        #region Update(TEntity) + UpdateRange(IEnumerable<TEntity>)
        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            if (entities.Any())
            {
                DbSet.UpdateRange(entities);
            }
        }
        #endregion
        #region Other
        protected DbSet<T> GetDbSet<T>() where T : class
        {
            var dbSet = _context.Set<T>();
            return dbSet;
        }
        #endregion
        #region GetAll(CancellationToken)

        public Task<IList<TEntity>> GetAll(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region GetTotalCount()
        public Task<long> GetTotalCount()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region GetById(int) + GetByIds(List<int>)
        public virtual async Task<TEntity> GetById(int id)
        {
            var queryable = GetQueryable(x => x.Id == id);
            var entity = await queryable.FirstOrDefaultAsync();

            return entity;
        }

        public virtual async Task<List<TEntity>> GetByIds(IList<int> ids)
        {
            var queryable = GetQueryable(x => ids.Contains(x.Id));
            var entity = await queryable.ToListAsync();

            return entity;
        }
        #endregion
        #region GetQueryable(CancellationToken) + GetQueryable() + GetQueryable(Expression<Func<TEntity, bool>>)

        public IQueryable<TEntity> GetQueryable(CancellationToken cancellationToken = default)
        {
            CheckCancellationToken(cancellationToken);
            IQueryable<TEntity> queryable = GetQueryable<TEntity>();
            return queryable;
        }
        public IQueryable<T> GetQueryable<T>()
            where T : BaseEntity
        {
            IQueryable<T> queryable = GetDbSet<T>(); // like DbSet in this
            return queryable;

        }
        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> queryable = GetQueryable<TEntity>();
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }
            return queryable;
        }
        #endregion
    }
}
