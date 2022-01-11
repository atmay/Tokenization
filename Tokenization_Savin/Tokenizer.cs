using System;
using System.Collections.Generic;
using System.Linq;

namespace Tokenization
{
    public class Tokenizer
    {
        private string _expr;
        private int _pos;

        public HashSet<string> variableNames = new HashSet<string>();
        private Dictionary<TokenType, string[]> typeDefinitions = new Dictionary<TokenType, string[]>();

        public void AddParameter(TokenType type, string[] definition)
        {
            typeDefinitions[type] = definition;
        }

        public List<Token> Parse(string expr)
        {
            _expr = expr;
            _pos = 0;

            var tokens = new List<Token>();

            string value;

            while (!IsEnd())
            {
                bool isAdded = false;

                foreach (var pair in typeDefinitions)
                {
                    value = TryMatchAny(pair.Value);

                    if (value != null)
                    {
                        tokens.Add(new Token(pair.Key, value));
                        isAdded = true;
                        break;
                    }
                }

                if (isAdded)
                {
                    continue;
                }

                value = TryGetNumber();
                if (value != null)
                {
                    tokens.Add(new Token(TokenType.Number, value));
                    continue;
                }

                value = TryGetSymbol();
                if (value != null)
                {
                    variableNames.Add(value);
                    tokens.Add(new Token(TokenType.Variable, value));
                    continue;
                }

                throw new Exception($"Не удалось распарсить строку целиком. " +
                                    $"Проверьте корректность ввода в позиции {_pos}!");
            }

            return tokens;
        }

        #region Base abstracts

        public bool TryMatch(string symb)
        {
            SkipWhitespaces();
            if (_expr.IndexOf(symb, _pos) == _pos)
            {
                _pos += symb.Length;
                return true;
            }
            return false;
        }

        public string TryMatchAny(params string[] symbols)
        {
            SkipWhitespaces();
            foreach (string symb in symbols)
                if (TryMatch(symb))
                    return symb;
            return null;
        }

        public bool Match(string symb)
        {
            SkipWhitespaces();
            if (!TryMatch(symb))
            {
                throw new Exception($"Неверный символ {symb.Substring(_pos, symb.Length)} на позиции {_pos}! " +
                    $"Ожидаемый символ: {symb}");
            }
            return true;
        }

        public string MatchAny(params string[] symbols)
        {
            SkipWhitespaces();
            string symb = TryMatchAny(symbols);
            if (symb == null)
            {
                throw new Exception($"Неверный символ {symb.Substring(_pos, symb.Length)} на позиции {_pos}! " +
                    $"Ожидаемые символы: {string.Join(", ", symbols)}");
            }
            return symb;
        }

        public void SkipWhitespaces()
        {
            string symbToSkip = " \f\v\r\t\n";
            while (!IsEnd() && symbToSkip.Contains(_expr[_pos]))
                _pos++;
        }

        public bool IsEnd()
        {
            return _expr.Length <= _pos;
        }

        public string TryGetNumber()
        {
            string number = "";
            while (!IsEnd() && Char.IsDigit(_expr[_pos]))
            {
                number += _expr[_pos];
                _pos++;
            }
            return number.Length == 0 ? null : number;
        }

        public string TryGetSymbol()
        {
            
            string variableName = "";
            while (!IsEnd() && Char.IsLetterOrDigit(_expr[_pos]))
            {
                variableName += _expr[_pos];
                _pos++;
            }
            return variableName.Length == 0 ? null : variableName;
        }
        #endregion
    }
}
