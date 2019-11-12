using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Travel.Shop.Back.Common.Domain;
using Travel.Shop.Back.Common.Dto;
using Travel.Shop.Back.Common.Extension;
using Travel.Shop.Back.Data;

namespace Travel.Shop.Back.Controllers.BaseControllers
{
    /// <summary>
    /// Базовый контроллер, от которого желательно наследоваться всем
    /// </summary>
    /// <typeparam name="TEntity"> Тип сущности </typeparam>
    /// <typeparam name="TEntityDto">
    /// Тип dto сущности - класс, который будет
    /// передаваться в клиентскую часть
    /// </typeparam>
    /// <typeparam name="TListDto"> Тип списка элементов </typeparam>
    /// <typeparam name="TFilterDto">
    /// Тип фильтра, который используется для фильтрации элементов при формировании
    /// списка. Данный класс также используется для отправки агрегированных значений
    /// при поиске
    /// </typeparam>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public abstract class AbstractEntityController<TEntity, TEntityDto, TListDto, TFilterDto>
        : BaseController<TEntity>
        where TEntity : EntityBase, new()
        where TEntityDto : EntityDto, new()
        where TListDto : class
        where TFilterDto : class, new()
    {
        /// <inheritdoc />
        protected AbstractEntityController(IEntityManager entityManager,
            IMapper mapper) : base(entityManager, mapper)
        {
        }

        /// <summary>
        /// Метод сохранения сущности по входному dto
        /// </summary>
        /// <param name="dto"> dto для сохранения </param>
        /// <returns> Сохраненная сущность </returns>
        [NonAction]
        public abstract TEntity SaveDto(TEntityDto dto);

        /// <summary>
        /// Метод, который вызывается из клиента для сохранения сущности
        /// </summary>
        /// <param name="dto"> dto сущности для сохранения </param>
        /// <returns> dto сохраненной сущности </returns>
        [HttpPost]
        public virtual TEntityDto PostEntity([FromBody] TEntityDto dto)
        {
            var entity = SaveDto(dto);

            return MapToDto(entity);
        }

        /// <summary>
        /// Метод, который вызывается из клиента для обновления сущности
        /// </summary>
        /// <param name="dto"> dto сущности для сохранения </param>
        /// <returns> dto сохраненной сущности </returns>
        [HttpPut]
        public virtual TEntityDto PutEntity([FromBody] TEntityDto dto)
        {
            var entity = SaveDto(dto);

            return MapToDto(entity);
        }

        /// <summary>
        /// Метод, используемый из клиента для получения сущности по id
        /// </summary>
        /// <param name="id"> Идентификатор сущности </param>
        /// <returns> dto сущности </returns>
        [HttpGet]
        public virtual TEntityDto GetEntity(int? id)
        {
            var entity = id.IsEmpty() ? CreateEntity() : Load(id.Value);

            return MapToDto(entity);
        }

        /// <summary>
        /// Метод, вызываемый из клиента для удаления
        /// </summary>
        /// <param name="dto"> </param>
        [HttpDelete]
        public virtual bool PostDelete([FromQuery] EntityDto dto)
        {
            if (dto == null || dto.Id.IsEmpty())
            {
                throw new ArgumentNullException(nameof(dto));
            }

            Delete(Load(dto.Id.Value, false));

            return true;
        }

        /// <summary>
        /// Удаляет сущность
        /// </summary>
        /// <param name="entity"> </param>
        [NonAction]
        public virtual void Delete([FromBody] TEntity entity)
        {
            EntityManager.MarkRemove(entity);
            EntityManager.SaveChanges();
        }

        /// <summary>
        /// Метод создания сущности
        /// </summary>
        /// <returns> новая сущность данного типа </returns>
        [NonAction]
        public virtual TEntity CreateEntity()
        {
            return new TEntity();
        }

        /// <summary>
        ///  Метод получения списка сущностей, с учетом фильтрации
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual IEnumerable<TListDto> PostQuery([FromBody] TFilterDto filter)
        {
            var filtredQueryResult = FilteredQuery(filter);

            return Mapper.Map<IQueryable<TEntity>, IEnumerable<TListDto>>(filtredQueryResult);
        }

        /// <summary>
        /// Загружает сущность по id, используется в GetEntity и LoadOrCreate,
        /// если нужно чтоб сущность загружалась по особенному для этих методов,
        /// нужно переопределить
        /// </summary>
        /// <param name="id">ид сущности</param>
        /// <param name="isIncludeSingleLink">нужно ли загружать связанные единичные сущности</param>
        /// <param name="include">выражение, определяет как именно нужно загрузить сущность</param>
        /// <returns></returns>
        [NonAction]
        public virtual TEntity Load(int id, bool isIncludeSingleLink = true, Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> include = null)
        {
            return EntityManager.LoadOrThrow(id, isIncludeSingleLink, include);
        }

        /// <summary>
        /// Загружает сущность по dto, или создает новую, если у dto нет идентификатора
        /// </summary>
        /// <param name="dto">дто</param>
        /// <param name="isIncludeSingleLink">нужно ли загружать связанные единичные сущности</param>
        /// <param name="include">выражение, определяет как именно нужно загрузить сущность</param>
        /// <returns>найденная или созданная сущность</returns>
        [NonAction]
        public virtual TEntity LoadOrCreate(EntityDto dto, bool isIncludeSingleLink = true, Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> include = null)
        {
            if (dto == null)
            {
                return null;
            }

            if (!dto.Id.IsEmpty())
            {
                return Load(dto.Id.Value, isIncludeSingleLink, include);
            }

            var entity = new TEntity();

            Mapper.Map(dto, entity);

            EntityManager.DbInsert(entity);

            EntityManager.SaveChanges();

            return entity;
        }

        protected abstract IQueryable<TEntity> FilteredQuery(TFilterDto filterDto);

        [NonAction]
        protected virtual TEntityDto MapToDto(TEntity entity)
        {
            return MapTo<TEntityDto>(entity);
        }

    }
}
