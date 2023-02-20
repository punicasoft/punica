namespace Punica.Bp.Core
{
    public class UserContext : IUserContext
    {
        public Guid UserId { get; } = Guid.NewGuid();
    }
}
