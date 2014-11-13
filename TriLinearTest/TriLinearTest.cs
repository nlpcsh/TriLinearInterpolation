namespace ThreeLinearInterpolationTEST
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;

    class InterpolateXS
    {
        static public void Main()
        {

            // Initial mesh of parameters' values
            double[] fT = new double[2] { 0.1E+1 , 0.2E+1  };
            double[] mT = new double[2] { 0.1E+1 , 0.2E+1  };
            double[] Dm = new double[2] { 0.1E+1 , 0.2E+1  };

            // Test points of squared mesh between 1,1,1 and 2,2,2 points
            double[, ,] xsValues = new double[2,2,2] ;

            xsValues[0, 0, 0] = 1.732050808;
            xsValues[0, 0, 1] = 2.449490;
            xsValues[0, 1, 0] = 2.449490;
            xsValues[0, 1, 1] = 3.0;
            xsValues[1, 0, 0] = 2.449490;
            xsValues[1, 0, 1] = 3.0;
            xsValues[1, 1, 0] = 3.0;
            xsValues[1, 1, 1] = 3.464101615;

            // New 2D array to store new - extrapolated values
            double[, ,] newXSs = new Double[2, 2, 2];
            // new parameters' values
            double[] newFt = new double[2] { 0.1E+1, 0.15E+1 };
            double[] newMt = new double[2] { 0.1E+1, 0.15E+1 };
            double[] newDm = new double[2] { 0.1E+1, 0.3E+1 };

            int interpolationCounter = 0;
            int x0, x1, y0, y1, z0, z1;

            for (int i = 0; i < newFt.Length; i++)
            {
                // cycle on fT to find 2 points to interpolate inbetween
                int[] outputCoeffArray = FindPointsToInterpolate(fT, newFt, i);
                x0 = outputCoeffArray[0];
                x1 = outputCoeffArray[1];

                // cycle on Mt to find 2 points to interpolate inbetween
                for (int j = 0; j < newMt.Length; j++)
                {
                    outputCoeffArray = FindPointsToInterpolate(mT, newMt, j);
                    y0 = outputCoeffArray[0];
                    y1 = outputCoeffArray[1];

                    // cycle on Dm to find 2 points to interpolate inbetween
                    for (int k = 0; k < newDm.Length; k++)
                    {
                        outputCoeffArray = FindPointsToInterpolate(Dm, newDm, k);
                        z0 = outputCoeffArray[0];
                        z1 = outputCoeffArray[1];

                        Console.WriteLine(" Interp. step {0}", interpolationCounter);

                        // set the coordinates of the new interpolated value
                        double fT_norm = CheckForZero(fT, newFt, x0, x1, i);
                        double mT_norm = CheckForZero(mT, newMt, y0, y1, j);
                        double dM_norm = CheckForZero(Dm, newDm, z0, z1, k);

                        // Interpolation block ....
                        // first step of the 3-linear interpolation
                        double f00 = xsValues[x0, y0, z0] * (1 - dM_norm) + xsValues[x1, y0, z0] * (dM_norm);
                        double f10 = xsValues[x0, y1, z0] * (1 - dM_norm) + xsValues[x1, y1, z0] * (dM_norm);
                        double f01 = xsValues[x0, y0, z1] * (1 - dM_norm) + xsValues[x1, y0, z1] * (dM_norm);
                        double f11 = xsValues[x0, y1, z1] * (1 - dM_norm) + xsValues[x1, y1, z1] * (dM_norm);

                        // second step of the 3-linear interpolation
                        double f0 = f00 * (1 - mT_norm) + f10 * mT_norm;
                        double f1 = f01 * (1 - mT_norm) + f11 * mT_norm;

                        // final step of the 3-linear interpolation
                        newXSs[i, j, k] = f0 * (1 - fT_norm) + f1 * fT_norm;

                        // printings to verify values and points
                        Console.WriteLine(" Coordinates {0}, {1}, {2}     -> Function value is    : '{3:E5}' ", fT[x0], mT[y0], Dm[z0], xsValues[x0, y0, z0]);
                        Console.WriteLine(" Coordinates {0}, {1}, {2}     -> Interpolated value is: '{3:E5}' ", newFt[i], newMt[j], newDm[k], newXSs[i, j, k]);
                        Console.WriteLine(" Coordinates {0}, {1}, {2}     -> Function value is    : '{3:E5}' ", fT[x1], mT[y1], Dm[z1], xsValues[x1, y1, z1]);

                        interpolationCounter++;

                    }
                }
            }
        }

        // Methods used in MAIN

        static public double[] ConvertTextToNumbers(string text)
        {
            //
            var controlChars = from c in text.ToCharArray() where Char.IsControl(c) select c;
            foreach (char c in controlChars)
            {
                text = text.Replace(c.ToString(), "");
            }

            char delimiterChars = ' ';

            // array to store readed separate values
            string[] words;
            words = text.Split(delimiterChars);

            Console.WriteLine("{0} words in text:", words.Length);

            foreach (string s in words)
            {
                Console.WriteLine(s);
            }

            NumberStyles styles;
            styles = NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint;
            double[] inputValues = new Double[words.Length];

            int i = 0;
            foreach (string s in words)
            {
                // convert string to double
                if (Double.TryParse(s, styles, CultureInfo.InvariantCulture, out inputValues[i]))
                {
                    i++;
                }
                else
                {
                    Console.WriteLine("Unable to convert '{0}'.", s);
                }
            }
            return inputValues;
        }

        static public double[, ,] DistributionOfValues(double[] fT, double[] mT, double[] Dm, double[] inputValues)
        {
            int l, parametersSum = 0;
            parametersSum = fT.Length + mT.Length + Dm.Length;
            double[] parameters = new double[parametersSum];
            double[, ,] xsValues = new double[fT.Length, mT.Length, Dm.Length];


            Console.WriteLine(" Sum of the parameters {0} ", parametersSum);

            for (l = 0; l < parametersSum; l++)
            {
                parameters[l] = inputValues[l];
            }

            l = parametersSum;
            for (int i = 0; i < fT.Length; i++)
            {
                for (int j = 0; j < mT.Length; j++)
                {
                    for (int k = 0; k < Dm.Length; k++)
                    {
                        xsValues[i, j, k] = inputValues[l];
                        Console.WriteLine("Iteration {4} - Value for points Ft {1}, Mt {2} and Dm {3}  is: '{0:E5}' ",
                            xsValues[i, j, k], i + 1, j + 1, k + 1, l - 15);
                        l++;
                    }
                }
            }
            return xsValues;
        }

        static public int[] FindPointsToInterpolate(double[] poitsToInterpolate, double[] newPoints, int counter)
        {
            int[] coordinates = new int[2];
            double delta = 0;

            // searching for points before and after the desired point to interpolate
            for (int h = 0; h < poitsToInterpolate.Length; h++)
            {
                delta = poitsToInterpolate[h] - newPoints[counter];
                if (delta == 0)
                {
                    if (h == 0)
                    {
                        coordinates[0] = h;
                        coordinates[1] = h + 1;
                    }
                    else
                    {
                        coordinates[0] = h - 1;
                        coordinates[1] = h ;
                    }
                    break;
                }
                else if (delta > 0)
                {
                    coordinates[0] = h - 1;
                    coordinates[1] = h;
                    break;
                }
                else if ((h == poitsToInterpolate.Length - 1) && (delta < 0))
                {
                    coordinates[0] = h - 1;
                    coordinates[1] = h;
                }
            }

            return coordinates;
        }

        static public double CheckForZero(double[] poitsToInterpolate, double[] newPoints, int a0, int a1, int counter)
        {
            double fT_norm;
            if ((poitsToInterpolate[a1] - poitsToInterpolate[a0]) == 0)
            {
                fT_norm = 0;
            }
            else
            {
                fT_norm = (newPoints[counter] - poitsToInterpolate[a0]) / (poitsToInterpolate[a1] - poitsToInterpolate[a0]);
            }
            return fT_norm;
        }

    }
}
