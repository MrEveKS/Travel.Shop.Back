using System;

namespace Travel.Shop.Back.Common.Dto.Orders
{
    public class OrderListDto : EntityDto
    {
        public decimal Cost { get; set; }

        public DateTime DateOf { get; set; }
    }
}
