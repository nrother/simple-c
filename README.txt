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

Licence
-------
MIT License