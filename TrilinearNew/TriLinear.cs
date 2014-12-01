namespace ThreeLinearInterpolation
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

            // Read the file as a string.
            string[] inputMatrix = { "841_abso_1gr.lib", "841_abso_2gr.lib", "841_diff_1gr.lib", "841_diff_2gr.lib", "841_scat_12gr.lib",
                                     "842_abso_1gr.lib", "842_abso_2gr.lib", "842_diff_1gr.lib", "842_diff_2gr.lib", "842_scat_12gr.lib",
                                     "843_abso_1gr.lib", "843_abso_2gr.lib", "843_diff_1gr.lib", "843_diff_2gr.lib", "843_scat_12gr.lib" };
            //string[] inputMatrix = { "841_diff_2gr.lib" };

            for (int i = 0; i < inputMatrix.Length; i++)
            {
                string inputName = inputMatrix[i];
                string text = File.ReadAllText(@"..\..\Input\" + inputName);

                LinearInterpolation(text, inputName);
            }
 
        }

        // Methods used in MAIN

        static public void LinearInterpolation(string text, string inputName)
        {

            // Display the file contents by using a foreach loop.
            // Console.WriteLine("Contents of input file = {0}", text);

            double[] inputValues = ConvertTextToNumbers(text);

            Console.WriteLine(" Start to dictribute values: ");

            // Initial mesh of parameters' values
            double[] fT = new double[5] { 0.4700000E+03, 0.6000000E+03, 0.8000000E+03, 0.1100000E+04, 0.1300000E+04 };
            double[] mT = new double[5] { 0.470000E+03, 0.500000E+03, 0.540000E+03, 0.580000E+03, 0.620000E+03 };
            double[] Dm = new double[6] { 0.500000E+02, 0.100000E+03, 0.3000E+03, 0.6000E+03, 0.74000E+03, 0.885000E+03 };

            double[, ,] xsValues = DistributionOfInputValues(fT, mT, Dm, inputValues);

            // New 2D array to store new - extrapolated values
            double[, ,] newXSs = new Double[5, 3, 13];
            // new parameters' values
            double[] newFt = new double[5] { 0.470000E+03, 0.852500E+03, 0.123500E+04, 0.161750E+04, 0.200000E+04 };
            double[] newMt = new double[3] { 0.470000E+03, 0.545000E+03, 0.620000E+03 };
            double[] newDm = new double[13] { 0.500000E+02, 0.760000E+02, 0.102000E+03, 0.128000E+03, 0.154000E+03, 0.207000E+03, 0.259000E+03,
                                  0.311000E+03, 0.363000E+03, 0.467500E+03, 0.572000E+03, 0.676000E+03, 0.885000E+03};
            //double[, ,] newXSs = new Double[1,1,1];
            //double[] newFt = new double[1] { 0.1100000E+04 };
            //double[] newMt = new double[1] { 0.580000E+03 };
            //double[] newDm = new double[1] { 0.67000E+03 };

            int interpolationCounter = 0;
            // indexes of the coordinates
            int ix0, ix1, iy0, iy1, iz0, iz1;
            // coordinates' values
            double x0, x1, y0, y1, z0, z1, x2, y2, z2;

            for (int i = 0; i < newFt.Length; i++)
            {
                // process fT to find 2 points to interpolate inbetween
                int[] outputCoeffArray = FindPointsToInterpolate(fT, newFt, i);
                ix0 = outputCoeffArray[0];
                ix1 = outputCoeffArray[1];

                for (int j = 0; j < newMt.Length; j++)
                {
                    // process Mt to find 2 points to interpolate inbetween
                    outputCoeffArray = FindPointsToInterpolate(mT, newMt, j);
                    iy0 = outputCoeffArray[0];
                    iy1 = outputCoeffArray[1];

                    for (int k = 0; k < newDm.Length; k++)
                    {
                        // process Dm to find 2 points to interpolate inbetween
                        outputCoeffArray = FindPointsToInterpolate(Dm, newDm, k);
                        iz0 = outputCoeffArray[0];
                        iz1 = outputCoeffArray[1];

                        Console.WriteLine(" Interp. step {0}", interpolationCounter);

                        // points to interpolate from:
                        x0 = fT[ix0];
                        y0 = mT[iy0];
                        z0 = Dm[iz0];

                        x1 = fT[ix1];
                        y1 = mT[iy1];
                        z1 = Dm[iz1];

                        // current points:
                        x2 = newFt[i];
                        y2 = newMt[j];
                        z2 = newDm[k];

                        if (true)
                        //if (false)
                        {
                            // FIRST VARIANT OF INTERPOLATION

                            // Normalized volumes:
                            double Na, Nb, Nc, Nd, Ne, Nf, Ng, Nh;
                            // current summation
                            double x12, x20, y12, y20, z12, z20, total;
                            total = (x1 - x0) * (y1 - y0) * (z1 - z0);

                            x12 = (x1 - x2);
                            x20 = (x2 - x0);
                            y12 = (y1 - y2);
                            y20 = (y2 - y0);
                            z12 = (z1 - z2);
                            z20 = (z2 - z0);

                            Na = (x12 * y12 * z20) / total;
                            Nb = (x12 * y20 * z20) / total;

                            Nc = (x20 * y12 * z20) / total;
                            Nd = (x20 * y20 * z20) / total;

                            Ne = (x12 * y12 * z12) / total;
                            Nf = (x12 * y20 * z12) / total;

                            Ng = (x20 * y12 * z12) / total;
                            Nh = (x20 * y20 * z12) / total;

                            //    f8           f0                              f1
                            newXSs[i, j, k] = xsValues[ix0, iy0, iz1] * Na + xsValues[ix0, iy1, iz1] * Nb +
                                //                 f2                              f3
                                              xsValues[ix1, iy0, iz1] * Nc + xsValues[ix1, iy1, iz1] * Nd +
                                //                 f4                              f5
                                              xsValues[ix0, iy0, iz0] * Ne + xsValues[ix0, iy1, iz0] * Nf +
                                //                 f6                              f7
                                              xsValues[ix1, iy0, iz0] * Ng + xsValues[ix1, iy1, iz0] * Nh;
                        }
                        else
                        {
                            // SECOND VARIANT TO INTERPOLATE
                            double fT_norm, mT_norm, dM_norm, f00, f10, f01, f11, f0, f1;
                            // set the coordinates of the new interpolated value
                            fT_norm = NormalizeDistance(fT, newFt, ix0, ix1, i);
                            mT_norm = NormalizeDistance(mT, newMt, iy0, iy1, j);
                            dM_norm = NormalizeDistance(Dm, newDm, iz0, iz1, k);

                            // Interpolation block ....
                            // first step of the 3-linear interpolation
                            f00 = xsValues[ix0, iy0, iz0] * Math.Abs(1 - fT_norm) + xsValues[ix1, iy0, iz0] * (fT_norm);
                            f10 = xsValues[ix0, iy1, iz0] * Math.Abs(1 - fT_norm) + xsValues[ix1, iy1, iz0] * (fT_norm);
                            f01 = xsValues[ix0, iy0, iz1] * Math.Abs(1 - fT_norm) + xsValues[ix1, iy0, iz1] * (fT_norm);
                            f11 = xsValues[ix0, iy1, iz1] * Math.Abs(1 - fT_norm) + xsValues[ix1, iy1, iz1] * (fT_norm);

                            // second step of the 3-linear interpolation
                            f0 = f00 * Math.Abs(1 - mT_norm) + f10 * mT_norm;
                            f1 = f01 * Math.Abs(1 - mT_norm) + f11 * mT_norm;

                            Console.WriteLine(" dM_norm = {0}, mT_norm={1}, fT_norm,={2} ", dM_norm, mT_norm, fT_norm);

                            Console.WriteLine(" f00={0:E5}, f10={1:E5}, f01={2:E5}, f11={3:E5} ", f00, f10, f01, f11);

                            Console.WriteLine(" f0={0:E5}, f1={1:E5}", f0, f1);

                            // final step of the 3-linear interpolation
                            newXSs[i, j, k] = f0 * Math.Abs(1 - dM_norm) + f1 * dM_norm;
                        }

                        // printings to verify values and points
                        Console.WriteLine(" Coordinates x0={0}, y0={1}, z0={2}  -> Function value is : '{3:E5}' ", x0, y0, z0, xsValues[ix0, iy0, iz0]);
                        Console.WriteLine(" Coordinates x0={0}, y0={1}, z1={2}  -> Function value is : '{3:E5}' ", x0, y0, z1, xsValues[ix0, iy0, iz1]);
                        Console.WriteLine(" Coordinates x0={0}, y1={1}, z0={2}  -> Function value is : '{3:E5}' ", x0, y1, z0, xsValues[ix0, iy1, iz0]);
                        Console.WriteLine(" Coordinates x0={0}, y1={1}, z1={2}  -> Function value is : '{3:E5}' ", x0, y1, z1, xsValues[ix0, iy1, iz1]);
                        Console.WriteLine(" Coordinates x1={0}, y0={1}, z0={2}  -> Function value is : '{3:E5}' ", x1, y0, z0, xsValues[ix1, iy0, iz0]);
                        Console.WriteLine(" Coordinates x1={0}, y0={1}, z1={2}  -> Function value is : '{3:E5}' ", x1, y0, z1, xsValues[ix1, iy0, iz1]);
                        Console.WriteLine(" Coordinates x1={0}, y1={1}, z0={2}  -> Function value is : '{3:E5}' ", x1, y1, z0, xsValues[ix1, iy1, iz0]);
                        Console.WriteLine(" Coordinates x1={0}, y1={1}, z1={2}  -> Function value is : '{3:E5}' ", x1, y1, z1, xsValues[ix1, iy1, iz1]);
                        //Console.WriteLine(" Coordinates {0}, {1}, {2}     -> Interpolated value is: '{3:E5}' ", newFt[i], newMt[j], newDm[k], newXSs[i, j, k]);
                        Console.WriteLine(" Coordinates x2={0}, y2={1}, z2={2}  -> INTERPOLATED VALUE: '{3:E5}' ", x2, y2, z2, newXSs[i, j, k]);
                        //Console.WriteLine(" Coordinates {0}, {1}, {2}     -> Function value is    : '{3:E5}' ", x1, y1, z1, xsValues[ix1, iy1, iz1]);

                        interpolationCounter++ ;

                    }
                }
            }

            //PrintInterpolatedValues(newFt, newMt, newDm, newXSs, inputName);
            using (StreamWriter file = new StreamWriter(@"..\..\Output\Interpolated_" + inputName))
            {
                int m = 1;

                // Print the state parameters points
                foreach (double digit in newFt)
                {
                    // add new line each 5-th element 
                    if (m % 5 == 0)
                    {
                        file.WriteLine(" {0:E5} ", digit);
                    }
                    else
                    {
                        file.Write(" {0:E5} ", digit);
                    }
                    m++;
                }
                foreach (double digit in newMt)
                {
                    // add new line each 5-th element 
                    if (m % 5 == 0)
                    {
                        file.WriteLine(" {0:E5} ", digit);
                    }
                    else
                    {
                        file.Write(" {0:E5} ", digit);
                    }
                    m++;
                }
                foreach (double digit in newDm)
                {
                    // add new line each 5-th element 
                    if (m % 5 == 0)
                    {
                        file.WriteLine(" {0:E5} ", digit);
                    }
                    else
                    {
                        file.Write(" {0:E5} ", digit);
                    }
                    m++;
                }

                // printings of the new interpolated XS values...
                for (int i = 0; i < newDm.Length; i++)
                {
                    for (int j = 0; j < newMt.Length; j++)
                    {
                        for (int k = 0; k < newFt.Length; k++)
                        {
                            if (m % 5 == 0)
                            {
                                file.WriteLine(" {0:E5} ", newXSs[k, j, i]);
                            }
                            else
                            {
                                file.Write(" {0:E5} ", newXSs[k, j, i]);
                            }
                            m++;
                        }
                    }
                }
            }
        }

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

        static public double[, ,] DistributionOfInputValues(double[] fT, double[] mT, double[] Dm, double[] inputValues)
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
            for (int k = 0; k < Dm.Length; k++)
            {
                for (int j = 0; j < mT.Length; j++)
                {
                    for (int i = 0; i < fT.Length; i++)
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
                // if delta == zero:
                if ( Math.Abs(delta) < 1e-5 )
                {
                    if (h == 0)
                    {
                        coordinates[0] = h;
                        coordinates[1] = h + 1;
                    }
                    else
                    {
                        coordinates[0] = h - 1;
                        coordinates[1] = h;
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

        static public double NormalizeDistance(double[] poitsToInterpolate, double[] newPoints, int a0, int a1, int counter)
        {
            double norm;
            if (Math.Abs(poitsToInterpolate[a1] - poitsToInterpolate[a0]) < 1e-5)
            {
                Console.WriteLine(" Warning! Points to interpolate are the same!!!! ");
                norm = 0;
            }
            else
            {
                norm = (newPoints[counter] - poitsToInterpolate[a0]) / (poitsToInterpolate[a1] - poitsToInterpolate[a0]);
            }
            return norm;
        }

        //static public void PrintInterpolatedValues(double[] newFt, double[] newMt, double[] newDm, double[, ,] newXSs, string inputName);
        //{
        //    using (StreamWriter file = new StreamWriter(@"..\Interpolated_" + inputName))
        //    {
        //        int m = 1;
        //        // Print the state parameters points
        //        foreach (double digit in newFt)
        //        {
        //            // add new line each 5-th element 
        //            if (m % 5 == 0)
        //            {
        //                file.WriteLine(" {0:E5} ", digit);
        //            }
        //            else
        //            {
        //                file.Write(" {0:E5} ", digit);
        //            }
        //            m++;
        //        }
        //        foreach (double digit in newMt)
        //        {
        //            // add new line each 5-th element 
        //            if (m % 5 == 0)
        //            {
        //                file.WriteLine(" {0:E5} ", digit);
        //            }
        //            else
        //            {
        //                file.Write(" {0:E5} ", digit);
        //            }
        //            m++;
        //        }
        //        foreach (double digit in newDm)
        //        {
        //            // add new line each 5-th element 
        //            if (m % 5 == 0)
        //            {
        //                file.WriteLine(" {0:E5} ", digit);
        //            }
        //            else
        //            {
        //                file.Write(" {0:E5} ", digit);
        //            }
        //            m++;
        //        }

        //        // printings of the new interpolated XS values...
        //        for (int i = 0; i < newDm.Length; i++)
        //        {
        //            for (int j = 0; j < newMt.Length; j++)
        //            {
        //                for (int k = 0; k < newFt.Length; k++)
        //                {
        //                    if (m % 5 == 0)
        //                    {
        //                        file.WriteLine(" {0:E5} ", newXSs[k, j, i]);
        //                    }
        //                    else
        //                    {
        //                        file.Write(" {0:E5} ", newXSs[k, j, i]);
        //                    }
        //                    m++;
        //                }
        //            }
        //        }
        //    }
        //}

        //static int PrintingValues(double newPoints, int m, StreamWriter file)
        //{
        //    foreach (int digit in newPoints)
        //    {
        //        // add new line each 5-th element 
        //        if (m % 5 == 0)
        //        {
        //            file.WriteLine(" {0:E5} ", digit);
        //        }
        //        else
        //        {
        //            file.Write(" {0:E5} ", digit);
        //        }
        //        m++;
        //    }
        //    return m;
        //}

    }
}