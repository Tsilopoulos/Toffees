using System.ComponentModel.DataAnnotations;

namespace Toffees.Web.Api.Models.Glucose
{
    public class GlucosePostRequest
    {
        [Required]
        public int Data { get; set; }

        [Required]
        public string Tag { get; set; }
    }
}
