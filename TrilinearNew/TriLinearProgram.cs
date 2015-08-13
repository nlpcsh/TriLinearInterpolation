﻿namespace ThreeLinearInterpolation
{
    class TriLinearProgram
    {
        static public void Main()
        {
            var inputReader = new InputReader();
            var interpolator = new InterpolateXS();

            for (int i = 0; i < inputReader.InputMatrix.Length; i++)
            {
                interpolator.InputText = inputReader.ReadFromFile(i);
                interpolator.InputDataFileName = inputReader.InputMatrix[i];
                interpolator.LinearInterpolation();
            }
        }
    }
}
