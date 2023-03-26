// See https://aka.ms/new-console-template for more information




// Example expression

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using ExpressionDynamicTest.Parsing;
using ExpressionDynamicTest.Parsing.Models;
using Punica.Linq.Dynamic;

//string expression = "!(5 > 3 && 2 <= 4 || 1 != 1 ) && 5 > 3";
//string expression = "(5 > 3 && 2 <= 4 || 1 != 1 ) && 5 > 3";
bool val = (5 > 3 && 2 <= 4 || 1 != 1) && 5 > 3;

var parameters = new Parameters() { Description = "cumon rayheyiar" };
parameters.Status = new List<string>() { "New", "Old", "InProgress" };
parameters.Ids = new int[] { 5, 4, 7, 8, 2, 1 };

var a = new MyHashtable();
a.hashtable.Add("hello", 89);
a.hashtable.Add("nice",100);

var person = new Person()
{
    FirstName = "P1",
    LastName = "P2",
    Childrens = new List<Child>()
    {
        new Child()
        {
            Name = "Child1",
            Gender = "Male"
        },
        new Child()
        {
            Name = "Child2",
            Gender = "Female"
        }
    },
    Account = new Account()
    {
        Balance = 5000,
        Name = "Premium"
    }
};

//MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
//var expression = Expression.PropertyOrField(Expression.Constant(parameters), nameof(Parameters.Description));
//var val = Expression.Constant("hey");
//var exp1 = Expression.Call(expression, containsMethod, val);




//var operands = Operands.Create(new Token("hey", TokenType.String), new Token("Description", TokenType.Parameter), null, Expression.Constant(parameters));



//var exp2 = Expression.Call(operands.Right, containsMethod, operands.Left);


MyMethod(parameters, person);

var sql = "new{Name,Id,Buyer.Name as BuyerName,Buyer.Email, Items.new{Id,ProductName as Name,UnitPrice}}";



Console.ReadLine();


static void MyMethod(Parameters paras, Person person1)
{
    //string expression = "'hey' in @Description";
    //var val = paras.Description == "hey";

    //string expression = "'hello' == 'hel' + 'lo'";
    //var val = paras.Description == "hey";

    //string expression = "8 in @Ids";
    //var val = paras.Status.Contains("Old");

    //string expression = "Childrens.any('DeMale' in Gender)";
    //Expression<Func<Person, bool>> val = p => p.Childrens.Any(c => c.Gender.Contains("DeMale"));

    // string expression = "new { Name , Id , Buyer.Name as BuyerName , Buyer.Email , Buyer.(new {Name , Email}) , Items.Select(new {Id,ProductName as Name,UnitPrice})}";
    //string expression = "new { FirstName , Account = new {Name + 's', Balance} , IsMale }";
    // string expression = "FirstName , Account = new {Name + 's', Balance} , IsMale ";
   
    //string expression = "Account = new {Account.Name + 's'  as 'Id', Account.Balance}"; //TODO add = support, also support levels like outer one could be in or assign value can be in
    string expression = "FirstName, Childrens.Select(new {Name , Gender}).ToList() as Kids";
    //string expression = "Account.Bind(new {Name + 's', Balance})";
    //string expression = "new {Name + 's' as 'Id', Balance}";
    // string expression = "new { FirstName , LastName , Account.Name , Account.Balance}";
    // string expression = " FirstName , Account.Bind(Name + 's', Balance) , IsMale ";
    Expression<Func<Person, bool>> val = p => p.Childrens.Any(c => c.Gender.Contains("DeMale"));
    

    // Evaluate the expression and print the result

    Evaluator evaluator = new Evaluator(typeof(Person), Expression.Constant(paras));
    var expression1 = TextParser.Evaluate(expression, evaluator)[0];
    Console.WriteLine(expression1); // False

    var func = evaluator.GetFilterExpression<Person, object>(expression1).Compile();
    Console.WriteLine(func(person1) + "  " + val.Compile()(person1));

}

public class Parameters
{
    public List<string> Status { get; set; }
    public int[] Ids { get; set; }
    public string Description { get; set; }
}