﻿
using Punica.Linq.Dynamic.RD.Tokens.abstractions;
using Punica.Linq.Dynamic.RD.Tokens;
using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.RD.Rd2
{
    public class MethodContext3
    {
        private const string _arg = "_arg";
        private readonly Dictionary<int, List<ParameterToken>> _parameters = new Dictionary<int, List<ParameterToken>>();
        private int _depth = 0;

        public int Depth => _depth;

        public MethodContext3()
        {
        }

        public MethodContext3(ParameterExpression parameter) : this(new ParameterToken(parameter))
        {
        }

        public MethodContext3(ParameterToken parameter)
        {
            AddParameter(parameter);
        }

        public ParameterToken? GetParameter(string name)
        {
            foreach (var key in _parameters.Keys)
            {
                var para = _parameters[key].FirstOrDefault(p => p.Name == name);

                if (para != null)
                {
                    return para; //if there is lambda this should populated before this method get invoked
                }
            }

         

            return null;
        }

        public ParameterToken? AddOrGetParameter()
        {
            if (_depth == 0)
            {
                return _parameters[0].First(p => p.Name == _arg); //should 
            }

            if (!_parameters.ContainsKey(_depth))
            {
                return AddParameter(new ParameterToken(_arg + _depth));
            }
            else
            {
                return GetParameter();
            }
            return null;
        }

        public ParameterToken? GetParameter()
        {
            return _parameters[_depth].FirstOrDefault();
        }

        public List<ParameterToken> GetParameters()
        {
            if (_parameters.ContainsKey(_depth))
            {
                return _parameters[_depth];
            }

            return new List<ParameterToken>();
        }

        public void NextDepth()
        {
            _depth++;
        }

        public void PreviousDepth()
        {
            _parameters.Remove(_depth);
            _depth--;
        }

        public ParameterToken[] MoveToNextArgument()
        {
            if (_parameters.ContainsKey(_depth))
            {
                var parameterTokens = _parameters[_depth].ToArray();
                _parameters[_depth].Clear();
               return parameterTokens;
            }

            return Array.Empty<ParameterToken>();
        }

        public void AddParameter(IExpression expression)
        {
            AddParameter(new ParameterToken(expression, _arg + _depth));
        }

        public ParameterToken AddParameter(ParameterToken parameter)
        {
            foreach (var key in _parameters.Keys)
            {
                if (key == _depth)
                {
                    continue;
                }

                var para = _parameters[key].FirstOrDefault(p => p.Name == parameter.Name);

                if (para != null)
                {
                    throw new Exception("Parameter with name " + parameter.Name + " already exists");
                }
            }

            if (!_parameters.ContainsKey(_depth))
            {
                _parameters[_depth] = new List<ParameterToken>();
            }

            _parameters[_depth].Add(parameter);

            return parameter;
        }

        public void AddParameters(IReadOnlyList<string> paraNames)
        {
            foreach (var name in paraNames)
            {
                AddParameter(new ParameterToken(name));
            }
        }
    }
}
