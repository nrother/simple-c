using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types
{
    /// <summary>
    /// OpCode in the SimpleC machine.
    /// Uses one byte.
    /// </summary>
    /// <remarks>
    /// Changing a value here WILL break existing
    /// code files. That why there is a explicit value
    /// for every OpCode, so that we do not rely on
    /// the order of the elements.
    /// DO NOT change the value of one of this members
    /// until absoulutly necessary!
    /// </remarks>
    enum OpCode : byte
    {
        LoadC   = 0x10,
        Load    = 0x11,
        LoadA   = 0x12,
        Dup     = 0x13,
        LoadRc  = 0x14,
        LoadR   = 0x15,
        LoadMc  = 0x16,
        LoadM   = 0x17,
        LoadV   = 0x18,
        LoadSc  = 0x19,
        LoadS   = 0x1A,

        Pop     = 0x20,
        Store   = 0x21,
        StoreA  = 0x22,
        StoreR  = 0x23,
        StoreM  = 0x24,

        Jump    = 0x30,
        JumpZ   = 0x31,
        JumpI   = 0x32,

        Add     = 0x40,
        Sub     = 0x41,
        Mul     = 0x42,
        Div     = 0x43,
        Mod     = 0x44,

        Neg     = 0x50,

        Eq      = 0x60,
        Neq     = 0x61,
        Le      = 0x62,
        Leq     = 0x63,
        Gr      = 0x64,
        Geq     = 0x65,

        And     = 0x70,
        Or      = 0x71,
        Not     = 0x72,

        Mark    = 0x80,
        Call    = 0x81,
        Enter   = 0x82,
        Alloc   = 0x83,
        Slide   = 0x84,
        Return  = 0x85,

        New     = 0x90,
        
        Nop     = 0x00,
        Halt    = 0xFF,
    }
}
