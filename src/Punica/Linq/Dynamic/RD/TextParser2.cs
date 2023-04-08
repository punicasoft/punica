using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD
{
    //public class TextParser2
    //{

    //    // TODO: add support for custom function call like MyMethod() since currently it only support Instance.MyMethod(), maybe use this.MyMethod()
    //    public static Expression[] Evaluate(TokenContext context)
    //    {
    //        // Convert the expression into a list of tokens
    //        List<IToken> tokens = Tokenizer2.Tokenize(context);

    //        //If there is no tokens return empty array;
    //        if (tokens.Count == 0)
    //        {
    //            return Array.Empty<Expression>();
    //        }

    //        // Evaluate the expression using the shunting-yard algorithm
    //        Stack<IToken> outputQueue = new Stack<IToken>();
    //        Stack<IToken> operatorStack = new Stack<IToken>();
    //        List<Expression> expressions = new List<Expression>();

    //        foreach (var token in tokens)
    //        {
    //            switch (token.TokenType)
    //            {
    //                case TokenType.Operator:
    //                    // Pop operators from the stack until a lower-precedence or left-associative operator is found
    //                    while (operatorStack.Count > 0 &&
    //                           (token.Precedence < operatorStack.Peek().Precedence ||
    //                           token.Precedence == operatorStack.Peek().Precedence && token.IsLeftAssociative))
    //                    {
    //                        outputQueue.Push(operatorStack.Pop());
    //                    }

    //                    // Push the new operator onto the stack
    //                    operatorStack.Push(token);
    //                    break;

    //                case TokenType.OpenParen:
    //                    // Push left parentheses onto the stack
    //                    operatorStack.Push(token);
    //                    break;

    //                case TokenType.CloseParen:
    //                    // Pop operators from the stack and add them to the output queue until a left parenthesis is found
    //                    while (operatorStack.Count > 0 && operatorStack.Peek().TokenType != TokenType.OpenParen)
    //                    {
    //                        outputQueue.Push(operatorStack.Pop());
    //                    }

    //                    // If a left parenthesis was not found, the expression is invalid
    //                    if (operatorStack.Count == 0)
    //                    {
    //                        throw new ArgumentException("Mismatched parentheses");
    //                    }

    //                    // Pop the left parenthesis from the stack
    //                    operatorStack.Pop();
    //                    break;

    //                case TokenType.Unknown:
    //                    // Do nothing 
    //                    break;

    //                case TokenType.Sequence:
    //                    // Pop any remaining operators from the stack and add them to the output queue

    //                    throw new ArgumentException("Should not be available");
    //                    //while (operatorStack.Count > 0)
    //                    //{
    //                    //    if (operatorStack.Peek().Value == "(")
    //                    //    {
    //                    //        throw new ArgumentException("Mismatched parentheses");
    //                    //    }

    //                    //    outputQueue.Push(operatorStack.Pop());
    //                    //}

    //                    //expressions.Add(Pop(outputQueue, evaluator));
    //                    break;
    //                default:
    //                    // Push operands onto the output queue
    //                    outputQueue.Push(token);
    //                    break;
    //            }
    //        }

    //        // Pop any remaining operators from the stack and add them to the output queue
    //        while (operatorStack.Count > 0)
    //        {
    //            if (operatorStack.Peek().TokenType == TokenType.OpenParen)
    //            {
    //                throw new ArgumentException("Mismatched parentheses");
    //            }

    //            outputQueue.Push(operatorStack.Pop());
    //        }

    //        // Evaluate the postfix expression
    //        expressions.Add(Pop(outputQueue));

    //        return expressions.ToArray();
    //    }

    //    static Expression Pop(Stack<IToken> outputQueue)
    //    {
    //        Stack<Expression> evaluationStack = new Stack<Expression>();

    //        if (outputQueue.Count == 1)
    //        {
    //            var token = outputQueue.Pop();
    //            if (token is ValueToken vt)
    //            {
    //               return vt.Value;
    //            }
    //            else if (token is Operation ot)
    //            {
    //                return ot.Evaluate(evaluationStack);
    //            }
    //        }

    //        var reverse = outputQueue.Reverse();


    //        foreach (var token in reverse)
    //        {
    //            if (token is Operation ot)
    //            {
    //                evaluationStack.Push(ot.Evaluate(evaluationStack)); //TODO sdkslds
    //            }

    //            switch (token.Operator)
    //            {
    //                case Operator.And:
    //                    var rightOperand1 = evaluationStack.Pop();
    //                    var leftOperand1 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.And(leftOperand1, rightOperand1));
    //                    break;

    //                case Operator.Or:
    //                    var rightOperand2 = evaluationStack.Pop();
    //                    var leftOperand2 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.Or(leftOperand2, rightOperand2));
    //                    break;

    //                case Operator.Not:
    //                    var operand3 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.Not(operand3));
    //                    break;

    //                case Operator.LessThan:
    //                    var rightOperand4 = evaluationStack.Pop();
    //                    var leftOperand4 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.LessThan(leftOperand4, rightOperand4));
    //                    break;

    //                case Operator.GreaterThanEqual:
    //                    var rightOperand5 = evaluationStack.Pop();
    //                    var leftOperand5 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.GreaterOrEqual(leftOperand5, rightOperand5));
    //                    break;

    //                case Operator.GreaterThan:
    //                    var rightOperand6 = evaluationStack.Pop();
    //                    var leftOperand6 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.GreaterThan(leftOperand6, rightOperand6));
    //                    break;

    //                case Operator.LessThanEqual:
    //                    var rightOperand7 = evaluationStack.Pop();
    //                    var leftOperand7 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.LessOrEqual(leftOperand7, rightOperand7));
    //                    break;

    //                case Operator.Equal:
    //                    var rightOperand8 = evaluationStack.Pop();
    //                    var leftOperand8 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.Equal(leftOperand8, rightOperand8));
    //                    break;

    //                case Operator.NotEqual:
    //                    var rightOperand9 = evaluationStack.Pop();
    //                    var leftOperand9 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.NotEqual(leftOperand9, rightOperand9));
    //                    break;

    //                case Operator.Plus:
    //                    var rightOperand10 = evaluationStack.Pop();
    //                    var leftOperand10 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.Add(leftOperand10, rightOperand10));
    //                    break;

    //                case Operator.Minus:
    //                    var rightOperand11 = evaluationStack.Pop();
    //                    var leftOperand11 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.Subtract(leftOperand11, rightOperand11));
    //                    break;

    //                case Operator.Multiply:
    //                    var rightOperand12 = evaluationStack.Pop();
    //                    var leftOperand12 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.Multiply(leftOperand12, rightOperand12));
    //                    break;

    //                case Operator.Divide:
    //                    var rightOperand13 = evaluationStack.Pop();
    //                    var leftOperand13 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.Divide(leftOperand13, rightOperand13));
    //                    break;

    //                case Operator.Modulo:
    //                    var rightOperand14 = evaluationStack.Pop();
    //                    var leftOperand14 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.Modulo(leftOperand14, rightOperand14));
    //                    break;
    //                case Operator.Question:
    //                    var ifFalse = evaluationStack.Pop();
    //                    var ifTrue = evaluationStack.Pop();
    //                    var condition = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.Condition(condition, ifTrue, ifFalse));
    //                    break;
    //                case Operator.NullCoalescing:
    //                    var rightOperand15 = evaluationStack.Pop();
    //                    var leftOperand15 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.Coalesce(leftOperand15, rightOperand15));
    //                    break;
    //                case Operator.Colon:
    //                    break;
    //                case Operator.In:
    //                    var rightOperand16 = evaluationStack.Pop();
    //                    var leftOperand16 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.Contains(leftOperand16, rightOperand16));
    //                    break;
    //                //case Operator.Method:
    //                //    var rightOperand17 = evaluationStack.Pop();
    //                //    var leftOperand17 = evaluationStack.Pop();
    //                //    evaluationStack.Push(evaluator.Any(leftOperand17, rightOperand17));
    //                //    break;
    //                case Operator.New:
    //                    var rightOperand18 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.New(rightOperand18));
    //                    break;
    //                case Operator.Dot:

    //                    var rightOperand19 = evaluationStack.Pop();
    //                    var leftOperand19 = evaluationStack.Pop();

    //                    if (evaluationStack.Count > 0 && (evaluationStack.Peek() is Token t1 && t1.Type == TokenType.Member || evaluationStack.Peek() is Expression))
    //                    {
    //                        var member = evaluationStack.Pop();
    //                        evaluationStack.Push(evaluator.Call(member, leftOperand19, rightOperand19));
    //                    }
    //                    else
    //                    {
    //                        evaluationStack.Push(evaluator.Dot(leftOperand19, rightOperand19));
    //                    }

    //                    break;
    //                //case Operator.Sequence:
    //                //    break;
    //                case Operator.As:
    //                    var rightOperand20 = evaluationStack.Pop();
    //                    var leftOperand20 = evaluationStack.Pop();
    //                    evaluationStack.Push(evaluator.As(leftOperand20, rightOperand20));
    //                    break;
    //                default:
    //                    evaluationStack.Push(token);
    //                    break;
    //            }
    //        }

    //        //// The final value on the stack is the result of the expression

    //        //List<Expression> result = new List<Expression>();

    //        //while (evaluationStack.Count > 0)
    //        //{
    //        //    result.Add((Expression)evaluationStack.Pop());
    //        //}
    //        //return result.ToArray();
    //        outputQueue.Clear();

    //        return (Expression)evaluationStack.Pop();
    //    }


    //}
}
