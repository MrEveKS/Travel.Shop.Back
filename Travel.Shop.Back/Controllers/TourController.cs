using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using Travel.Shop.Back.Common.Domain.Tours;
using Travel.Shop.Back.Common.Dto.Tours;
using Travel.Shop.Back.Controllers.BaseControllers;
using Travel.Shop.Back.Data;

namespace Travel.Shop.Back.Controllers
{
    [Authorize]
    public class TourController : AbstractEntityController<Tour, TourDto, TourListDto, TourFilterDto>
    {
        public TourController(IEntityManager entityManager, IMapper mapper)
            : base(entityManager, mapper)
        {
        }

        public override Tour SaveDto(TourDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var entity = LoadOrCreate(dto);

            return entity;
        }

        protected override IQueryable<Tour> FilteredQuery(TourFilterDto filterDto)
        {
            var result = EntityManager.Query<Tour>();

            if (filterDto == null)
            {
                return result;
            }

            if (filterDto.QueryName != null)
            {
                result = result.Where(e => e.Name.Contains(filterDto.QueryName));
            }

            if (filterDto.Id != null)
            {
                result = result.Where(e => e.Id == filterDto.Id);
            }

            return result;
        }
    }
}
