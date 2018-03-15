using System;

namespace Toffees.Glucose.Models.DTOs
{
    public class GlucoseDto
    {
        public int Id { get; set; }

        public int Data { get; set; }

        public DateTime PinchDateTime { get; set; }

        public string Tag { get; set; }
    }
}
