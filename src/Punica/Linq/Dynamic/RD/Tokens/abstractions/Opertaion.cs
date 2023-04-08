using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.RD.Tokens.abstractions
{
    public abstract class Operation: IOperation
    {
        public virtual bool IsLeftAssociative => true;
        public abstract short Precedence { get; }
        public TokenType TokenType => TokenType.Operator;
        public abstract ExpressionType ExpressionType { get; }

        public abstract Expression Evaluate(Stack<Expression> stack);


        protected static Expression Process(List<IToken> tokens)
        {
            //If there is no tokens
            if (tokens.Count == 0)
            {
               throw new ArgumentException("No tokens");
            }

            // Evaluate the expression using the shunting-yard algorithm
            Stack<IToken> outputQueue = new Stack<IToken>();
            Stack<IToken> operatorStack = new Stack<IToken>();

            foreach (var token in tokens)
            {
                switch (token.TokenType)
                {
                    case TokenType.Operator:
                        // Pop operators from the stack until a lower-precedence or left-associative operator is found
                        while (operatorStack.Count > 0 &&
                               (token.Precedence < operatorStack.Peek().Precedence ||
                               token.Precedence == operatorStack.Peek().Precedence && token.IsLeftAssociative))
                        {
                            outputQueue.Push(operatorStack.Pop());
                        }

                        // Push the new operator onto the stack
                        operatorStack.Push(token);
                        break;

                    case TokenType.OpenParen:
                        // Push left parentheses onto the stack
                        operatorStack.Push(token);
                        break;

                    case TokenType.CloseParen:
                        // Pop operators from the stack and add them to the output queue until a left parenthesis is found
                        while (operatorStack.Count > 0 && operatorStack.Peek().TokenType != TokenType.OpenParen)
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

                    case TokenType.Unknown:
                        // Do nothing 
                        break;

                    case TokenType.Sequence:
                        // Pop any remaining operators from the stack and add them to the output queue

                        throw new ArgumentException("Should not be available");
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
                if (operatorStack.Peek().TokenType == TokenType.OpenParen)
                {
                    throw new ArgumentException("Mismatched parentheses");
                }

                outputQueue.Push(operatorStack.Pop());
            }

            return Pop(outputQueue);
        }

        private static Expression Pop(Stack<IToken> outputQueue)
        {
            Stack<Expression> evaluationStack = new Stack<Expression>();

            if (outputQueue.Count == 1)
            {
                var token = outputQueue.Pop();
                if (token is ValueToken vt)
                {
                    return vt.Value;
                }

                if (token is Operation ot)
                {
                    return ot.Evaluate(evaluationStack);
                }
            }

            var reverse = outputQueue.Reverse();


            foreach (var token in reverse)
            {
                if (token is Operation ot)
                {
                    evaluationStack.Push(ot.Evaluate(evaluationStack)); //TODO sdkslds
                }
                else if (token is ValueToken vt)
                {
                    evaluationStack.Push(vt.Value);
                }
                else
                {
                    throw new ArgumentException("Invalid token");
                }

            }

            outputQueue.Clear();

            return evaluationStack.Pop();
        }
    }
}
