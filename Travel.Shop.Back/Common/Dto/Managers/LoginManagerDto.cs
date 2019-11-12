using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Travel.Shop.Back.Common.Dto.Managers
{
    public class LoginManagerDto
    {
        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DefaultValue(false)]
        public bool? RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
