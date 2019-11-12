using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Travel.Shop.Back.Common.Domain.Managers;
using Travel.Shop.Back.Common.Dto.Managers;
using Travel.Shop.Back.Services;

namespace Travel.Shop.Back.Controllers
{
    /// <summary>
    /// Контроллер генерации токена
    /// </summary>
    [ApiController, Route("api/[controller]/[action]")]
    public class TokenController : Controller
    {
        private readonly UserManager<Manager> _userManager;

        private readonly IAutorizationService _autorizationService;

        public TokenController(UserManager<Manager> userManager,
            IAutorizationService autorizationService)
        {
            _userManager = userManager;
            _autorizationService = autorizationService;
        }

        /// <summary>
        /// Получаем токен авторизации
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<string>> RequestToken([FromForm]LoginManagerDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = await _userManager.FindByNameAsync(model.Name);

            if (manager == null || !await _userManager.CheckPasswordAsync(manager, model.Password))
            {
                return Unauthorized();
            }

            return Ok(await _autorizationService.GetToken(manager, model.RememberMe.HasValue && model.RememberMe.Value));
        }
    }
}
