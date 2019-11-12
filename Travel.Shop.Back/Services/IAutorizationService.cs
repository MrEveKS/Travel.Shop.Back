using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Travel.Shop.Back.Common.Domain.Managers;

namespace Travel.Shop.Back.Services
{
    public interface IAutorizationService
    {
        Task<string> GetToken(Manager manager, bool rememberMe);
    }
}