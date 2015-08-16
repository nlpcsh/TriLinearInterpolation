"TriLinearInterpolation" 
------------------------------------------------------

1.  The C# program that can interpolate and extrapolate linearly functions depending on 3 parameters (x, y, z). Initial mesh supposed to be given and interpolation is happening between points of that mesh.

2.  Originally this code was used to reorder large data files.

3.  Structure of the code:
    - class `TriLinearProgram` - contains Main() method 
    - class `TriLinear` - interpolates / extrapolates linearly by given initial mesh values, function valies in that mesh points and the new mesh points
    - class `InputReader` - reads the input data form external files
    - class `DataInitializer` - places all data in proper variables that can be used from `TriLinear` class
    - class `OutputToFile` - is responsible for printing output values in external files.

4.  Unit test are included in this project.


Interpolation algorithm was taken from:
http://en.wikipedia.org/wiki/Trilinear_interpolation

