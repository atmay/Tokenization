

namespace Tokenization
{
    public enum TokenType
    {
        Operator = 1,
        Number = 2,
        Keyword = 3,
        DataType = 4,
        Variable = 5,
        Delimeter = 6

    }

    public class Token
    {
        public TokenType type;
        public string value;

        public Token(TokenType t, string v)
        {
            type = t;
            value = v;
        }

        public override string ToString()
        {
            return $"({type}, '{value}')";
        }
    }
}
