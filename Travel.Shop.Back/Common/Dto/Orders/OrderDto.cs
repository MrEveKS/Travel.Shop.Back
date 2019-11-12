using System;

namespace Travel.Shop.Back.Common.Dto.Orders
{
    public class OrderDto : EntityDto
    {
        public decimal Cost { get; set; }

        public DateTime DateOf { get; set; }
    }
}
