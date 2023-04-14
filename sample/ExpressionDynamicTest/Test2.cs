using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionDynamicTest;

public class Test2
{
    // public void Test1()
    // {
    //   ParameterExpression parameterExpression = Expression.Parameter(typeof (Person), "p");
    //
    //   var expressions = new ParameterExpression[1] { parameterExpression };
    //   var constructionInfo =(ConstructorInfo) MethodBase.GetMethodFromHandle(__methodref (<>f__AnonymousType0<string, string>..ctor), __typeref (<>f__AnonymousType0<string, string>));
    //
    //   var newExpression = Expression.New(constructionInfo, (IEnumerable<Expression>) new Expression[2]
    //   {
    //     (Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Person.get_FirstName))),
    //     (Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Person.get_LastName)))
    //   }, new MemberInfo[2]
    //   {
    //     (MemberInfo) MethodBase.GetMethodFromHandle(__methodref (<>f__AnonymousType0<string, string>.get_Name1), __typeref (<>f__AnonymousType0<string, string>)),
    //     (MemberInfo) MethodBase.GetMethodFromHandle(__methodref (<>f__AnonymousType0<string, string>.get_Name2), __typeref (<>f__AnonymousType0<string, string>))
    //   });
    //   MethodBase.GetMethodFromHandle()
    //   Expression.Lambda<Func<Person, object>>((Expression) newExpression, expressions );
    // }
    //
    // public void Test2()
    // {
    //   ParameterExpression parameterExpression = Expression.Parameter(typeof (Person), "p");
    //   Expression.Lambda<Func<Person, object>>((Expression) Expression.New((ConstructorInfo) MethodBase.GetMethodFromHandle(__methodref (<>f__AnonymousType1<string, Guid, int, bool>..ctor), __typeref (<>f__AnonymousType1<string, Guid, int, bool>)), (IEnumerable<Expression>) new Expression[4]
    //   {
    //     (Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Person.get_FirstName))),
    //     (Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Person.get_Id))),
    //     (Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Person.get_Age))),
    //     (Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Person.get_Active)))
    //   }, new MemberInfo[4]
    //   {
    //     (MemberInfo) MethodBase.GetMethodFromHandle(__methodref (<>f__AnonymousType1<string, Guid, int, bool>.get_Name1), __typeref (<>f__AnonymousType1<string, Guid, int, bool>)),
    //     (MemberInfo) MethodBase.GetMethodFromHandle(__methodref (<>f__AnonymousType1<string, Guid, int, bool>.get_Id), __typeref (<>f__AnonymousType1<string, Guid, int, bool>)),
    //     (MemberInfo) MethodBase.GetMethodFromHandle(__methodref (<>f__AnonymousType1<string, Guid, int, bool>.get_AgeNow), __typeref (<>f__AnonymousType1<string, Guid, int, bool>)),
    //     (MemberInfo) MethodBase.GetMethodFromHandle(__methodref (<>f__AnonymousType1<string, Guid, int, bool>.get_IsAlive), __typeref (<>f__AnonymousType1<string, Guid, int, bool>))
    //   }), new ParameterExpression[1]{ parameterExpression });
    // }
}