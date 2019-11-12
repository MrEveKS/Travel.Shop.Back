using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Travel.Shop.Back.Common.Domain.Managers;
using Travel.Shop.Back.Common.Domain.Orders;
using Travel.Shop.Back.Common.Domain.Tourists;
using Travel.Shop.Back.Common.Domain.Tours;

namespace Travel.Shop.Back.Data
{
    /// <summary>
    /// Базовый контекст Базы данных
    /// </summary>
    public class BaseDbContext : IdentityDbContext<Manager>
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options)
        {
        }

        #region DbSets

        public DbSet<Tourist> Tourists { get; set; }

        public DbSet<Tour> Tours { get; set; }

        public DbSet<Order> Orders { get; set; }

        #endregion

    }
}
