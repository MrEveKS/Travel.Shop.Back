using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Travel.Shop.Back.Common.Domain.Tourists;
using Travel.Shop.Back.Common.Dto.Tourists;
using Travel.Shop.Back.Controllers.BaseControllers;
using Travel.Shop.Back.Data;

namespace Travel.Shop.Back.Controllers
{
    [Authorize]
    public class TouristController : AbstractEntityController<Tourist, TouristDto, TouristListDto, TouristFilterDto>
    {
        public TouristController(IEntityManager entityManager, IMapper mapper) 
            : base(entityManager, mapper)
        {
        }

        public override Tourist SaveDto(TouristDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var entity = LoadOrCreate(dto);

            return entity;
        }

        protected override IQueryable<Tourist> FilteredQuery(TouristFilterDto filterDto)
        {
            var result = EntityManager.Query<Tourist>();

            if (filterDto == null)
            {
                return result;
            }

            if (filterDto.QueryName != null)
            {
                result = result.Where(e => e.FirstName.Contains(filterDto.QueryName) 
                || e.LastName.Contains(filterDto.QueryName)
                || e.MiddleName.Contains(filterDto.QueryName));
            }

            if (filterDto.Id != null)
            {
                result = result.Where(e => e.Id == filterDto.Id);
            }

            return result;
        }
    }
}
