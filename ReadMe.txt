“TriLinearInterpolation” 

The C# program that can interpolate and extrapolate linearly functions depending on 3 parameters (x, y, z). Initial mesh supposed to be given and interpolation is happening between points of that mesh.

Originally this code was used to reorder large data files.

Structure of the code:
    `TriLinearProgram` - Main() containing class
    `TriLinear` - class that interpolates / extrapolates linearly by given initial mesh values, function valies in that mesh points and the new mesh points
    `InputReader` - reads the input data form external files
    `DataInitializer` - places all data in proper variables that can be used from `TriLinear` class
    `OutputToFile` - is responsible for printing output values in external files.

Unit test are included in this project.


Interpolation algorithm was taken from:
http://en.wikipedia.org/wiki/Trilinear_interpolation

