using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Travel.Shop.Back.Common.Domain.Managers;
using Travel.Shop.Back.Common.Domain.Tourists;
using Travel.Shop.Back.Common.Domain.Tours;

namespace Travel.Shop.Back.Common.Domain.Orders
{
    public class Order : EntityBase
    {
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }

        public DateTime DateOf { get; set; }

        public IList<Tourist> Tourists { get; set; }

        public int TourId { get; set; }

        public Tour Tour { get; set; }

        public int ManagerId { get; set; }

        public Manager Manager { get; set; }
    }
}
