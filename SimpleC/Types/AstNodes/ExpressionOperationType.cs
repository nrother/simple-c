using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    //we need the enum additionally to OperatorType to support the parser,
    //which must decide between unary and binary minus and support function
    //calls, which are no "operators" in the token's sense.
    enum ExpressionOperationType
    {
        Add,
        Substract,
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
        Negate,
        FunctionCall,
        OpenBrace,
    }
}
