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
        Boolean,
        Parameter,
    }
}
