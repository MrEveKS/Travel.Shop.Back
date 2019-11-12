using System.ComponentModel.DataAnnotations.Schema;

namespace Travel.Shop.Back.Common.Domain.Tours
{
    public class Tour : EntityBase
    {
        public string Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }
    }
}
