using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.Tokens
{
    class NumberLiteralToken:Token
    {
        public int Number
        {
            get
            {
                return number;
            }
        }
        private int number;

        public NumberLiteralToken(string content)
            :base(content)
        {
            if (!int.TryParse(content, out number))
                throw new ArgumentException("The content is no valid number.", "content");
        }
    }
}
