using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionDynamicTest.Parsing
{

    [DebuggerDisplay("{Value}")]
    public struct Token
    {
        public string Value { get; }
        public TokenType Type { get; }
        public Operator Operator { get; }

        public Token(string value, TokenType type, Operator @operator)
        {
            Value = value;
            Type = type;
            Operator = @operator;
        }
    }

    public enum TokenType
    {
        Unknown,
        Operator,
        OpenParen,
        CloseParen,
        OpenCurlyParen,
        CloseCurlyParen,
        OpenBracket,
        CloseBracket,
        Member,
        String,
        Number,
        RealNumber,
        Boolean,
        Parameter,
        Sequence,
    }


    public enum Operator
    {
        Unknown,
        Sequence, // ,

        // Assignment Operators
        Assignment, // =
        AdditionAssignment, // +=
        SubtractionAssignment, // -=
        MultiplicationAssignment,
        DivisionAssignment,
        ModuloAssignment,
        BitwiseAndAssignment,
        BitwiseOrAssignment,
        BitwiseXorAssignment,
        LeftShiftAssignment,
        RightShiftAssignment,

        // Conditional Operators
        Question, //?
        Colon, // :

        // Null-coalescing operator
        NullCoalescing, // ??


        // Logical Operators
        Or, // ||
        And, // &&

        // Bitwise Operators
        BitwiseOr, // |
        BitwiseAnd, // &
        BitwiseXor,

        // Equality Operators
        Equal,
        NotEqual,

        // Relational Operators
        GreaterThanEqual,
        LessThan,
        GreaterThan,
        LessThanEqual,

        // Type Operators
        Is,
        As,

        // Collection
        In,

        // Shift Operators
        LeftShift,
        RightShift,

        // Arithmetic Operators
        Plus,
        Minus,
        Multiply,
        Divide,
        Modulo,

        // Unary Operators
        Tide,
        Not,
        Increment,
        Decrement,

        // Grouping Operators
        OpenParen,
        CloseParen,

        OpenCurlyParen,
        CloseCurlyParen,

        OpenBracket,
        CloseBracket,

        // New Operator
        New,

        // Other Operator
        Dot,

        Method,
    }
}
