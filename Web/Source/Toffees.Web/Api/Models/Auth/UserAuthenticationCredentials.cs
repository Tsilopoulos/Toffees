using System.ComponentModel.DataAnnotations;

namespace Toffees.Web.Api.Models.Auth
{
    public class UserAuthenticationCredentials
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
