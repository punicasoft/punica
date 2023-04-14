using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Punica.Bp.EFCore.Query.Parsing
{
    public static class TextParser
    {
        public static Expression Evaluate(string expression, IEvaluator evaluator)
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

        static Expression Pop(Stack<Token> outputQueue, IEvaluator evaluator)
        {
            Stack<object> evaluationStack = new Stack<object>();

            if (outputQueue.Count == 1)
            {
                return evaluator.Single(outputQueue.Pop());
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

                    default:
                        evaluationStack.Push(token);
                        break;
                }
            }

            // The final value on the stack is the result of the expression
            return (Expression)evaluationStack.Pop();
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
                if (c == '&' && i + 1 < expression.Length && expression[i + 1] == '&')
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

                        j++;
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
                    else
                    {
                        type = TokenType.Member;
                    }



                    tokens.Add(new Token(stringVal.Trim('\''), type));
                    i = j - 1;
                }
            }

            return tokens;
        }


        //static int GetPrecedence(string op)
        //{
        //    switch (op)
        //    {
        //        case "!":
        //            return 5;
        //        case "<":
        //        case "<=":
        //        case ">":
        //        case ">=":
        //            return 4;
        //        case "==":
        //        case "!=":
        //            return 3;
        //        case "&&":
        //            return 2;
        //        case "||":
        //            return 1;
        //        default:
        //            return -1;
        //    }
        //}

        private static int GetPrecedence(string op)
        {
            switch (op)
            {
                case "!":
                    return 6;
                case "==":
                case "!=":
                    return 5;
                case "<":
                case "<=":
                case ">":
                case ">=":
                    return 4;
                case "&&":
                    return 3;
                case "||":
                    return 2;
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                case "%":
                    return 0;
                default:
                    return -1;
            }
        }

        public static int GetOperatorPrecedence(string op)
        {
            switch (op)
            {

                // Conditional Operators
                case "?:":
                case "??":
                    return 14;

                // Assignment Operators
                case "=":
                    return 13;
                case "+=":
                case "-=":
                    return 13;
                case "*=":
                case "/=":
                case "%=":
                    return 13;
                case "&=":
                case "|=":
                case "^=":
                    return 13;
                case "<<=":
                case ">>=":
                    return 13;

                // Logical Operators
                case "&&":
                    return 11;
                case "||":
                    return 12;
                case "!":
                    return 10;

                //// Type Operators
                //case "as":
                //case "is":
                //    return 7;

                // Equality Operators
                case "==":
                case "!=":
                    return 7;

                // Relational Operators
                case ">":
                case "<":
                case ">=":
                case "<=":
                    return 6;

                // Arithmetic Operators
                case "^":
                    return 4;
                case "*": 
                case "/":
                case "%":
                    return 3;
                case "+":
                case "-":
                    return 2;
                case "++":
                case "--":
                    return 1;
                case "(":
                case ")":
                    return -1;



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
                    return true;
                case "==":
                case "!=":
                case "<":
                case "<=":
                case ">":
                case ">=":
                    return false;
                default:
                    throw new ArgumentException($"Invalid operator: {op}");
            }
        }

    }

    public struct Token
    {
        public string Value { get; }
        public TokenType Type { get; }

        public Token(string value, TokenType type)
        {
            Value = value;
            Type = type;
        }
    }

    public enum TokenType
    {
        Unknown,
        Operator,
        Member,
        String,
        Number,
        RealNumber,
    }
}
