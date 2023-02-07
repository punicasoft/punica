using Microsoft.Extensions.DependencyInjection;

namespace Punica.Bp.CQRS.Extensions.DependencyInjection
{
    public class MediatorOptions
    {
        public Type ImplementationType { get; private set; }
        public ServiceLifetime Lifetime { get; private set; }

        public MediatorOptions()
        {
            ImplementationType = typeof(Mediator);
            Lifetime = ServiceLifetime.Transient;
        }

        public void Use<TMediator>() where TMediator : IMediator
        {
            ImplementationType = typeof(TMediator);
        }

        public void UseScope(ServiceLifetime lifetime) 
        {
            Lifetime = lifetime;
        }
    }
}
