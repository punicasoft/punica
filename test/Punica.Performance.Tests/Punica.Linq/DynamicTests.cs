using BenchmarkDotNet.Attributes;
using Punica.Linq.Dynamic;

namespace Punica.Performance.Tests.Punica.Linq
{
    [MemoryDiagnoser]
    public class DynamicTests
    {
        //13ms
        [Benchmark]
        public void Evaluvate()
        {
            string stringExp = "this.Select( new { FirstName , Children.Select(new {Name , Gender}).ToList() as Kids} )";
            Evaluator evaluator = new Evaluator(typeof(IQueryable<Person>), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<IQueryable<Person>, object>(expression1[0]);
        }

    }

    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public DateTime Dob { get; set; }
        public int NoOfChildren { get; set; }
        public bool IsMarried { get; set; }
        public bool IsMale { get; set; }
        public Account Account { get; set; }

        public List<Child> Children { get; set; }
    }

    public class Account
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }

    public class Child
    {
        public string Name { get; set; }

        public string Gender { get; set; }
    }

}
