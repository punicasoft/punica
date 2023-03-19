using Microsoft.EntityFrameworkCore;
using Punica.Bp.EFCore.Query.Parsing;
using Punica.Dynamic;
using Punica.Extensions;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TypeExtensions = Punica.Extensions.TypeExtensions;

namespace Punica.Bp.EFCore.Querying
{
    public class QueryBase
    {
        private readonly DbContext _dbContext;
        private static readonly MethodInfo WhereMethod = typeof(QueryBase).GetMethod(nameof(Where));
        private static readonly MethodInfo _stringContainsMethod =
            typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) });

        private static readonly MethodInfo _enubarelContainsMethod =
            typeof(Enumerable).GetMethod(nameof(Enumerable.Contains));

        public QueryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected List<dynamic> GetList<TEntity>(List<string> columns, string filter) where TEntity : class
        {
            var query = _dbContext.Set<TEntity>().AsNoTracking();

            var type = GetType(typeof(TEntity), columns);

            MethodInfo methodInfoGeneric = WhereMethod.MakeGenericMethod(new[] { typeof(TEntity), type });
            var exp = methodInfoGeneric.Invoke(this, new object[] { query, columns.AsEnumerable(), filter });
            var fillterdQuery = exp as IQueryable<dynamic>;
            return fillterdQuery.ToList();
        }

        public IQueryable<TResult> Where<TSource, TResult>(IQueryable<TSource> queryable, IEnumerable<string> fields, string filter)
        {
            var projectionQuery = queryable.Select(Select<TSource, TResult>(fields));
            //var predicate = ExpandoFilter2<TResult>(propertyName, propertyValue);

            var evaluator = new Evaluator(typeof(TResult));
            var expression = TextParser.Evaluate(filter, evaluator);

            return projectionQuery.Where(evaluator.GetFilterExpression<TResult>(expression));

            return projectionQuery;
        }

        public Expression<Func<TSource, TResult>> Select<TSource, TResult>(IEnumerable<string> fields)
        {
            ParameterExpression arg = Expression.Parameter(typeof(TSource), "arg");

            var expression = GetMemberInit(fields, typeof(TResult), arg); 

            return Expression.Lambda<Func<TSource, TResult>>(expression, arg);
        }

        private MemberInitExpression GetMemberInit(IEnumerable<string> fields, Type resultType, Expression arg)
        {
            // loop through all of the result fields and generate an expression that will 
            // add a new property to the result expando object using its IDictionary interface
            var bindings = new List<MemberBinding>();
            var expando = Expression.New(resultType);

            var properties = resultType.GetProperties();

            foreach (string name in fields)
            {
                if (!name.Contains("|"))
                {
                    var field = GetFieldMetaData(name);

                    var parts = field.Name.Split(".");
                    Expression expression = arg;

                    for (var i = 0; i < parts.Length; i++)
                    {
                        var part = parts[i];
                        expression = Expression.PropertyOrField(expression, part);
                    }

                    var prop = properties.First(prop => prop.Name == field.Alias);
                    bindings.Add(Expression.Bind(prop, expression));
                }
                else
                {
                    var index = name.IndexOf("|");

                    if (index == 0 || index == name.Length - 1)
                    {
                        throw new InvalidDataException($"{name} unable to parse as a valid select");
                    }

                    var propName = name.Substring(0, index);
                    var field = GetFieldMetaData(propName);

                    var prop = properties.First(prop => prop.Name == field.Alias);

                    if (IsCollectionOrList(prop.PropertyType))
                    {
                        //      list = query.Select(a => new
                        //      {
                        //          Id = a.Id,
                        //          Status = a.Status,
                        //          Buyer = new { a.Buyer.Name, Email}
                        //          Products = a.Items.Select(i => new
                        //          {
                        //              Id = i.Id,
                        //              Name = i.ProductName,
                        //              Price = i.UnitPrice
                        //          }).ToList()
                        //      });

                        // in List<Products> extract the type of Generic Argument typeof(Product) 
                        var type = TypeExtensions.GetElementOrGenericArgType(prop.PropertyType);

                        var columns = name.Substring(index + 1).Split(",");

                        // a.Items
                        var expression = Expression.PropertyOrField(arg, field.Name);

                        //   // type of Items from List<Items> 
                        var sourceType = TypeExtensions.GetElementOrGenericArgType(expression.Type);
                        

                        // Function<Item, Product>
                        var funcType = typeof(Func<,>).MakeGenericType(sourceType, type);


                        // MethodInfo addMethod = sourceType.GetMethod("Select", new Type[] { funcType });

                        MethodInfo selectMethod = new Func<IEnumerable<object>, Func<object, object>, IEnumerable<object>>(Enumerable.Select)
                            .GetMethodInfo().GetGenericMethodDefinition().MakeGenericMethod(sourceType, type);
                        //MethodInfo selectMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.Select), new Type[] { funcType });
                        MethodInfo toListMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToList)).MakeGenericMethod(type);


                        ParameterExpression arg2 = Expression.Parameter(sourceType, "arg2");

                        var memberInitExpression = GetMemberInit(columns, type, arg2);

                        var lambdaExpression = Expression.Lambda(funcType, memberInitExpression, arg2);

                        var selectExpression = Expression.Call((Expression)null, selectMethod, expression, lambdaExpression);

                        var toListExpression = Expression.Call((Expression)null, toListMethod, selectExpression);

                        //Expression.Lambda(expression, arg)

                        //Expression.Call(expression, addMethod, );

                        //GetMemberInit(columns, prop.PropertyType, Expression.PropertyOrField(arg, field.Name))
                        //var assignment = Expression.Bind(prop,
                        //    );

                        var assignment = Expression.Bind(prop, toListExpression);
                        bindings.Add(assignment);
                    }
                    else
                    {
                        var columns = name.Substring(index + 1).Split(",");

                        var assignment = Expression.Bind(prop,
                            GetMemberInit(columns, prop.PropertyType, Expression.PropertyOrField(arg, field.Name)));
                        bindings.Add(assignment);
                    }
                }
            }

            return Expression.MemberInit(expando, bindings);
        }


        private Type GetType(Type type, List<string> columns)
        {
            var properties = new List<AnonymousProperty>();

            foreach (var name in columns)
            {
                if (!name.Contains("|"))
                {
                    properties.Add(GetProperty(type, name));
                }
                else
                {
                    var parts = name.Split("|");
                    if (parts.Length != 2)
                    {
                        throw new InvalidDataException($"{name} unable to parse as a valid select");
                    }

                    var field = GetFieldMetaData(parts[0]);
                    var property = type.GetProperty(field.Name);

                    if (IsCollectionOrList(property.PropertyType))
                    {
                        var implementedType = TypeExtensions.GetElementOrGenericArgType(property.PropertyType);
                        var requesteType = GetType(implementedType, parts[1].Split(",").ToList());
                        var listType = typeof(List<>).MakeGenericType(requesteType);
                        properties.Add(new AnonymousProperty(field.Alias, listType));
                    }
                    else
                    {
                        properties.Add(new AnonymousProperty(field.Alias, GetType(property.PropertyType, parts[1].Split(",").ToList())));
                    }
                }
            }

            return AnonymousTypeFactory.CreateType(properties);
        }

        private AnonymousProperty GetProperty(Type type, string name)
        {

            var field = GetFieldMetaData(name);

            var parts = field.Name.Split(".");
            Type propertyType = type;

            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var propertyInfo = propertyType.GetProperty(part);
                propertyType = propertyInfo.PropertyType;
            }

            return new AnonymousProperty(field.Alias, propertyType);

        }

        private Field GetFieldMetaData(string name)
        {
            if (name.Contains("as"))
            {
                var parts = name.Split("as");

                if (parts.Length != 2)
                {
                    throw new InvalidDataException($"{name} unable to parse as alias");
                }

                return new Field(parts[0], parts[1]);
            }
            else
            {
                return new Field(name);
            }
        }

        private struct Field
        {
            public string Name { get; private set; }
            public string Alias { get; private set; }

            public Field(string name)
            {
                Name = name.Trim();
                Alias = name.Replace(".", "").Trim();
            }

            public Field(string name, string alias)
            {
                Name = name.Trim();
                Alias = alias.Trim();
            }
        }

        public static bool IsCollectionOrList(Type type)
        {
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(List<>) || genericTypeDefinition == typeof(IList<>)
                                                            || genericTypeDefinition == typeof(ICollection<>))
                {
                    return true;
                }
            }

            return false;
        }

        
    }


}
