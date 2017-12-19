using System;

namespace Toffees.Glucose.Data.Contracts
{
    public interface IGlucose : IEntity<int>
    {
        int Id { get; set; }

        int Data { get; set; }

        DateTime PinchDateTime { get; set; }

        string Tag { get; set; }

        string UserId { get; set; }
    }
}
