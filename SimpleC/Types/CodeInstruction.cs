using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SimpleC.Types
{
    /// <summary>
    /// Single instruction in the SimpleC machine code.
    /// Consists of one eight bit opcode and either a 16 bit
    /// argument or two eight bit arguments.
    /// This is a struct, using LayoutKind.Explicit!
    /// </summary>
    /// <remarks>
    /// Convention:
    /// When two args are needed, both have to be bytes.
    /// If the only arg is a memory address it is a short
    /// (to allow addressing the complete memory), when it
    /// is a counter/amount, it is a byte.
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    struct CodeInstruction
    {
        [FieldOffset(0)]
        public OpCode OpCode;
        [FieldOffset(1)]
        public byte ByteArg1;
        [FieldOffset(2)]
        public byte ByteArg2;
        [FieldOffset(1)] //overlay over ByteArg1/2
        public short ShortArg;
    }
}
