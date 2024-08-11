using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using Models.Entities.Shared;

namespace DataAccess
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly eShopContext _context;

        private DbSet<T> entities;

        public GenericRepository(eShopContext context)
        {
            this._context = context;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            entities = _context.Set<T>();
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            entities.Update(entity);
            //context.SaveChanges();
        }

        public List<T> FindAllByCondition(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AsNoTracking().Where(predicate).ToList();
        }

        public IQueryable<T> FindAllByConditionWithIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = entities.AsNoTracking().Where(predicate);
            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public Tuple<List<T>, int> FindAllByConditionWithIncludesAndPagination(int skip, int take, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                var records = entities.Where(predicate);
                int totalCount = records == null ? 0 : records.Count();
                var query = records.OrderByDescending(x => x.ID).Skip(skip).Take(take).AsNoTracking();
                return Tuple.Create(includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).ToList(), totalCount);
            }
            catch (Exception)
            {
                return Tuple.Create(new List<T>(), 0);
            }

        }

        public IQueryable<T> FindAllWithIncludes(params Expression<Func<T, object>>[] includes)
        {
            var query = entities.AsQueryable();
            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public T FindByCondition(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AsNoTracking().SingleOrDefault(predicate);
        }

        public T FindByConditionWithIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = entities.AsNoTracking().Where(predicate);
            var result = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (!result.Any())
                return null;

            return result.First();
        }

        public bool FindIsExistByCondition(Expression<Func<T, bool>> predicate)
        {
            return entities.AsNoTracking().Any(predicate);
        }

        public List<T> GetAll()
        {
            return entities.AsNoTracking().Where(x => x.IsDeleted == false).ToList();
        }

        public T Insert(T entity)
        {
            entities.Add(entity);
            //context.SaveChanges();

            return entity;
        }

        public void Update(T entity)
        {

            entities.Update(entity);
            //context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
