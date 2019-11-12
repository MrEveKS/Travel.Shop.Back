using AutoMapper;
using Travel.Shop.Back.Common.Domain.Orders;
using Travel.Shop.Back.Common.Domain.Tourists;
using Travel.Shop.Back.Common.Domain.Tours;
using Travel.Shop.Back.Common.Dto.Orders;
using Travel.Shop.Back.Common.Dto.Tourists;
using Travel.Shop.Back.Common.Dto.Tours;

namespace Travel.Shop.Back.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TouristDto, TouristListDto>();
            CreateMap<TouristDto, Tourist>();
            CreateMap<Tourist, TouristDto>();

            CreateMap<TourDto, TourListDto>();
            CreateMap<TourDto, Tour>();
            CreateMap<Tour, TourDto>();

            CreateMap<OrderDto, OrderListDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<Order, OrderDto>();
        }
    }
}
