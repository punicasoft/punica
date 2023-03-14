using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionDynamicTest.Parsing
{
    public static class TextParser
    {
        public static Expression[] Evaluate(string expression, IEvaluator evaluator)
        {
            // Convert the expression into a list of tokens
            List<Token> tokens = Tokenize(expression);

            // Evaluate the expression using the shunting-yard algorithm
            Stack<Token> outputQueue = new Stack<Token>();
            Stack<Token> operatorStack = new Stack<Token>();


            foreach (var token in tokens)
            {
                switch (token.Value)
                {
                    case "&&":
                    case "||":
                    case "<":
                    case ">":
                    case "<=":
                    case ">=":
                    case "==":
                    case "!=":
                    case "!":
                    case "+":
                    case "-":
                    case "/":
                    case "%":
                    case "*":
                    case "?":
                    case ":":
                    case "??":
                    case "in":
                    case "any": // may be take as one type methods
                    case "bind":
                    case "select": // may be take as one type methods
                    case "new":
                    case ",":
                        // Pop operators from the stack until a lower-precedence or left-associative operator is found
                        while (operatorStack.Count > 0 &&
                               (GetPrecedence(token.Value) < GetPrecedence(operatorStack.Peek().Value) ||
                                GetPrecedence(token.Value) == GetPrecedence(operatorStack.Peek().Value) && IsLeftAssociative(token.Value)))
                        {
                            outputQueue.Push(operatorStack.Pop());
                        }

                        // Push the new operator onto the stack
                        operatorStack.Push(token);
                        break;

                    case "(":
                        // Push left parentheses onto the stack
                        operatorStack.Push(token);
                        break;

                    case ")":
                        // Pop operators from the stack and add them to the output queue until a left parenthesis is found
                        while (operatorStack.Count > 0 && operatorStack.Peek().Value != "(")
                        {
                            outputQueue.Push(operatorStack.Pop());
                        }

                        // If a left parenthesis was not found, the expression is invalid
                        if (operatorStack.Count == 0)
                        {
                            throw new ArgumentException("Mismatched parentheses");
                        }

                        // Pop the left parenthesis from the stack
                        operatorStack.Pop();
                        break;
                    case "{":
                        // Push left parentheses onto the stack
                        operatorStack.Push(token);
                        break;

                    case "}":
                        // Pop operators from the stack and add them to the output queue until a left parenthesis is found
                        while (operatorStack.Count > 0 && operatorStack.Peek().Value != "{")
                        {
                            outputQueue.Push(operatorStack.Pop());
                        }

                        // If a left parenthesis was not found, the expression is invalid
                        if (operatorStack.Count == 0)
                        {
                            throw new ArgumentException("Mismatched curly brackets");
                        }

                        // Pop the left parenthesis from the stack
                        operatorStack.Pop();
                        break;


                    default:
                        // Push operands onto the output queue
                        outputQueue.Push(token);
                        break;
                }
            }

            // Pop any remaining operators from the stack and add them to the output queue
            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek().Value == "(")
                {
                    throw new ArgumentException("Mismatched parentheses");
                }

                outputQueue.Push(operatorStack.Pop());
            }

            // Evaluate the postfix expression
            return Pop(outputQueue, evaluator);
        }

        static Expression[] Pop(Stack<Token> outputQueue, IEvaluator evaluator)
        {
            Stack<object> evaluationStack = new Stack<object>();

            if (outputQueue.Count == 1)
            {
                var exp = evaluator.Single(outputQueue.Pop());
                return new[] { exp };
            }

            foreach (var token in outputQueue.Reverse())
            {
                switch (token.Value)
                {
                    case "&&":
                        var rightOperand1 = evaluationStack.Pop();
                        var leftOperand1 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.And(leftOperand1, rightOperand1));
                        break;

                    case "||":
                        var rightOperand2 = evaluationStack.Pop();
                        var leftOperand2 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Or(leftOperand2, rightOperand2));
                        break;

                    case "!":
                        var operand3 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Not(operand3));
                        break;

                    case "<":
                        var rightOperand4 = evaluationStack.Pop();
                        var leftOperand4 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.LessThan(leftOperand4, rightOperand4));
                        break;

                    case ">=":
                        var rightOperand5 = evaluationStack.Pop();
                        var leftOperand5 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.GreaterOrEqual(leftOperand5, rightOperand5));
                        break;

                    case ">":
                        var rightOperand6 = evaluationStack.Pop();
                        var leftOperand6 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.GreaterThan(leftOperand6, rightOperand6));
                        break;

                    case "<=":
                        var rightOperand7 = evaluationStack.Pop();
                        var leftOperand7 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.LessOrEqual(leftOperand7, rightOperand7));
                        break;

                    case "==":
                        var rightOperand8 = evaluationStack.Pop();
                        var leftOperand8 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Equal(leftOperand8, rightOperand8));
                        break;

                    case "!=":
                        var rightOperand9 = evaluationStack.Pop();
                        var leftOperand9 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.NotEqual(leftOperand9, rightOperand9));
                        break;

                    case "+":
                        var rightOperand10 = evaluationStack.Pop();
                        var leftOperand10 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Add(leftOperand10, rightOperand10));
                        break;

                    case "-":
                        var rightOperand11 = evaluationStack.Pop();
                        var leftOperand11 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Subtract(leftOperand11, rightOperand11));
                        break;

                    case "*":
                        var rightOperand12 = evaluationStack.Pop();
                        var leftOperand12 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Multiply(leftOperand12, rightOperand12));
                        break;

                    case "/":
                        var rightOperand13 = evaluationStack.Pop();
                        var leftOperand13 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Divide(leftOperand13, rightOperand13));
                        break;

                    case "%":
                        var rightOperand14 = evaluationStack.Pop();
                        var leftOperand14 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Modulo(leftOperand14, rightOperand14));
                        break;
                    case "?":
                        var ifFalse = evaluationStack.Pop();
                        var ifTrue = evaluationStack.Pop();
                        var condition = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Condition(condition, ifTrue, ifFalse));
                        break;
                    case "??":
                        var rightOperand15 = evaluationStack.Pop();
                        var leftOperand15 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Coalesce(leftOperand15, rightOperand15));
                        break;
                    case ":":
                        break;
                    case "in":
                        var rightOperand16 = evaluationStack.Pop();
                        var leftOperand16 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Contains(leftOperand16, rightOperand16));
                        break;
                    case "any":
                        var rightOperand17 = evaluationStack.Pop();
                        var leftOperand17 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.Any(leftOperand17, rightOperand17));
                        break;
                    case "new":
                        var rightOperand18 = evaluationStack.Pop();
                        evaluationStack.Push(evaluator.New(rightOperand18));
                        break;
                    default:
                        evaluationStack.Push(token);
                        break;
                }
            }

            // The final value on the stack is the result of the expression

            List<Expression> result = new List<Expression>();

            while (evaluationStack.Count > 0)
            {
               result.Add((Expression)evaluationStack.Pop());
            }

            return result.ToArray();
        }

        static List<Token> Tokenize(string expression)
        {
            List<Token> tokens = new List<Token>();

            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];

                // Skip whitespace
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                // Handle operators
                if (c == ',')
                {
                    tokens.Add(new Token(",", TokenType.Operator));
                }
                else if (c == '&' && i + 1 < expression.Length && expression[i + 1] == '&')
                {
                    tokens.Add(new Token("&&", TokenType.Operator));
                    i++;
                }
                else if (c == '|' && i + 1 < expression.Length && expression[i + 1] == '|')
                {
                    tokens.Add(new Token("||", TokenType.Operator));
                    i++;
                }
                else if (c == '!' && i + 1 < expression.Length && expression[i + 1] == '=')
                {
                    tokens.Add(new Token("!=", TokenType.Operator));
                    i++;
                }
                else if (c == '!')
                {
                    tokens.Add(new Token("!", TokenType.Operator));
                }
                else if (c == '<' && i + 1 < expression.Length && expression[i + 1] == '=')
                {
                    tokens.Add(new Token("<=", TokenType.Operator));
                    i++;
                }
                else if (c == '>' && i + 1 < expression.Length && expression[i + 1] == '=')
                {
                    tokens.Add(new Token(">=", TokenType.Operator));
                    i++;
                }
                else if (c == '<')
                {
                    tokens.Add(new Token("<", TokenType.Operator));
                }
                else if (c == '>')
                {
                    tokens.Add(new Token(">", TokenType.Operator));
                }
                else if (c == '=' && i + 1 < expression.Length && expression[i + 1] == '=')
                {
                    tokens.Add(new Token("==", TokenType.Operator));
                    i++;
                }
                else if (c == '(')
                {
                    tokens.Add(new Token("(", TokenType.Operator));
                }
                else if (c == ')')
                {
                    tokens.Add(new Token(")", TokenType.Operator));
                }


                else if (c == '{')
                {
                    tokens.Add(new Token("{", TokenType.Operator));
                }
                else if (c == '}')
                {
                    tokens.Add(new Token("}", TokenType.Operator));
                }


                else if (c == '+')
                {
                    tokens.Add(new Token("+", TokenType.Operator));
                }
                else if (c == '-')
                {
                    tokens.Add(new Token("-", TokenType.Operator));
                }
                else if (c == '/')
                {
                    tokens.Add(new Token("/", TokenType.Operator));
                }
                else if (c == '*')
                {
                    tokens.Add(new Token("*", TokenType.Operator));
                }
                else if (c == '%')
                {
                    tokens.Add(new Token("%", TokenType.Operator));
                }
                else if (c == '?' && i + 1 < expression.Length && expression[i + 1] == '?')
                {
                    tokens.Add(new Token("??", TokenType.Operator));
                    i++;
                }
                else if (c == '?')
                {
                    tokens.Add(new Token("?", TokenType.Operator));
                }
                else if (c == ':')
                {
                    tokens.Add(new Token(":", TokenType.Operator));
                }
                else if (c == 'i' && i + 1 < expression.Length && expression[i + 1] == 'n')
                {
                    tokens.Add(new Token("in", TokenType.Operator));
                    i++;
                }
                else if (c == '{')
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
                    tokens.Add(new Token(innerExp, TokenType.String));
                    i = j - 1;
                }
                else
                {
                    bool number = true;
                    bool real = false;

                    // Handle operands
                    int j = i + 1;

                    if (!char.IsDigit(c))
                    {
                        number = false;
                    }

                    int dotIndex = -1;
                    int methodIndex = -1;

                    while (j < expression.Length && !char.IsWhiteSpace(expression[j]))
                    {
                        if (number)
                        {
                            if (!char.IsDigit(expression[j]) && expression[j] != '.')
                            {
                                number = false;
                            }

                            if (expression[j] == '.')
                            {
                                if (real)
                                {
                                    throw new ArgumentException(
                                        $"Invalid number detected {expression.Substring(i, j)}");
                                }

                                real = true;
                            }
                        }
                        else
                        {
                            //Childrens.any('Male' in Gender && 5 > (1+2) ).any()
                            if (expression[j] == '.')
                            {
                                dotIndex = j;
                            }
                            else if (expression[j] == '(')
                            {
                                if (dotIndex < 0 || dotIndex + 1 == j)
                                {
                                    throw new ArgumentException($"Invalid method syntax detected {expression.Substring(i, j)}");
                                }

                                methodIndex = j;

                                int k = j + 1;
                                int depth = 1;

                                while (k < expression.Length && depth > 0)
                                {
                                    switch (expression[k])
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

                                    k++;
                                }

                                if (depth != 0)
                                {
                                    throw new ArgumentException("Input contains mismatched parentheses.");
                                }

                                var member = expression.Substring(i, dotIndex - i);
                                var method = expression.Substring(dotIndex + 1, methodIndex - dotIndex - 1);
                                var innerExp = expression.Substring(methodIndex + 1, k - methodIndex - 2);

                                tokens.Add(new Token(member, TokenType.Member));
                                tokens.Add(new Token(method, TokenType.Operator));
                                tokens.Add(new Token(innerExp, TokenType.String));
                                i = k;
                                j = i - 1; //j++ will make j = i since we want to start where we left off instead original like j= i+1;
                            }

                        }

                        j++;
                    }

                    if (i >= expression.Length || i == j)
                    {
                        continue;
                    }

                    var stringVal = expression.Substring(i, j - i);
                    var type = TokenType.Unknown;

                    if (number)
                    {
                        type = real ? TokenType.RealNumber : TokenType.Number;
                    }
                    else if (stringVal.StartsWith('\'') && stringVal.EndsWith('\''))
                    {
                        type = TokenType.String;
                    }
                    else if (stringVal.Equals("true", StringComparison.Ordinal) || stringVal.Equals("false", StringComparison.Ordinal))
                    {
                        type = TokenType.Boolean;
                    }
                    else if (stringVal.Equals("any", StringComparison.Ordinal))
                    {
                        type = TokenType.Operator; // remove this may be??
                    }
                    else if (stringVal.Equals("new", StringComparison.Ordinal))
                    {
                        type = TokenType.Operator; // remove this may be??
                    }
                    else if (stringVal.StartsWith('@'))
                    {
                        type = TokenType.Parameter;
                    }
                    else
                    {
                        type = TokenType.Member;
                    }


                    tokens.Add(new Token(stringVal.Trim('\'').TrimStart('@'), type));
                    i = j - 1;
                }
            }

            return tokens;
        }




        // Based on https://learn.microsoft.com/en-us/cpp/c-language/precedence-and-order-of-evaluation?view=msvc-170
        // and https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/
        public static int GetPrecedence(string op)
        {
            switch (op)
            {

                // Sequential evaluation
                case ",":
                    return -1;

                // Assignment Operators
                case "=":
                    return 0;
                case "+=":
                case "-=":
                    return 0;
                case "*=":
                case "/=":
                case "%=":
                    return 0;
                case "&=":
                case "|=":
                case "^=":
                    return 0;
                case "<<=":
                case ">>=":
                    return 0;


                // Conditional Operators
                case "?":
                case ":":
                    return 1;

                // Null-coalescing operator
                case "??":
                    return 2;


                // Logical Operators
                case "||":
                    return 3;
                case "&&":
                    return 4;

                // Bitwise Operators
                case "|":
                    return 5;
                case "^":
                    return 6;
                case "&":
                    return 7;

                // Equality Operators
                case "==":
                case "!=":
                    return 8;

                // Relational Operators
                case ">":
                case "<":
                case ">=":
                case "<=":
                    return 9;

                // Type Operators
                case "as":
                case "is":
                    return 9;

                // Collection
                case "in":
                    return 9;

                // Shift
                case "<<":
                case ">>":
                    return 10;

                // Arithmetic Operators
                case "+":
                case "-":
                    return 11;
                case "*":
                case "/":
                case "%":
                    return 12;

                // Unary Operators
                case "~":
                case "!":
                    return 13;

                case "++":
                case "--":
                    return 14;
                case "(":
                case ")":
                    return -2; // while parentheses has higher precedence this handles differently

                case "{":
                case "}":
                    return -2; // while parentheses has higher precedence this handles differently

                case "any":
                case "select":
                case "new":
                case "bind":
                    return 14;


                //// Bitwise Operators
                //case "&":
                //    return 8;
                //case "|":
                //    return 9;
                //case "^":
                //    return 10;
                //case "~":
                //    return 1;
                //case "<<":
                //case ">>":
                //    return 5;

                //// Miscellaneous Operators
                //case "sizeof":
                //case "typeof":
                //case "delegate":
                //    return 1;
                //case "new":
                //    return 2;

                default:
                    throw new ArgumentException("Invalid operator", nameof(op));
            }
        }


        static bool IsLeftAssociative(string op)
        {
            switch (op)
            {
                case "&&":
                case "||":
                case "==":
                case "!=":
                case "<":
                case "<=":
                case ">":
                case ">=":
                case "*":
                case "/":
                case "%":
                case "+":
                case "-":
                case "in":
                case "any":
                case "bind":
                case ",":
                    return true;
                case ":":
                case "??":
                // case "?":
                case "=":
                case "++":
                case "--":
                case "!":
                case "new":
                    return false;
                default:
                    throw new ArgumentException($"Invalid operator: {op}");
            }
        }


        //static bool IsLeftAssociative(string op)
        //{
        //    switch (op)
        //    {
        //        case "&&":
        //        case "||":
        //            return true;
        //        case "==":
        //        case "!=":
        //        case "<":
        //        case "<=":
        //        case ">":
        //        case ">=":
        //            return false;
        //        default:
        //            throw new ArgumentException($"Invalid operator: {op}");
        //    }
        //}

    }
}
