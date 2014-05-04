using SimpleC.Lexing;
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
            string code = @"5+7;";

            var lexer = new Tokenizer(code);
            var tokens = lexer.Tokenize();

            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }

            Console.ReadKey(false);
        }
    }
}
