using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Travel.Shop.Back.Services
{
    public static class SecurityKeyService
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(this string key)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }
    }
}
