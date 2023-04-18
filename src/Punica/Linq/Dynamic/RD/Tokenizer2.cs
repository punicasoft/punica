using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD
{
    public class Tokenizer2
    {
        private static readonly Dictionary<string, IToken> Tokens = new Dictionary<string, IToken>()
        {
            {"in", TokenCache.In},
            {"as", TokenCache.As},
            {"true", TokenCache.True},
            {"false", TokenCache.False},
           // {"new", TokenCache.New}
        };

        public static RootToken Evaluate(TokenContext context)
        {
            return new RootToken(context.MethodContext.GetParameters(), Tokenize(context));
        }

        public static List<IToken> Tokenize(TokenContext context)
        {
            List<IToken> tokens = new List<IToken>();

            while (context.CanRead)
            {
                GetToken(context, out var token);

                if (token != null)
                {
                    tokens.Add(token);
                }

               // context.NextToken();
            }

            //for (int i = 0; i < context.Text.Length; i++)
            //{
            //    i = GetToken(context, i, out var token);
            //    if (token != null)
            //    {
            //        tokens.Add(token);
            //    }

            //}

            return tokens;
        }

        private static void GetToken(TokenContext context, out IToken? token)
        {
            token = null;
            char c = context.Current; //context.Text[i];

            //// Skip whitespace
            //if (char.IsWhiteSpace(c))
            //{
            //    return i;
            //}

            switch (c)
            {
                case '!':
                    context.NextToken();
                    if (context.Current == '=')
                    {
                        context.NextToken();
                        token = TokenCache.NotEqual;
                    }
                    else
                    {
                        token = TokenCache.Not;
                    }

                    break;
                case '"':
                    AddString(context, c, out token);
                    break;
                case '#':
                case '$':
                    throw new NotImplementedException($"Character '{c}' not supported");
                case '%':
                    context.NextToken();
                    token = TokenCache.Modulo;
                    break;
                case '&':
                    context.NextToken();
                    if (context.Current== '&')
                    {
                        context.NextToken();
                        token = TokenCache.AndAlso;
                    }
                    else
                    {
                        token = TokenCache.BitwiseAnd;
                    }

                    break;
                case '\'':
                    AddString(context, c, out token);
                    context.NextToken();
                    break;
                case '(':
                    context.NextToken();
                    token = TokenCache.LeftParenthesis;
                    break;
                case ')':
                    context.NextToken();
                    token = TokenCache.RightParenthesis;
                    break;
                case '*':
                    context.NextToken();
                    token = TokenCache.Multiply;
                    break;
                case '+':
                    context.NextToken();
                    token = TokenCache.Add;
                    break;
                case ',':
                    context.NextToken();
                    break;
                case '-':
                    context.NextToken();
                    token = TokenCache.Subtract;
                    break;
                case '.':
                    throw new NotImplementedException($"Character '{c}' not supported at this level");
                    //token = TokenCache.Dot;
                    break;
                case '/':
                    context.NextToken();
                    token = TokenCache.Divide;
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    AddNumber(context, out token);
                    context.NextToken(); // To Skip spaces
                    break;
                case ':':
                    context.NextToken();
                    break;
                case ';':
                    throw new NotImplementedException($"Character ';' not supported");
                case '<':
                    context.NextToken();
                    if (context.Current == '=')
                    {
                        context.NextToken();
                        token = TokenCache.LessThanOrEqual;
                    }
                    else
                    {
                        token = TokenCache.LessThan;
                    }

                    break;
                case '=':
                    context.NextToken();
                    if (context.Current == '=')
                    {
                        context.NextToken();
                        token = TokenCache.Equal;
                    }
                    else
                    {
                        token = TokenCache.Assign;
                    }

                    break;
                case '>':
                    context.NextToken();
                    if (context.Current == '=')
                    {
                        context.NextToken();
                        token = TokenCache.GreaterThanOrEqual;
                    }
                    else
                    {
                        token = TokenCache.GreaterThan;
                    }

                    break;
                case '?':
                    context.NextToken();
                    if (context.Current == '?')
                    {
                        context.NextToken();
                        token = TokenCache.NullCoalescing;
                    }
                    else
                    {
                        token = TokenCache.Conditional;
                    }

                    break;
                case '@':
                    AddParameter(context, out token);
                    break;
                case '[':
                    throw new NotImplementedException($"Character '{c}' not supported");
                    // tokens.Add(new Token("[", TokenType.OpenBracket, Operator.Unknown));
                    break;
                case ']':
                    // tokens.Add(new Token("[", TokenType.CloseBracket, Operator.Unknown));
                    throw new NotImplementedException($"Character '{c}' not supported");
                    break;
                case '^':
                    throw new NotImplementedException($"Character '^' not supported");
                case '{':
                    throw new NotImplementedException($"Character '{c}' not supported");
                    //i = AddCurlyParenthesesContent(expression, i, out token);
                    break;
                case '|':
                    context.NextToken();
                    if (context.Current == '|')
                    {
                        context.NextToken();
                        token = TokenCache.OrElse;
                    }
                    else
                    {
                        token = TokenCache.BitwiseOr;
                    }

                    break;
                default:
                    if (Match(context, out var key))
                    {
                        context.SetPosition(context.CurrentPosition + key.Length);
                        token = Tokens[key];
                        context.NextToken();
                    }
                    else
                    {
                        Parse3(context, out token);
                    }

                    break;
            }


        }

        /// Person.Name or Person() or Person.Select() or  Person.Select().ToList() 
        public static void Parse3(TokenContext context, out IToken token)
        {
            IExpression exp = context.MethodContext.GetParameter();
            token = null;
            var identifier = GetIdentifier(context);
            //i++;

            //context.NextToken(false);


            var literal = NextToken(context);
            while (!string.IsNullOrEmpty(literal))
            {
                if (literal == ".")
                {
                    context.NextToken(false);
                    if (!string.IsNullOrEmpty(identifier))
                    {
                        exp = new PropertyToken(exp, identifier);
                    }
                }
                else if (literal == "(")
                {
                    context.MethodContext.NextDepth();
                    context.MethodContext.AddParameter(exp);
                    var methodToken = new MethodToken(identifier, exp, context.MethodContext.GetParameter());
                    identifier = string.Empty;

                    var parameter = new Argument();
                    context.NextToken();
                    int depth = 1;

                    while (context.CanRead && depth > 0)
                    {
                        switch (context.Current)
                        {
                            case '(':
                                throw new ArgumentException("Invalid Case Check algorithm"); // possibly not a option
                            case ')':
                                depth--;
                                if (parameter.Tokens.Count > 0)
                                {
                                    methodToken.AddToken(parameter); // only add if it is on same level as there could be mix due to incorrect number of end curly brackets
                                }
                                context.NextToken();
                                break;
                            case ',':
                                methodToken.AddToken(parameter);
                                parameter = new Argument();
                                context.MethodContext.MoveToNextArgument();
                                context.NextToken();
                                break;
                            default:
                                {
                                    GetToken(context, out var token2);

                                    if (token2 != null)
                                    {
                                        parameter.AddToken(token2);
                                    }

                                    break;
                                }
                        }

                       // context.NextToken();
                    }

                    if (depth != 0)
                    {
                        throw new ArgumentException("Input contains mismatched parentheses.");
                    }

                    context.MethodContext.PreviousDepth();
                    token = methodToken;
                    exp = methodToken;

                }
                else if (literal == "{")
                {
                    if (identifier != "new")
                    {
                        throw new NotSupportedException($"Not able to handle {{ with this identifier {identifier}");
                    }

                    var newToken = new NewToken(context.MethodContext.GetParameter());
                    identifier = string.Empty;

                    var parameter = new Argument();

                    context.NextToken();
                    int depth = 1;

                    while (context.CanRead && depth > 0)
                    {
                        switch (context.Current)
                        {
                            case '{':
                                throw new ArgumentException("Invalid Case Check algorithm"); // possibly not a option
                            case '}':
                                depth--;
                                if (parameter.Tokens.Count > 0)
                                {
                                    newToken.AddToken(parameter); // only add if it is on same level as there could be mix due to incorrect number of end curly brackets
                                }
                                context.NextToken();
                                break;
                            case ',':
                                newToken.AddToken(parameter);
                                context.NextToken();
                                parameter = new Argument();
                                break;
                            default:
                                {
                                    GetToken(context, out var token2);

                                    if (token2 != null)
                                    {
                                        parameter.AddToken(token2);
                                    }

                                    break;
                                }
                        }

                        //context.NextToken();
                    }

                    if (depth != 0)
                    {
                        throw new ArgumentException("Input contains mismatched parentheses.");
                    }

                    token = newToken;
                    exp = newToken;

                }
                else
                {
                   // i++;
                    identifier = literal;
                }

                literal = NextToken(context);
            }

            if (!string.IsNullOrEmpty(identifier))
            {
                exp = new PropertyToken(exp, identifier);
                token = new ValueToken(exp);
            }

           // return i - 1;

        }

        public static string NextToken(TokenContext context)
        {

            //for (; i < context.Text.Length; i++)
            //{

            var c = context.Current;

            while (char.IsWhiteSpace(c))
            {
                context.NextToken(false);
                c = context.Current;
            }

            //// Skip whitespace
            //if (char.IsWhiteSpace(c))
            //{
            //    continue;
            //}

            if (c == '.')
            {
                return ".";
            }
            else if (c == '(')
            {
                return "(";
            }
            else if (c == '[')
            {
                return "[";
            }
            else if (c == '{')
            {
                return "{";
            }
            if (Match(context, out var key))
            {
                return "";
            }
            else if (char.IsLetter(c))
            {
                return GetIdentifier(context);
            }
            else
            {
                return "";
            }
            //}

            return null;
        }

        public static string GetIdentifier(TokenContext context)
        {
            // Handle operands
            int i = context.CurrentPosition;
            context.NextToken(false);
            // int j = i + 1;


            // TODO : have custom punctuation
            while (context.CanRead && !(char.IsWhiteSpace(context.Current) || (char.IsPunctuation(context.Current))))
            {
                context.NextToken(false);
            }


            var stringVal = context.Substring(i, context.CurrentPosition - i);
            return stringVal;
        }




        //private static char Peek(int i, TokenContext context, out int index)
        //{
        //    var j = i + 1;
        //    while (j < context.Text.Length) //&& !(char.IsWhiteSpace(expression[j]) || (char.IsPunctuation(expression[j])))
        //    {
        //        if (char.IsWhiteSpace(context.Text[j]))
        //        {
        //            continue;
        //        }

        //        index = j;
        //        return context.Text[j];
        //    }

        //    index = j;
        //    return '\0';
        //}

        //public static bool IsEndOfExpression(int i, TokenContext context, out int index)
        //{
        //    index = i;

        //    if (char.IsWhiteSpace(context.Text[i]))
        //    {
        //        for (int j = i + 1; j < context.Text.Length; j++)
        //        {
        //            if (char.IsWhiteSpace(context.Text[j]))
        //            {
        //                continue;
        //            }

        //            if (context.Text[j] == '.')
        //            {
        //                index = j;
        //                return false;
        //            }

        //            index = j;
        //            return true;
        //        }

        //        return false;
        //    }
        //    else if (context.Text[i] == '(' || context.Text[i] == '{' || context.Text[i] == ',' || context.Text[i] == ')' || context.Text[i] == '}')
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        //private static IExpression GetStartExpression(ref int i, TokenContext context, out int j, out int index)
        //{
        //    IExpression exp = context.MethodContext.GetParameter();
        //    // Handle operands
        //    j = i + 1;

        //    index = j;
        //    // TODO : have custom punctuation   user, user.name, user . name, user.name.select()
        //    while (j < context.Text.Length && !IsEndOfExpression(j, context, out index))  //&& !(char.IsWhiteSpace(expression[j]) || (char.IsPunctuation(expression[j])))
        //    {

        //        if (context.Text[index] == '.')
        //        {
        //            var property = context.Text.Substring(i, j - i);
        //            exp = new PropertyToken(exp, property);

        //            i = index + 1;

        //            while (i < context.Text.Length && char.IsWhiteSpace(context.Text[i]))
        //            {
        //                i++;
        //            }

        //            index = i;
        //        }

        //        j = index;

        //        j++;
        //    }

        //    return exp;
        //}

        ////TODO detect lambda expressions and use correct one
        //public static int Parse2(int i, TokenContext context, out IToken token)
        //{
        //    token = null;
        //    //Expression exp = context.Parameter;
        //    //// Handle operands
        //    //int j = i + 1;

        //    //int index = j;
        //    //// TODO : have custom punctuation   user, user.name, user . name, user.name.select()
        //    //while (j < context.Text.Length && !IsEndOfExpression(j, context, out index))  //&& !(char.IsWhiteSpace(expression[j]) || (char.IsPunctuation(expression[j])))
        //    //{

        //    //    if (context.Text[index] == '.')
        //    //    {
        //    //        var property = context.Text.Substring(i, j - i);
        //    //        exp = Expression.PropertyOrField(exp, property);

        //    //        i = index + 1;

        //    //        while (i < context.Text.Length && char.IsWhiteSpace(context.Text[i]))
        //    //        {
        //    //            i++;
        //    //        }

        //    //        index = i;
        //    //    }

        //    //    j = index;

        //    //    j++;
        //    //}

        //    var exp = GetStartExpression(ref i, context, out var j, out var index);

        //    if (i >= context.Text.Length || i == j)
        //    {
        //        return i;
        //    }

        //    var stringVal = context.Text.Substring(i, j - i); // 1 char length

        //    j = GetMethod(context, out token, index, stringVal, exp, j);


        //    i = j - 1;

        //    return i;
        //}

        //private static int GetMethod(TokenContext context, out IToken? token, int index, string stringVal, IExpression exp, int j)
        //{
        //    if (context.Text[index] == '(' || context.Text[index] == '{')
        //    {
        //        var openChar = context.Text[index];

        //        var isMethod = false;
        //        var method = stringVal;

        //        ITokenList methodToken = null;

        //        if (method == "new")
        //        {
        //            methodToken = new NewToken(context.MethodContext.GetParameter());
        //        }
        //        else
        //        {
        //            isMethod = true;
        //            context.MethodContext.NextDepth();
        //            context.MethodContext.AddParameter(exp);
        //            methodToken = new MethodToken(method, exp, context.MethodContext.GetParameter());
        //        }

        //        var parameter = new TokenList();
        //        j = index + 1;

        //        int depth = 1;

        //        while (j < context.Text.Length && depth > 0)
        //        {
        //            switch (context.Text[j])
        //            {
        //                case '(':
        //                case '{':
        //                    throw new ArgumentException("Invalid Case Check algorithm"); // possibly not a option
        //                case ')':
        //                    depth--;
        //                    if (openChar == '(' && parameter.Tokens.Count > 0)
        //                    {
        //                        methodToken.AddToken(parameter); // only add if it is on same level as there could be mix due to incorrect number of end curly brackets
        //                    }

        //                    break;
        //                case '}':
        //                    depth--;
        //                    if (openChar == '{' && parameter.Tokens.Count > 0)
        //                    {
        //                        methodToken.AddToken(parameter); // only add if it is on same level as there could be mix due to incorrect number of close parentheses
        //                    }

        //                    break;
        //                case ',':
        //                    methodToken.AddToken(parameter);
        //                    parameter = new TokenList();
        //                    if (isMethod)
        //                    {
        //                        context.MethodContext.MoveToNextArgument(); // TODO: add proper parameters next arguments
        //                    }
        //                    break;
        //                default:
        //                    {
        //                        j = GetToken(context, j, out var token2);

        //                        if (token2 != null)
        //                        {
        //                            parameter.AddToken(token2);
        //                        }

        //                        break;
        //                    }
        //            }

        //            j++;
        //        }

        //        if (depth != 0)
        //        {
        //            throw new ArgumentException("Input contains mismatched parentheses.");
        //        }

        //        if (isMethod)
        //        {
        //            context.MethodContext.PreviousDepth(); // TODO: add proper parameters next arguments
        //        }

        //        token = methodToken;

        //        if (j < context.Text.Length && context.Text[j] == '.')
        //        {
        //            //TODO fix chaining of methods instead of using parameters
        //            context.MethodContext.NextDepth(); // this is incorrect depth but has to use to chain properly
        //            context.MethodContext.AddParameter(methodToken as IExpression); //TODO: has error due to passing as parameter
        //            j = Parse2(j + 1, context, out token);

        //            context.MethodContext.PreviousDepth(); // temp fix additional depth
        //            return j + 1;
        //        }

        //    }
        //    else
        //    {
        //        exp = new PropertyToken(exp, stringVal);
        //        token = new ValueToken(exp);
        //        //exp = Expression.PropertyOrField(exp, stringVal);
        //        // token = new ValueToken(exp);
        //    }

        //    return j;
        //}

        private static int Parse(int i, string expression, out IToken token)
        {
            token = null;
            // Handle operands
            int j = i + 1;


            // TODO : have custom punctuation
            while (j < expression.Length &&
                   !(char.IsWhiteSpace(expression[j]) || (char.IsPunctuation(expression[j]))))
            {
                j++;
            }

            if (i >= expression.Length || i == j)
            {
                return i;
            }

            var stringVal = expression.Substring(i, j - i);

            token = new ValueToken(Expression.Constant(stringVal));
            i = j - 1;
            return i;
        }



        private static bool Match(TokenContext context, out string index)
        {
            index = "";

            foreach (var key in Tokens.Keys)
            {
                index = key;
                var value = key;

                if (context.Match(value))
                {
                    return true;
                }
            }

            return false;
        }

        private static void AddParameter(TokenContext context, out IToken token)
        {
            if (context.VariablesInstance == null)
            {
                throw new ArgumentException("Parameters not supplied");
            }

            context.NextToken(false);
            int i = context.CurrentPosition;
            while (context.CanRead && !(char.IsPunctuation(context.Current)))
            {
                context.NextToken(false);
            }

            var stringVal = context.Substring(i, context.CurrentPosition - i);



            token = new ValueToken(Expression.PropertyOrField(context.VariablesInstance, stringVal));
            // i = j - 1;

            // return i;
        }

        private static void AddNumber(TokenContext context, out IToken token)
        {
            int i = context.CurrentPosition;
            context.NextToken(false);
            //int j = i + 1;
            var real = false;

            while (context.CanRead && !(char.IsWhiteSpace(context.Current)))
            {
                if (!char.IsDigit(context.Current) && context.Current != '.')
                {
                    throw new ArgumentException($"Invalid number detected {context.Substring(i, context.CurrentPosition)}");
                }

                if (context.Current == '.')
                {
                    if (real)
                    {
                        throw new ArgumentException($"Invalid number detected {context.Substring(i, context.CurrentPosition)}");
                    }

                    real = true;
                }

                context.NextToken(false);
            }

            var stringVal = context.Substring(i, context.CurrentPosition - i);

            token = real ? new ValueToken(Expression.Constant(double.Parse(stringVal))) : new ValueToken(Expression.Constant(int.Parse(stringVal)));

            //i = j - 1;

            //return i;
        }



        private static void AddString(TokenContext context, char c, out IToken token)
        {
            // Handle operands
            context.NextToken();
            int startIndex = context.CurrentPosition;

            while (context.CanRead && context.Current != c)
            {
                context.NextToken();
            }

            if (context.Current != c)
            {
                throw new ArgumentException("Input contains invalid string literal.");
            }

            var stringVal = context.Substring(startIndex, context.CurrentPosition - startIndex);

            token = new ValueToken(Expression.Constant(stringVal));
        }
    }
}
