using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Travel.Shop.Back.Common.Domain;

namespace Travel.Shop.Back.Data
{
    public class EntityManager : IEntityManager
    {
        private DataScope DataScope { get; }

        private BaseDbContext DbContext => DataScope.DbContext;

        private readonly Dictionary<Type, object> _dictionary = new Dictionary<Type, object>();

        public EntityManager(DataScope dataScope)
        {
            DataScope = dataScope;
        }

        public virtual DbSet<T> GetDbSet<T>()
            where T : SqlDbEntity
        {
            _dictionary.TryGetValue(typeof(T), out var set);

            if (set != null)
            {
                return set as DbSet<T>;
            }

            set = DbContext.Set<T>();
            _dictionary.Add(typeof(T), set);

            return (DbSet<T>)set;
        }

        #region Load

        /// <inheritdoc />
        public virtual T LoadOrThrow<T>(int id, bool isIncludeSingleLink, Expression<Func<IQueryable<T>, IQueryable<T>>> include = null)
            where T : EntityBase, new()
        {
            var query = GetSingleLoadQueryable(isIncludeSingleLink, include);

            return query.FirstOrDefault(x => x.Id == id)
                    ?? throw new Exception($"Entity [{typeof(T).Name}] with [{id}] not found");
        }

        private IQueryable<T> GetSingleLoadQueryable<T>(bool isIncludeSingleLink, Expression<Func<IQueryable<T>, IQueryable<T>>> include)
            where T : SqlDbEntity
        {
            var query = Query<T>(true, false);

            if (isIncludeSingleLink)
            {
                foreach (var propertyInfo in typeof(T).GetProperties().Where(x => typeof(SqlDbEntity).IsAssignableFrom(x.PropertyType)))
                {
                    query = query.Include(propertyInfo.Name);

                    if (!typeof(SqlDbEntity).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        continue;
                    }

                    query = propertyInfo.PropertyType.GetProperties()
                        .Where(x => typeof(SqlDbEntity).IsAssignableFrom(x.PropertyType))
                        .Aggregate(query, (current, children) => current.Include($"{propertyInfo.Name}.{children.Name}"));
                }
            }

            if (include != null)
            {
                query = include.Compile()(query);
            }

            return query;
        }

        #endregion

        /// <inheritdoc />
        public virtual IQueryable<T> Query<T>(bool asTracking = true, bool actual = true)
            where T : SqlDbEntity
        {
            var query = asTracking ? GetDbSet<T>().AsTracking() : GetDbSet<T>().AsNoTracking();

            return query;
        }

        /// <inheritdoc />
        public virtual void DbInsert<T>(T entity)
            where T : SqlDbEntity
        {
            MarkInsert(entity);
            DbContext.SaveChanges();
        }

        /// <inheritdoc />
        public virtual void MarkInsert<T>(T entity)
            where T : SqlDbEntity
        {
            GetDbSet<T>().Add(entity);
        }

        /// <inheritdoc />
        public virtual void MarkRemove<T>(T entity)
            where T : SqlDbEntity
        {
            GetDbSet<T>().Remove(entity);
        }

        /// <inheritdoc />
        public virtual int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

    }
}
