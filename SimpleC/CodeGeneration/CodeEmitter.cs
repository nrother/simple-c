using SimpleC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.CodeGeneration
{
    /// <summary>
    /// Code emitter for SimpleC machine code.
    /// </summary>
    class CodeEmitter
    {
        List<CodeInstruction> emittedCode = new List<CodeInstruction>();

        public void Emit(OpCode code)
        {
            Emit(code, 0, 0);
        }

        public void Emit(OpCode code, byte arg1)
        {
            Emit(code, arg1, 0);
        }

        public void Emit(OpCode code, byte arg1, byte arg2)
        {
            emittedCode.Add(new CodeInstruction() { ByteArg1 = arg1, ByteArg2 = arg2 });
        }

        public void Emit(OpCode code, short arg)
        {
            emittedCode.Add(new CodeInstruction() { ShortArg = arg });
        }

        public CodeInstruction[] GetEmittedCode()
        {
            return emittedCode.ToArray();
        }
    }
}
