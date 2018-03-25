using System;
using System.ComponentModel.DataAnnotations.Schema;
using Toffees.Glucose.Data.Contracts;

namespace Toffees.Glucose.Data.Entities
{
    public class Glucose : IGlucose
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Data { get; set; }
        public DateTime PinchDateTime { get; set; }
        public string Tag { get; set; }
        public string UserId { get; set; }
        public int Identity => Id;
    }
}