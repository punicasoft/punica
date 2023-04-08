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
            return new RootToken(context.Parameter, Tokenize(context));
        }

        public static List<IToken> Tokenize(TokenContext context)
        {
            List<IToken> tokens = new List<IToken>();

            for (int i = 0; i < context.Text.Length; i++)
            {
                i = GetToken(context, i, out var token);
                if (token != null)
                {
                    tokens.Add(token);
                }

            }

            return tokens;
        }

        private static int GetToken(TokenContext context, int i, out IToken? token)
        {
            token = null;
            char c = context.Text[i];

            // Skip whitespace
            if (char.IsWhiteSpace(c))
            {
                return i;
            }

            switch (c)
            {
                case '!':
                    if (i + 1 < context.Text.Length && context.Text[i + 1] == '=')
                    {
                        token = TokenCache.NotEqual;
                        i++;
                    }
                    else
                    {
                        token = TokenCache.Not;
                    }

                    break;
                case '"':
                    i = AddString(context.Text, i, c, out token);
                    break;
                case '#':
                case '$':
                    throw new NotImplementedException($"Character '{c}' not supported");
                case '%':
                    token = TokenCache.Modulo;
                    break;
                case '&':
                    if (i + 1 < context.Text.Length && context.Text[i + 1] == '&')
                    {
                        token = TokenCache.AndAlso;
                        i++;
                    }
                    else
                    {
                        token = TokenCache.BitwiseAnd;
                    }

                    break;
                case '\'':
                    i = AddString(context.Text, i, c, out token);
                    break;
                case '(':
                    //i = AddOpenParenthsesOrMethodContent(expression, tokens, i);
                    token = TokenCache.LeftParenthesis;
                    break;
                case ')':
                    token = TokenCache.RightParenthesis;
                    break;
                case '*':
                    token = TokenCache.Multiply;
                    break;
                case '+':
                    token = TokenCache.Add;
                    break;
                case ',':
                    //tokens.Add(new Token(",", TokenType.Sequence, Operator.Unknown));
                    break;
                case '-':
                    token = TokenCache.Subtract;
                    break;
                case '.':
                    throw new NotImplementedException($"Character '{c}' not supported at this level");
                    //token = TokenCache.Dot;
                    break;
                case '/':
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
                    i = AddNumber(context.Text, i, out token);
                    break;
                case ':':
                    //token = TokenCache.Colon;
                    break;
                case ';':
                    throw new NotImplementedException($"Character ';' not supported");
                case '<':
                    if (i + 1 < context.Text.Length && context.Text[i + 1] == '=')
                    {
                        token = TokenCache.LessThanOrEqual;
                        i++;
                    }
                    else
                    {
                        token = TokenCache.LessThan;
                    }

                    break;
                case '=':
                    if (i + 1 < context.Text.Length && context.Text[i + 1] == '=')
                    {
                        token = TokenCache.Equal;
                        i++;
                    }
                    else
                    {
                        token = TokenCache.Assign;
                    }

                    break;
                case '>':
                    if (i + 1 < context.Text.Length && context.Text[i + 1] == '=')
                    {
                        token = TokenCache.GreaterThanOrEqual;
                        i++;
                    }
                    else
                    {
                        token = TokenCache.GreaterThan;
                    }

                    break;
                case '?':
                    if (i + 1 < context.Text.Length && context.Text[i + 1] == '?')
                    {
                        token = TokenCache.NullCoalescing;
                        i++;
                    }
                    else
                    {
                        token = TokenCache.Conditional;
                    }

                    break;
                case '@':
                    i = AddParameter(context, i, out token);
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
                    if (i + 1 < context.Text.Length && context.Text[i + 1] == '|')
                    {
                        token = TokenCache.OrElse;
                        i++;
                    }
                    else
                    {
                        token = TokenCache.BitwiseOr;
                    }

                    break;
                default:
                    if (Match(i, context.Text, out var key)) //TODO handle new?
                    {
                        token = Tokens[key];
                        //tokens.Add(token);
                        i = i + key.Length - 1;
                        return i;
                    }
                    else
                    {
                        i = Parse2(i, context, out token);
                    }

                    break;
            }

            return i;
        }


        private static char Peek(int i, TokenContext context, out int index)
        {
            var j = i + 1;
            while (j < context.Text.Length) //&& !(char.IsWhiteSpace(expression[j]) || (char.IsPunctuation(expression[j])))
            {
                if (char.IsWhiteSpace(context.Text[j]))
                {
                    continue;
                }

                index = j;
                return context.Text[j];
            }

            index = j;
            return '\0';
        }

        public static bool IsEndOfExpression(int i, TokenContext context, out int index)
        {
            bool isEndOfIdentifier = false;
            index = i;

            if (char.IsWhiteSpace(context.Text[i]))
            {
                for (int j = i + 1; j < context.Text.Length; j++)
                {
                    if (char.IsWhiteSpace(context.Text[j]))
                    {
                        continue;
                    }

                    if (context.Text[j] == '.')
                    {
                        index = j;
                        return false;
                    }

                    index = j;
                    return true;
                }

                return false;
            }
            else if (context.Text[i] == '(' || context.Text[i] == '{' || context.Text[i] == ',' || context.Text[i] == ')' || context.Text[i] == '}')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //TODO parse at once for expressions
        public static int Parse2(int i, TokenContext context, out IToken token)
        {
            token = null;
            Expression exp = context.Parameter;
            // Handle operands
            int j = i + 1;

            int index = j;
            // TODO : have custom punctuation   user, user.name, user . name, user.name.select()
            while (j < context.Text.Length && !IsEndOfExpression(j, context, out index))  //&& !(char.IsWhiteSpace(expression[j]) || (char.IsPunctuation(expression[j])))
            {

                if (context.Text[index] == '.')
                {
                    var property = context.Text.Substring(i, j - i);
                    exp = Expression.PropertyOrField(exp, property);

                    i = index + 1;

                    while (i < context.Text.Length && char.IsWhiteSpace(context.Text[i]))
                    {
                        i++;
                    }

                    index = i;
                }

                j = index;

                j++;
            }

            if (i >= context.Text.Length || i == j)
            {
                return i;
            }


            var stringVal = context.Text.Substring(i, j - i); // 1 char length

            if (context.Text[index] == '(' || context.Text[index] == '{')
            {
                var openChar = context.Text[index];

                var method = stringVal;

                ITokenList methodToken = null;

                if (method == "new")
                {
                    methodToken = new NewToken(context.Parameter);
                }
                else
                {
                    methodToken = new MethodToken(method, exp, context.Depth + 1);
                }

                var parameter = new TokenList();
                j = index + 1;

                int depth = 1;

                while (j < context.Text.Length && depth > 0)
                {
                    switch (context.Text[j])
                    {
                        case '(':
                        case '{':
                            depth++; // possibly not a option
                            throw new ArgumentException("Invalid Case Check algorithm");
                            break;
                        case ')':
                            depth--;
                            if (openChar == '(')
                            {
                                methodToken.AddToken(parameter); // only add if it is on same level as there could be mix due to incorrect number of end curly brackets
                            }
                            break;
                        case '}':
                            depth--;
                            if (openChar == '{')
                            {
                                methodToken.AddToken(parameter);  // only add if it is on same level as there could be mix due to incorrect number of close parentheses
                            }
                            break;
                        case ',':
                            methodToken.AddToken(parameter);
                            parameter = new TokenList();
                            break;
                        default:
                            {
                                //TODO: if it is method with function/expression then change the context with correct arg
                                j = GetToken(new TokenContext(context, methodToken.Parameter), j, out var token2);

                                if (token2 != null)
                                {
                                    parameter.AddToken(token2);
                                }

                                break;
                            }

                    }

                    j++;
                }

                if (depth != 0)
                {
                    throw new ArgumentException("Input contains mismatched parentheses.");
                }

                token = methodToken;

                //j = j - 1;

            }
            else
            {
                exp = Expression.PropertyOrField(exp, stringVal);
                token = new ValueToken(exp);

            }


            i = j - 1;

            return i;
        }

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



        private static bool Match(int i, string expression, out string index)
        {
            index = "";

            foreach (var key in Tokens.Keys)
            {
                index = key;
                var value = key;
                int j;
                for (j = 0; j < value.Length; j++)
                {
                    int k = i + j;

                    if (k >= expression.Length || expression[k] != value[j])
                    {
                        break;
                    }
                }

                if (j == value.Length)
                {
                    return true;
                }
            }

            return false;
        }

        private static int AddCurlyParenthesesContent(string expression, int i, out IToken token)
        {
            int j = i + 1;

            int depth = 1;

            while (j < expression.Length && depth > 0)
            {
                switch (expression[j])
                {
                    case '{':
                        depth++;
                        break;
                    case '}':
                        depth--;
                        break;
                    default:
                        break;
                }

                j++;
            }

            if (depth != 0)
            {
                throw new ArgumentException("Input contains mismatched parentheses.");
            }

            var innerExp = expression.Substring(i + 1, j - i - 2);
            token = new ValueToken(Expression.Constant(innerExp));
            i = j - 1;
            return i;
        }

        private static int AddParameter(TokenContext context, int i, out IToken token)
        {
            if (context.ParameterInstance == null)
            {
                throw new ArgumentException("Parameters not supplied");
            }

            int j = i + 1;
            while (j < context.Text.Length && !(char.IsWhiteSpace(context.Text[j])))
            {
                j++;
            }

            var stringVal = context.Text.Substring(i, j - i);



            token = new ValueToken(Expression.PropertyOrField(context.ParameterInstance, stringVal));
            i = j - 1;

            return i;
        }

        private static int AddNumber(string expression, int i, out IToken token)
        {
            int j = i + 1;
            var real = false;

            while (j < expression.Length && !(char.IsWhiteSpace(expression[j])))
            {
                if (!char.IsDigit(expression[j]) && expression[j] != '.')
                {
                    throw new ArgumentException($"Invalid number detected {expression.Substring(i, j)}");
                }

                if (expression[j] == '.')
                {
                    if (real)
                    {
                        throw new ArgumentException($"Invalid number detected {expression.Substring(i, j)}");
                    }

                    real = true;
                }

                j++;
            }

            var stringVal = expression.Substring(i, j - i);

            token = real ? new ValueToken(Expression.Constant(double.Parse(stringVal))) : new ValueToken(Expression.Constant(int.Parse(stringVal)));

            i = j - 1;

            return i;
        }

        private static int AddOpenParenthsesOrMethodContent(string expression, List<IToken> tokens, int i)
        {
            var count = tokens.Count;

            if (count > 0)
            {
                if (tokens[count - 1].TokenType == TokenType.Member)
                {
                    int j = i + 1;

                    int depth = 1;

                    while (j < expression.Length && depth > 0)
                    {
                        switch (expression[j])
                        {
                            case '(':
                                depth++;
                                break;
                            case ')':
                                depth--;
                                break;
                            default:
                                break;
                        }

                        j++;
                    }

                    if (depth != 0)
                    {
                        throw new ArgumentException("Input contains mismatched parentheses.");
                    }

                    var innerExp = expression.Substring(i + 1, j - i - 2);

                    //tokens[count - 1] = new Token(tokens[count - 1].Value, TokenType.Operator, Operator.Method); //set previous token as a method
                    tokens.Add(new ValueToken(Expression.Constant(innerExp)));

                    i = j - 1;
                }
                else
                {
                    tokens.Add(TokenCache.LeftParenthesis);
                }
            }
            else
            {
                tokens.Add(TokenCache.LeftParenthesis);
            }

            return i;
        }

        private static int AddString(string expression, int i, char c, out IToken token)
        {
            // Handle operands
            int j = i + 1;

            while (j < expression.Length && expression[j] != c)
            {
                j++;
            }

            if (expression[j] != c)
            {
                throw new ArgumentException("Input contains invalid string literal.");
            }

            var stringVal = expression.Substring(i + 1, j - i - 1);

            token = new ValueToken(Expression.Constant(stringVal));

            i = j; // last character since we starting with next
            return i;
        }
    }
}
