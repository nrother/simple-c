SimpleC
=======

This is a very simple C compiler written in C# by Niklas Rother.

Inspiration for this project came from the lecture "Programmiersprachen
und Übersetzer" at the Leibniz University Hannover with I visit this
semester.

The main design of the compiler follows the examples given in the lecture.
Most important the compiled code is no machine-code, but code for the a
virtual machine (VM). There is an interpreter for this code included.

The compiler compiles a small subset of the C syntax. Important exception:
There is no need to declare functions bevore using them, so no header files :)

Current state
-------------
The project is currently in an early state:
* The tokenizer is complete and works
* The parser is in a early state, currently under heavly development
* The code generator is not even started
* The interpreter is no even started

The parser is probably the most complicated part of the project. I know that there
are things like yacc/Bison that can generate parsers, but opted for doing it myself,
I find this more entertaining :)

The code generator should be doable (the lectures script has a nice chapter about it),
and the interpreter should be really easy...

Licence
-------
MIT License