using SimpleC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Excecution
{
    /// <summary>
    /// Excecutor vor SimpleC machine code.
    /// </summary>
    class Excecutor
    {
        private const int DEFAULT_MEMORY_SIZE = 1000; //1k Cells = 4kB Memory

        private int[] memory; //one memory cell is sizeof(int) big
        private int programCounter;
        private int stackPointer;
        private int heapPointer;
        private int framePointer;
        private int extremePointer; //TODO: Needed?
        private CodeInstruction[] code;

        public Excecutor(CodeInstruction[] code)
            : this(code, 0)
        { }

        public Excecutor(CodeInstruction[] code, int memorySize)
        {
            if (memorySize < 0)
                throw new ArgumentOutOfRangeException("Negative memory size given", "memorySize");

            if (memorySize > short.MaxValue)
                throw new ArgumentOutOfRangeException("Memory size to big (16bit addresses!)", "memorySize");

            if (memorySize == 0) //auto
                memorySize = DEFAULT_MEMORY_SIZE;

            memory = new int[memorySize];

            this.code = code;
        }

        /// <summary>
        /// Start the excecution of the code!
        /// </summary>
        public void Start()
        {
            programCounter = 0;
            stackPointer = 0;
            heapPointer = memory.Length - 1;
            framePointer = 0;
            extremePointer = 0;

            CodeInstruction instrunction = code[0];

            while (instrunction.OpCode != OpCode.Halt)
            {
                exceuteInstruction(instrunction);
                programCounter++;
            }
        }

        private void exceuteInstruction(CodeInstruction instr)
        {
            //Genrally: Not much operand checking is done here (as in a real hardware)
            //if some args are wrong, the behavoir is not defined.
            switch (instr.OpCode)
            {
                case OpCode.LoadC:
                    stackPointer++;
                    memory[stackPointer] = instr.ShortArg;
                    break;
                case OpCode.Load:
                    for (int i = instr.ByteArg1 - 1; i >= 0; i--)
                        memory[stackPointer + i] = memory[memory[stackPointer] + i];
                    stackPointer += instr.ByteArg1 - 1;
                    break;
                case OpCode.LoadA:
                    stackPointer++;
                    for (int i = instr.ByteArg2 - 1; i >= 0; i--)
                        memory[stackPointer + i] = memory[instr.ByteArg1 + i];
                    stackPointer += instr.ByteArg2 - 1;
                    break;
                case OpCode.Dup:
                    memory[stackPointer + 1] = memory[stackPointer];
                    stackPointer++;
                    break;
                case OpCode.LoadRc:
                    stackPointer++;
                    memory[stackPointer] = framePointer + instr.ShortArg;
                    break;
                case OpCode.LoadR:
                    stackPointer++;
                    for (int i = instr.ByteArg2 - 1; i >= 0; i--)
                        memory[stackPointer + i] = memory[framePointer + instr.ByteArg1 + i];
                    stackPointer += instr.ByteArg2 - 1;
                    break;
                case OpCode.LoadMc:
                    stackPointer++;
                    memory[stackPointer] = memory[framePointer - 3] + instr.ByteArg1;
                    break;
                case OpCode.LoadM:
                    stackPointer++;
                    for (int i = instr.ByteArg2 - 1; i >= 0; i--)
                        memory[stackPointer + i] = memory[memory[framePointer - 3] + instr.ByteArg1];
                    stackPointer += instr.ByteArg2 - 1;
                    break;
                case OpCode.LoadV:
                    memory[stackPointer + 1] = memory[memory[memory[stackPointer - 2]] + instr.ByteArg1];
                    stackPointer++;
                    break;
                case OpCode.LoadSc:
                    memory[stackPointer + 1] = stackPointer - instr.ByteArg1;
                    stackPointer++;
                    break;
                case OpCode.LoadS:
                    stackPointer++;
                    memory[stackPointer] = memory[stackPointer - instr.ByteArg1];
                    break;
                case OpCode.Pop:
                    stackPointer -= instr.ByteArg1;
                    break;
                case OpCode.Store:
                    for (int i = 0; i < instr.ByteArg1; i++)
                        memory[memory[stackPointer] + i] = memory[stackPointer - instr.ByteArg1 + i];
                    stackPointer--;
                    break;
                case OpCode.StoreA:
                    stackPointer++;
                    for (int i = 0; i < instr.ByteArg2; i++)
                        memory[instr.ByteArg1 + i] = memory[stackPointer - instr.ByteArg2 + i];
                    stackPointer--;
                    break;
                case OpCode.StoreR:
                    stackPointer++;
                    for (int i = 0; i < instr.ByteArg2; i++)
                        memory[framePointer + instr.ByteArg1 + i] = memory[stackPointer - instr.ByteArg2 + i];
                    stackPointer--;
                    break;
                case OpCode.StoreM:
                    stackPointer++;
                    for (int i = 0; i < instr.ByteArg2; i++)
                        memory[memory[framePointer - 3] + instr.ByteArg1 + i] = memory[stackPointer - instr.ByteArg2 + i];
                    stackPointer--;
                    break;
                case OpCode.Jump:
                    programCounter = instr.ShortArg;
                    break;
                case OpCode.JumpZ:
                    if (memory[stackPointer] == 0)
                        programCounter = instr.ShortArg;
                    stackPointer--;
                    break;
                case OpCode.JumpI:
                    programCounter = instr.ShortArg + memory[stackPointer];
                    stackPointer--;
                    break;
                case OpCode.Add:
                    memory[stackPointer - 1] = memory[stackPointer - 1] + memory[stackPointer];
                    stackPointer--;
                    break;
                case OpCode.Sub:
                    memory[stackPointer - 1] = memory[stackPointer - 1] - memory[stackPointer];
                    stackPointer--;
                    break;
                case OpCode.Mul:
                    memory[stackPointer - 1] = memory[stackPointer - 1] * memory[stackPointer];
                    stackPointer--;
                    break;
                case OpCode.Div:
                    memory[stackPointer - 1] = memory[stackPointer - 1] / memory[stackPointer];
                    stackPointer--;
                    break;
                case OpCode.Mod:
                    memory[stackPointer - 1] = memory[stackPointer - 1] % memory[stackPointer];
                    stackPointer--;
                    break;
                case OpCode.Neg:
                    memory[stackPointer] = -memory[stackPointer];
                    break;
                case OpCode.Eq:
                    memory[stackPointer - 1] = (memory[stackPointer - 1] == memory[stackPointer]) ? 1 : 0;
                    stackPointer--;
                    break;
                case OpCode.Neq:
                    memory[stackPointer - 1] = (memory[stackPointer - 1] != memory[stackPointer]) ? 1 : 0;
                    stackPointer--;
                    break;
                case OpCode.Le:
                    memory[stackPointer - 1] = (memory[stackPointer - 1] < memory[stackPointer]) ? 1 : 0;
                    stackPointer--;
                    break;
                case OpCode.Leq:
                    memory[stackPointer - 1] = (memory[stackPointer - 1] <= memory[stackPointer]) ? 1 : 0;
                    stackPointer--;
                    break;
                case OpCode.Gr:
                    memory[stackPointer - 1] = (memory[stackPointer - 1] > memory[stackPointer]) ? 1 : 0;
                    stackPointer--;
                    break;
                case OpCode.Geq:
                    memory[stackPointer - 1] = (memory[stackPointer - 1] >= memory[stackPointer]) ? 1 : 0;
                    stackPointer--;
                    break;
                case OpCode.And:
                    memory[stackPointer - 1] = (memory[stackPointer - 1] != 0 && memory[stackPointer] != 0) ? 1 : 0;
                    stackPointer--;
                    break;
                case OpCode.Or:
                    memory[stackPointer - 1] = (memory[stackPointer - 1] != 0 || memory[stackPointer] != 0) ? 1 : 0;
                    stackPointer--;
                    break;
                case OpCode.Not:
                    memory[stackPointer] = (memory[stackPointer] == 0) ? 1 : 0;
                    break;
                case OpCode.Mark:
                    memory[stackPointer + 1] = extremePointer;
                    memory[stackPointer + 2] = framePointer;
                    stackPointer += 2;
                    break;
                case OpCode.Call:
                    framePointer = stackPointer;
                    int tmp = programCounter;
                    programCounter = memory[stackPointer];
                    memory[stackPointer] = tmp;
                    break;
                case OpCode.Enter:
                    extremePointer = stackPointer + instr.ByteArg1;
                    if (extremePointer >= heapPointer)
                        throw new StackOverflowException();
                    break;
                case OpCode.Alloc:
                    stackPointer += instr.ByteArg1;
                    break;
                case OpCode.Slide:
                    if (instr.ByteArg1 > 0)
                    {
                        if (instr.ByteArg2 == 0)
                            stackPointer -= instr.ByteArg1;
                        else
                        {
                            stackPointer -= instr.ByteArg1 + instr.ByteArg2;
                            for (int i = 0; i < instr.ByteArg2; i++)
                            {
                                stackPointer++;
                                memory[stackPointer] = memory[stackPointer + instr.ByteArg1];
                            }
                        }
                    }
                    break;
                case OpCode.Return:
                    programCounter = memory[framePointer];
                    extremePointer = memory[framePointer - 2];
                    if (extremePointer >= heapPointer)
                        throw new StackOverflowException();
                    stackPointer = framePointer - instr.ByteArg1;
                    framePointer = memory[framePointer - 1];
                    break;
                case OpCode.New:
                    if (heapPointer - memory[stackPointer] > extremePointer)
                    {
                        heapPointer = heapPointer - memory[stackPointer];
                        memory[stackPointer] = heapPointer;
                    }
                    else
                        memory[stackPointer] = 0; //out of memory
                    break;
                case OpCode.Nop:
                    //do nothing...
                    break;
                case OpCode.Halt:
                    //do nothing, will be handled by main excecution cycle
                    break;
                default:
                    throw new InvalidOpCodeException("An unknown opcode was found. OpCode: " + instr.OpCode);
            }
        }
    }
}
