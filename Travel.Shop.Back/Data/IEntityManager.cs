using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Travel.Shop.Back.Common.Domain;

namespace Travel.Shop.Back.Data
{
    /// <summary>
    /// Различные методы работы с БД
    /// </summary>
    public interface IEntityManager
    {
        DbSet<T> GetDbSet<T>()
            where T : SqlDbEntity;

        /// <summary>
        /// Загрузить сущность по id или выбросить исключение если сущность не найдена
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="id">id сущности</param>
        /// <param name="isIncludeSingleLink">нужно ли загружать связанные единичные сущности</param>
        /// <param name="include">выражение, определяет как именно нужно загрузить сущность</param>
        /// <returns>найденная сущность</returns>
        T LoadOrThrow<T>(int id, bool isIncludeSingleLink, Expression<Func<IQueryable<T>, IQueryable<T>>> include = null)
            where T : EntityBase, new();

        /// <summary>
        /// Получить IQueryable для заданной сущности
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="asTracking">включить change tracker для результатов запроса</param>
        /// <param name="actual">брать только актуальные</param>
        /// <returns></returns>
        IQueryable<T> Query<T>(bool asTracking = true, bool actual = true)
            where T : SqlDbEntity;

        /// <summary>
        /// Вставить сущность
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="entity">сущность</param>
        void DbInsert<T>(T entity)
            where T : SqlDbEntity;

        /// <summary>
        /// Пометить сущность на вставку
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="entity">сущность</param>
        void MarkInsert<T>(T entity)
            where T : SqlDbEntity;

        /// <summary>
        /// Пометить сущность на удаление
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="entity">сущность</param>
        void MarkRemove<T>(T entity)
            where T : SqlDbEntity;

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

    }
}