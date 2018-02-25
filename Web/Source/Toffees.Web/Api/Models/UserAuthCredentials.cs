using System.ComponentModel.DataAnnotations;

namespace Toffees.Web.Api.Models
{
    public class UserAuthCredentials
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
