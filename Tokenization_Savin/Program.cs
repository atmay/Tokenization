using System;


namespace Tokenization
{
    static class Program
    {
        private static readonly string[] operators = new[] { "+=", "-=", "*=", "/=", "%=", "++", "--", ">=", "<=", "!=", "-", "=", "+", "-", "*", "/", "%", "<", ">"};
        private static readonly string[] keywords = new[] { "for", "if", "else" };
        private static readonly string[] dataTypes = new[] { "int", "float", "char", "bool" };
        private static readonly string[] delimeters = new[] { ";", "(", ")" , "{", "}"};

        public static void Main(string[] args)
        {

            Tokenizer parser = new Tokenizer();
            parser.AddParameter(TokenType.Operator, operators);
            parser.AddParameter(TokenType.Keyword, keywords);
            parser.AddParameter(TokenType.DataType, dataTypes);
            parser.AddParameter(TokenType.Delimeter, delimeters);

            while (true)
            {
                var command = Console.ReadLine();
                if (command == "exit")
                    break;

                try
                {
                    var tokens = parser.Parse(command);
                    Console.WriteLine($"Список токенов (количество токенов: {tokens.Count}):\n  {string.Join("\n  ", tokens)}");
                    var variables = parser.variableNames;
                    Console.WriteLine($"Список переменных (количество переменных: {variables.Count}):\n  {string.Join("\n  ", variables)}");
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }
            }
        }
    }
}
