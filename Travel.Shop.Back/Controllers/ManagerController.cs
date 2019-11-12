using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Travel.Shop.Back.Common.Domain.Managers;
using Travel.Shop.Back.Common.Dto.Managers;
using Travel.Shop.Back.Services;

namespace Travel.Shop.Back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ManagerController : Controller
    {
        private readonly UserManager<Manager> _userManager;

        public ManagerController(UserManager<Manager> userManager)
        {
            _userManager = userManager;
        }

        // POST api/accounts/register
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterManagerDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = new Manager
            {
                UserName = model.Name ?? model.Email,
                Email = model.Email,
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return new OkResult();
        }
    }
}
