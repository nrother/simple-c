using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types
{
    enum OperatorType
    {
        Add,
        SubstractNegate, //negate would have the same string representation that substract
        Multiply,
        Divide,
        Modulo,
        Assignment,
        Equals,
        GreaterThan,
        LessThan,
        GreaterEquals,
        LessEquals,
        NotEquals,
        Not,
        And,
        Or,
    }
}
