using System;

namespace Toffees.Web.Api.Models.Glucose
{
    public class GlucoseDto
    {
        public int Id { get; set; }

        public int Data { get; set; }

        public DateTime PinchDateTime { get; set; }

        public string Tag { get; set; }
    }
}
