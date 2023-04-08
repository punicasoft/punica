using System.Linq.Expressions;
using ExpressionDynamicTest.Parsing.Models;

namespace ExpressionDynamicTest;

public class Test
{
    public void Test1()
    {
        Expression<Func<Person, object>> exp = p => new
        {
            Name1 = p.FirstName,
            Name2 = p.LastName
        };

    }

    public void Test2()
    {
        Expression<Func<Person, dynamic>> exp = p => new
        {
            Name1 = p.FirstName,
            Id = p.Id,
        };
    }

    public void Test3()
    {
        var status = new string[] { "New", "Active", "Hello" };
        Expression<Func<bool>> exp = () => status.Contains("New");
    }

    public void Teste3()
    {
        var status = new string[] { "New", "Active", "Hello" };
        Expression<Func<bool>> exp = () => status.Contains("New");
    }

    public void Test4()
    {
        var r = "Hello new world".Contains("hey");

        Expression<Func<bool>> exp = () => "Hello new world".Contains("New");
    }
    public void Test5()
    {
        //ParameterExpression left = Expression.Parameter(typeof(string), "st");
        //Expression.Lambda<Func<string, string>>((Expression)Expression.Add((Expression)left, (Expression)Expression.Constant((object)"hey", typeof(string)), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(string.Concat))), new ParameterExpression[1]
        //{
        //    left
        //});
        Expression<Func<string,string>> e = (st) => st + "hey";
    }

    public void Test6()
    {
        Expression<Func<double>> exp = () => -5.6 / 7.9;

        Expression<Func<double>> exp2 = () => 5.6 / - 7.9;
    }
}