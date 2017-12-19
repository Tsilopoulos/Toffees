
namespace Toffees.Glucose.Data.Contracts
{
    public interface IEntity<TKey>
    {
        TKey Identity { get; }
    }
}
