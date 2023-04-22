using BenchmarkDotNet.Attributes;
using Punica.Linq.Dynamic.Old;
using Punica.Linq.Dynamic.RD;
using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Rd2;
using System.Reflection;

namespace Punica.Performance.Tests.Punica.Linq
{
    [MemoryDiagnoser]
    public class DynamicTests
    {
        private List<MyClass> c;

        [GlobalSetup]
        public void GlobalSetup()
        {
            c = new List<MyClass>()
            {
                new MyClass()
                {
                    x= 10,
                },
                new MyClass()
                {
                    x= 30,
                },
                new MyClass()
                {
                    x= 4,
                }
            };
        }

        [Benchmark]
        public void Test()
        {
            var type = typeof(Enumerable);
            var _methods = new Dictionary<string, MethodInfo>();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                var key = $"{type.FullName}.{method.Name}.{method.GetParameters().Length}";
                if (!_methods.ContainsKey(key))
                {
                    _methods.Add(key, method);
                }
            }
        }

        //13ms
        [Benchmark]
        public void SelectInsideSelect_Evaluator()
        {
            string stringExp = "this.Select( new { FirstName , Children.Select(new {Name , Gender}).ToList() as Kids} )";
            Evaluator evaluator = new Evaluator(typeof(IQueryable<Person>), null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<IQueryable<Person>, object>(expression1[0]);
        }

        [Benchmark]
        public void SelectInsideSelect_Tokenizer2()
        {
            string stringExp = "Select( new { FirstName , Children.Select(new {Name , Gender}).ToList() as 'Kids'} )";

            var rootToken = Tokenizer2.Evaluate(new TokenContext(stringExp, new MethodContext(Expression.Parameter(typeof(IQueryable<Person>), "arg"))));
            var resultExpression = rootToken.Evaluate();
        }

        [Benchmark]
        public void SelectInsideSelect_Tokenizer3()
        {
            string stringExp = "Select( new { FirstName , Children.Select(new {Name , Gender}).ToList() as 'Kids'} )";

            var context = new TokenContext3(stringExp);
            context.AddStartParameter(typeof(IQueryable<Person>));

            var rootToken = Tokenizer3.Evaluate(context);
            var resultExpression = rootToken.Evaluate();
        }


        [Benchmark]
        public void PrimitiveOperators_Evaluator()
        {
            string stringExp = $"(5 > 3 && 2 <= 4 || 1 != 1 ) && 2 + 4 > 3 && 's' in 'cro' + 's'";
            Evaluator evaluator = new Evaluator((Type)null, null);
            var expression1 = TextParser.Evaluate(stringExp, evaluator);
            var resultExpression = evaluator.GetFilterExpression<bool>(expression1[0]);
        }


        [Benchmark]
        public void PrimitiveOperators_Tokenizer2()
        {
            string stringExp = $"(5 > 3 && 2 <= 4 || 1 != 1 ) && 2 + 4 > 3 && 's' in 'cro' + 's'";

            var rootToken = Tokenizer2.Evaluate(new TokenContext(stringExp));
            var resultExpression = rootToken.Evaluate();
        }

        [Benchmark]
        public void PrimitiveOperators_Tokenizer3()
        {
            string stringExp = $"(5 > 3 && 2 <= 4 || 1 != 1 ) && 2 + 4 > 3 && 's' in 'cro' + 's'";

            var rootToken = Tokenizer3.Evaluate(new TokenContext3(stringExp));
            var resultExpression = rootToken.Evaluate();
        }


        [Benchmark]
        public void Average_Tokenizer2()
        {
            string stringExp = $"Average(x)";

            var rootToken = Tokenizer2.Evaluate(new TokenContext(stringExp, new MethodContext(Expression.Parameter(typeof(List<MyClass>), "arg"))));
            var resultExpression = (LambdaExpression)rootToken.Evaluate();
            //resultExpression.Compile().DynamicInvoke(c);
        }


        [Benchmark]
        public void Average_Tokenizer3()
        {
            string stringExp = $"Average(x)";

            //var rootToken = Tokenizer3.Evaluate(new TokenContext3(stringExp, new MethodContext(Expression.Parameter(typeof(List<MyClass>), "arg"))));
            //var resultExpression = (LambdaExpression)rootToken.Evaluate();
            //resultExpression.Compile().DynamicInvoke(c);

            var context = new TokenContext3(stringExp);
            context.AddStartParameter(typeof(List<MyClass>));
            var rootToken = Tokenizer3.Evaluate(context);
            var resultExpression = (LambdaExpression)rootToken.Evaluate();
            resultExpression.Compile().DynamicInvoke(c);
        }

        class MyClass
        {
            public int x { get; set; }
        }

        //[Benchmark]
        //public void Evaluate_One_New()
        //{
        //    string stringExp = "this.Select( new { FirstName , LastName as Kids} )";
        //    Evaluator evaluator = new Evaluator(typeof(IQueryable<Person>), null);
        //    var expression1 = TextParser.Evaluate(stringExp, evaluator);
        //    var resultExpression = evaluator.GetFilterExpression<IQueryable<Person>, object>(expression1[0]);
        //}

        //[Benchmark]
        //public void Evaluate_Normal_Expression()
        //{
        //    string stringExp = $"(5 > 3 && 2 <= 4 || 1 != 1 ) && 2 + 4 > 3 && 's' in 'cro' + 's'";
        //    Evaluator evaluator = new Evaluator((Type)null, null);
        //    var expression1 = TextParser.Evaluate(stringExp, evaluator);
        //    var resultExpression = evaluator.GetFilterExpression<bool>(expression1[0]);
        //}

        //[Benchmark]
        //public void Tokenize_New()
        //{
        //    string stringExp = "this.Select( new { FirstName , Children.Select(new {Name , Gender}).ToList() as Kids} )";
        //    var tokens = Tokenizer.Tokenize(stringExp);
        //}

        //[Benchmark]
        //public void Tokenize_New2()
        //{
        //    string stringExp = "this.Select( new { FirstName , Children.Select(new {Name , Gender}).ToList() as Kids} )";
        //    var tokens = Tokenizer2.Tokenize(stringExp, null);
        //}

        //[Benchmark]
        //public void Tokenize_Old()
        //{
        //    string stringExp = "this.Select( new { FirstName , Children.Select(new {Name , Gender}).ToList() as Kids} )";
        //    var tokens = TextParser.Tokenize(stringExp);
        //}




        //[Benchmark]
        //public void Tokenize_New_Normal()
        //{
        //    string stringExp = "(5 > 3 && 2 <= 4 || 1 != 1 ) && 2 + 4 > 3 && 's' in 'cro' + 's'";
        //    var tokens = Tokenizer.Tokenize(stringExp);
        //}

        //[Benchmark]
        //public void Tokenize_New2_Normal()
        //{
        //    string stringExp = "(5 > 3 && 2 <= 4 || 1 != 1 ) && 2 + 4 > 3 && 's' in 'cro' + 's'";
        //    var tokens = Tokenizer2.Tokenize(new TokenContext(stringExp));
        //}

        //[Benchmark]
        //public void Tokenize_New2_Complex()
        //{
        //    var sql = "new{Id,FirstName as 'BuyerName', Account.Name, Account . Name , Children.Select(new{Name,Gender})}";

        //    var tokens = Tokenizer2.Tokenize(new TokenContext(sql, Expression.Parameter(typeof(Person), "arg")));
        //}

        //[Benchmark]
        //public void Tokenize_Old_Complex()
        //{
        //    string stringExp = "new{Id,FirstName as 'BuyerName', Account.Name, Account . Name , Childrens.Select(new{Name,Gender})}";
        //    var tokens = TextParser.Tokenize(stringExp);
        //}

        //[Benchmark]
        //public void Tokenize_Old_Normal()
        //{
        //    string stringExp = "(5 > 3 && 2 <= 4 || 1 != 1 ) && 2 + 4 > 3 && 's' in 'cro' + 's'";
        //    var tokens = TextParser.Tokenize(stringExp);
        //}



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
