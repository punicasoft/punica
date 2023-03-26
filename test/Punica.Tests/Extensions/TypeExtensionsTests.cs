using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Punica.Extensions;
using Punica.Tests.Utils;

namespace Punica.Tests.Extensions
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

            result = typeof(D).IsImplementedFrom(typeof(IC<B>));
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

        [Theory]
        [InlineData(false, typeof(int), null)]
        [InlineData(false, typeof(string), null)]
        [InlineData(true, typeof(int[]), typeof(int))]
        [InlineData(true, typeof(List<string>), typeof(string))]
        [InlineData(true, typeof(IList<string>), typeof(string))]
        [InlineData(true, typeof(ICollection<string>), typeof(string))]
        [InlineData(true, typeof(IEnumerable<string>), typeof(string))]
        [InlineData(true, typeof(IQueryable<object>), typeof(object))]
        public void IsCollectionWithOut_ShouldCheckForCollectionWIthTypes(bool expected, Type type, Type? expectedType)
        {
            var actual = type.IsCollection(out Type? actualType);
            Assert.Equal(expected, actual);
            Assert.Equal(expectedType, actualType);
        }
    }
}
