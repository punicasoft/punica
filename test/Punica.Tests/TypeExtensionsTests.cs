using Punica.Extensions;

namespace Punica.Tests
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void IsImplementedFrom_Should_Work()
        {
            bool result = false;
            result = typeof(A).IsImplementedFrom(typeof(IA));
            Assert.True(result);

            result = typeof(Aa).IsImplementedFrom(typeof(A));
            Assert.True(result);

            result = typeof(Aa).IsImplementedFrom(typeof(IA));
            Assert.True(result);

            result = typeof(B).IsImplementedFrom(typeof(IB<Guid>));
            Assert.True(result);

            result = typeof(B).IsImplementedFrom(typeof(IB<>));
            Assert.True(result);

            result = typeof(C).IsImplementedFrom(typeof(IC<IB<Guid>>));
            Assert.True(result);

            result = typeof(Ca).IsImplementedFrom(typeof(IC<Aa>));
            Assert.True(result);

            result = typeof(Ca).IsImplementedFrom(typeof(IC<A>));
            Assert.True(result);

            result = typeof(Ca).IsImplementedFrom(typeof(IC<IA>));
            Assert.True(result);

            result = typeof(Ca).IsImplementedFrom(typeof(IC<IB>));
            Assert.False(result);

            result = typeof(D).IsImplementedFrom(typeof(C));
            Assert.True(result);

            result = typeof(D).IsImplementedFrom(typeof(B));
            Assert.False(result);

            result = typeof(D).IsImplementedFrom(typeof(IC<>));
            Assert.True(result);

            result = typeof(D).IsImplementedFrom(typeof(IC<IB<Guid>>));
            Assert.True(result);

        }


        [Fact]
        public void GetImplementedType_Should_Work()
        {
            Type? result = null;

            result = typeof(A).GetImplementedType(typeof(IA));
            Assert.Null(result);

            result = typeof(B).GetImplementedType(typeof(IB<Guid>));
            Assert.Equal(typeof(Guid), result);

            result = typeof(B).GetImplementedType(typeof(IB<>));
            Assert.Equal(typeof(Guid), result);

            result = typeof(C).GetImplementedType(typeof(IC<IB<Guid>>));
            Assert.Equal(typeof(B), result);

        }


        internal interface IA
        {

        }

        internal interface IB
        {
        }

        internal interface IB<T> :IB
        {
            T Id { get; }
        }

        internal interface IC<T>
        {
            T Id { get; }
        }

        internal class A : IA
        {

        }
        internal class Aa : A
        {

        }

        internal class B : IB<Guid>
        {
            public Guid Id { get; }
        }

        internal class C : IC<B>
        {
            public B Id { get; }
        }

        internal class Ca : IC<Aa>
        {
            public Aa Id { get; }
        }

        internal class D : C
        {
        }
    }
}
