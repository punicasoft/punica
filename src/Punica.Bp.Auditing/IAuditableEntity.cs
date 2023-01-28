namespace Punica.Bp.Auditing
{
    public interface IAuditableEntity : 
        ICreationAuditableEntity, 
        IModificationAuditableEntity,
        IDeletionAuditableEntity
    {
        
    }
}
