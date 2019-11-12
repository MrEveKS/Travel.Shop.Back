using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Travel.Shop.Back.Common.Domain.Orders;
using Travel.Shop.Back.Common.Dto.Orders;
using Travel.Shop.Back.Controllers.BaseControllers;
using Travel.Shop.Back.Data;

namespace Travel.Shop.Back.Controllers
{
    [Authorize]
    public class OrderController : AbstractEntityController<Order, OrderDto, OrderListDto, OrderFilterDto>
    {
        public OrderController(IEntityManager entityManager, IMapper mapper) 
            : base(entityManager, mapper)
        {
        }

        public override Order SaveDto(OrderDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var entity = LoadOrCreate(dto);

            return entity;
        }

        protected override IQueryable<Order> FilteredQuery(OrderFilterDto filterDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
