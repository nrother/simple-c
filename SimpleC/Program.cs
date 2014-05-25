using SimpleC.Lexing;
using SimpleC.Parsing;
using SimpleC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = @"
int a = 5;

int main()
{
    a = 6;
    return a*2;
}";

            //lexing

            var lexer = new Tokenizer(code);
            var tokens = lexer.Tokenize();

            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }

            Console.WriteLine();
            Console.WriteLine();

            //parsing
            var parser = new Parser(tokens);
            var ast = parser.ParseToAst();

            //TODO: Dump tree

            Console.ReadKey(false);
        }
    }
}
