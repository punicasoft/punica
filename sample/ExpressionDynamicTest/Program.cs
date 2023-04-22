// See https://aka.ms/new-console-template for more information




// Example expression

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using ExpressionDynamicTest;
using ExpressionDynamicTest.Parsing;
using ExpressionDynamicTest.Parsing.Models;
using Punica.Linq.Dynamic.Old;
using Punica.Linq.Dynamic.RD;
using Punica.Reflection;

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
    Children = new List<Child>()
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

Person magnus = new Person { FirstName = "Magnus" };
Person terry = new Person { FirstName = "Terry" };
Person charlotte = new Person { FirstName = "Charlotte" };

Pet barley = new Pet { Name = "Barley", Owner = terry };
Pet boots = new Pet { Name = "Boots", Owner = terry };
Pet whiskers = new Pet { Name = "Whiskers", Owner = charlotte };
Pet daisy = new Pet { Name = "Daisy", Owner = magnus };

List<Person> people = new List<Person> { magnus, terry, charlotte };
List<Pet> pets = new List<Pet> { barley, boots, whiskers, daisy };

// Create a list where each element is an anonymous
// type that contains a person's name and
// a collection of names of the pets they own.
var expected = people.GroupJoin(pets,
    p => p,
    p => p.Owner,
    (p, petCollection) =>
        new Groued
        {
            Owner = person.FirstName,
            Pets = petCollection.Select(pet => pet.Name).ToList(),
        });





MethodInfo methodInfo = EnumerableCachedMethodInfo.GroupJoin(typeof(List<Person>),typeof(List<Pet>), typeof(Person),typeof(Groued)).GetGenericMethodDefinition();


List<Type> providedTypes = new List<Type> { typeof(List<Person>), typeof(List<Pet>) };

MethodFinder.Instance.Print();

//ArgData.GetArgData(methodInfo, providedTypes);

//var resolver = MethodFinder.Instance.GetArgData(methodInfo);

//Expression<Func<Person, Person>> exp1 = p => p;
//Expression<Func<Pet, Person>> exp2 = p => p.Owner;
//Expression<Func<Person, IEnumerable<Pet>, Groued>> exp3 = (p, pt) => new Groued { Owner = p.FirstName, Pets = pt.Select(pet => pet.Name).ToList(), };

//List<Expression> arg = new List<Expression> { Expression.Constant(people), Expression.Constant(pets), exp1, exp2, exp3 };

//for (int i = 0; i < resolver.FuncCount; i++)
//{
//    var types = resolver.LambdasTypes(arg.ToArray(), i);

//    foreach (var type in types)
//    {
//        Console.WriteLine(type.Name);
//    }
//    Console.WriteLine();
//}


Console.ReadLine();



public class Groued
{
    public string Owner { get; set; }
    public List<string> Pets { get; set; }
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

public class Pet
{
    public string Name { get; set; }
    public Person Owner { get; set; }
}


public class Parameters
{
    public List<string> Status { get; set; }
    public int[] Ids { get; set; }
    public string Description { get; set; }
}