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
(see below)

Current state
-------------
The project is currently in an early state:
* The tokenizer is complete and works
* The parser is in a early state, currently under heavly development
* The code generator is not even started
* The interpreter is no even started

Currently the parser seems to be the most complicated thing of the whole project.
I know that normally yacc/Bison is used to generate the parser, but I'll try to
implement it on my own (note that this is my first try in writing a parser...).

The code generator should be doable (the lectures script has a nice chapter about it),
and the interpreter should be really easy...

Limitations
-----------
The following limitations are currently active, to simplify the whole thing:
* No nested variable scopes in if/while/etc. That means, you can not override a name
  of a variable inside a loop etc. There are only two scopes: Global or function-local
* You may use functions before declaring them (no header files!). This may not be a limitation...
* Variables may be used before declared (must must be declared somewhere). This means you should
  declare all local vairables inside a funtion on top of it, otherwise the compiler will behave as
  if you did. Maybe this will emit a error later.

Licence
-------
MIT License