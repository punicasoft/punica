namespace Punica.Bp.Auditing
{
    public interface IDeletedDate : ISoftDeletable
    {
        DateTime? DeletedOn { get; set; }
    }
}
