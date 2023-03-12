using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionDynamicTest.Parsing.Models;

namespace ExpressionDynamicTest;

public class Test3
{
    public void Method1()
    {
        Expression<Func<Person, object>> exp = p => new Dictionary<string, object>()
        {
            {"Id", p.Id},
            {"Name2", p.LastName}
        };
    }
    
    public object Method2()
    {
        ParameterExpression arg = Expression.Parameter(typeof(Person), "p");


        var expression = Expression.Lambda<Func<Person, object>>(
            Expression.ListInit(Expression.New(typeof(Dictionary<string, object>)),
                new ElementInit[]
                {
                    Expression.ElementInit(typeof(IDictionary<string, object>).GetMethod("Add", new Type[] { typeof(string), typeof(object) }), 
                        Expression.Constant("Id"), 
                        Expression.Property(arg, "Id")),
                    Expression.ElementInit(typeof(IDictionary<string, object>).GetMethod("Add", new Type[] { typeof(string), typeof(object) }), 
                        Expression.Constant("Name2"),
                        Expression.Convert(Expression.PropertyOrField(arg, "LastName"), typeof(object)))
                }), 
            arg);

        var func = expression.Compile();

        return func(new Person() { FirstName = "a", LastName = "b" ,Id = Guid.NewGuid()});
    }

    public void Method13(Func<bool> test, int ifTrue, int ifFalse)
    {
        Expression exp = () => test()? ifTrue : ifFalse;
    }

}