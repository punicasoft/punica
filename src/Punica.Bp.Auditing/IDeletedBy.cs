
namespace Punica.Bp.Auditing
{
    public interface IDeletedBy : ISoftDeletable
    {
        Guid DeletedBy { get; set; }
    }
}
