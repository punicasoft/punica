//using System.Linq.Expressions;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using Punica.Bp.Core;
//using Punica.Bp.Ddd.Domain.Entities;

//namespace Punica.Bp.Auditing.Tests.Infrastructure
//{
//    public class PropertySetterTests
//    {
//        [Fact]
//        public void SetProperty_Should_Work()
//        {
//            var setter = new PropertySetter(new UserContext(Guid.NewGuid()));

//            using (var context = new TestDb())
//            {
//                var entity = new TestAudit();
//                context.Attach(entity);
//                var entry = context.Entry(entity);
//                setter.SetProperty(entity, new InternalSetter(entry));
//            }
           
//        }
//    }


//    public class User : IEntity<Guid>
//    {
//        public Guid Id { get; set; }
//    }

//    public class TestAudit : ICreatedBy<User>, ICreatedDate, IEntity<Guid>
//    {
//        public User CreatedBy { get; private set; } = null!;
//        public DateTime CreatedOn { get;  set; }
//        public Guid Id { get; set; }
//    }

//    public class UserContext : IUserContext
//    {
//        public Guid UserId { get; }

//        public UserContext(Guid userId)
//        {
//            UserId = userId;
//        }
//    }

//    public class InternalSetter : IEntrySetter
//    {
//        private readonly EntityEntry entry;

//        public InternalSetter(EntityEntry entry)
//        {
//            this.entry = entry;
//        }

//        public void SetProperty<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, TProperty value) where TEntity : class 
//        {
//            EntityEntry<TEntity> entityEntry = entry.Context.Entry((TEntity)entry.Entity);

//            entityEntry.Property(propertyExpression).CurrentValue = value;
//        }

//        public void SetNavigation(string propertyName, object? value)
//        {
//            //entry.Property(propertyName).CurrentValue = value;
//            entry.Navigation(propertyName).CurrentValue = value;
//        }
//    }

//    public class TestDb : DbContext
//    {
//        public TestDb(): base(new DbContextOptionsBuilder<TestDb>()
//        .UseInMemoryDatabase(Guid.NewGuid().ToString())
//            .Options)
//        { }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);
//        }

//        public DbSet<TestAudit> Audit { get; set; }
//    }
//}
