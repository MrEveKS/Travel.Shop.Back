using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Travel.Shop.Back.Common.Domain;
using Travel.Shop.Back.Data;

namespace Travel.Shop.Back.Controllers.BaseControllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class BaseController<TEntity> : Controller
        where TEntity : SqlDbEntity, new()
    {
        /// <summary>
        /// Менеджер для доступа к данным
        /// </summary>
        public virtual IEntityManager EntityManager { get; }

        /// <summary>
        /// Automapper
        /// </summary>
        public virtual IMapper Mapper { get; }

        public BaseController(IEntityManager entityManager,
            IMapper mapper)
        {
            EntityManager = entityManager;

            Mapper = mapper;
        }

        [NonAction]
        public virtual IQueryable<TEntity> Query(bool asTracking = true, bool actual = true)
        {
            return EntityManager.Query<TEntity>(asTracking, actual);
        }

        [NonAction]
        public virtual TDest MapTo<TDest>(TEntity entity)
        {
            return Mapper.Map<TDest>(entity);
        }

    }
}
