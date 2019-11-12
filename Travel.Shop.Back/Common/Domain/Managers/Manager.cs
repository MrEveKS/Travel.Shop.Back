using Microsoft.AspNetCore.Identity;

namespace Travel.Shop.Back.Common.Domain.Managers
{
    public class Manager : IdentityUser
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}
