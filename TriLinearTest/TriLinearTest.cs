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

            // TEST FUNCTION
            // f = a*x + b*y + c*z
            //
            double a, b, c;
            a = 2.0 ;
            b = 1.0 ;
            c = 0.5 ;

            xsValues[0, 0, 0] = fT[0] * a + mT[0] * b + Dm[0] * c;
            xsValues[0, 0, 1] = fT[0] * a + mT[0] * b + Dm[1] * c;
            xsValues[0, 1, 0] = fT[0] * a + mT[1] * b + Dm[0] * c;
            xsValues[0, 1, 1] = fT[0] * a + mT[1] * b + Dm[1] * c;
            xsValues[1, 0, 0] = fT[1] * a + mT[0] * b + Dm[0] * c;
            xsValues[1, 0, 1] = fT[1] * a + mT[0] * b + Dm[1] * c;
            xsValues[1, 1, 0] = fT[1] * a + mT[1] * b + Dm[0] * c;
            xsValues[1, 1, 1] = fT[1] * a + mT[1] * b + Dm[1] * c;

            // New 2D array to store new - extrapolated values
            double[, ,] newXSs = new Double[1,1,1];
            // new parameters' values
            double[] newFt = new double[1] { 0.1E+1 };
            double[] newMt = new double[1] { 0.1E+1 };
            double[] newDm = new double[1] { 0.15E+1 };

            int interpolationCounter = 0;
            // indexes of the coordinates
            int ix0, ix1, iy0, iy1, iz0, iz1;
            // coordinates' values
            double x0, x1, y0, y1, z0, z1, x2, y2, z2;

            for (int i = 0; i < newFt.Length; i++)
            {
                // cycle on fT to find 2 points to interpolate inbetween
                int[] outputCoeffArray = FindPointsToInterpolate(fT, newFt, i);
                ix0 = outputCoeffArray[0];
                ix1 = outputCoeffArray[1];

                // cycle on Mt to find 2 points to interpolate inbetween
                for (int j = 0; j < newMt.Length; j++)
                {
                    outputCoeffArray = FindPointsToInterpolate(mT, newMt, j);
                    iy0 = outputCoeffArray[0];
                    iy1 = outputCoeffArray[1];

                    // cycle on Dm to find 2 points to interpolate inbetween
                    for (int k = 0; k < newDm.Length; k++)
                    {
                        outputCoeffArray = FindPointsToInterpolate(Dm, newDm, k);
                        iz0 = outputCoeffArray[0];
                        iz1 = outputCoeffArray[1];

                        //Console.WriteLine(" Interp. step {0}", interpolationCounter);

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
                            f00 = xsValues[ix0, iy0, iz0] * (1 - fT_norm) + xsValues[ix1, iy0, iz0] * (fT_norm);
                            f10 = xsValues[ix0, iy1, iz0] * (1 - fT_norm) + xsValues[ix1, iy1, iz0] * (fT_norm);
                            f01 = xsValues[ix0, iy0, iz1] * (1 - fT_norm) + xsValues[ix1, iy0, iz1] * (fT_norm);
                            f11 = xsValues[ix0, iy1, iz1] * (1 - fT_norm) + xsValues[ix1, iy1, iz1] * (fT_norm);

                            // second step of the 3-linear interpolation
                            f0 = f00 * (1 - mT_norm) + f10 * mT_norm;
                            f1 = f01 * (1 - mT_norm) + f11 * mT_norm;

                            Console.WriteLine(" dM_norm = {0}, mT_norm={1}, fT_norm,={2} ", dM_norm, mT_norm, fT_norm);

                            Console.WriteLine(" f00={0:E5}, f10={1:E5}, f01={2:E5}, f11={3:E5} ", f00, f10, f01, f11);

                            Console.WriteLine(" f0={0:E5}, f1={1:E5}", f0, f1);

                            // final step of the 3-linear interpolation
                            newXSs[i, j, k] = f0 * (1 - dM_norm) + f1 * dM_norm;
                        }

                        // printings to verify values and points
                        Console.WriteLine(" Test function is f = x * {0} + y * {1} + z * {2}", a, b, c);
                        double testF = x2 * a + y2 * b + z2 * c;
                        Console.WriteLine(" ");
                        Console.WriteLine(" Coordinates x0={0}, y0={1}, z0={2}  -> Function value is : '{3:E5}' ", x0, y0, z0, xsValues[ix0, iy0, iz0]);
                        Console.WriteLine(" Coordinates x0={0}, y0={1}, z1={2}  -> Function value is : '{3:E5}' ", x0, y0, z1, xsValues[ix0, iy0, iz1]);
                        Console.WriteLine(" Coordinates x0={0}, y1={1}, z0={2}  -> Function value is : '{3:E5}' ", x0, y1, z0, xsValues[ix0, iy1, iz0]);
                        Console.WriteLine(" Coordinates x0={0}, y1={1}, z1={2}  -> Function value is : '{3:E5}' ", x0, y1, z1, xsValues[ix0, iy1, iz1]);
                        Console.WriteLine(" Coordinates x1={0}, y0={1}, z0={2}  -> Function value is : '{3:E5}' ", x1, y0, z0, xsValues[ix1, iy0, iz0]);
                        Console.WriteLine(" Coordinates x1={0}, y0={1}, z1={2}  -> Function value is : '{3:E5}' ", x1, y0, z1, xsValues[ix1, iy0, iz1]);
                        Console.WriteLine(" Coordinates x1={0}, y1={1}, z0={2}  -> Function value is : '{3:E5}' ", x1, y1, z0, xsValues[ix1, iy1, iz0]);
                        Console.WriteLine(" Coordinates x1={0}, y1={1}, z1={2}  -> Function value is : '{3:E5}' ", x1, y1, z1, xsValues[ix1, iy1, iz1]);
                        //Console.WriteLine(" Coordinates {0}, {1}, {2}     -> Interpolated value is: '{3:E5}' ", newFt[i], newMt[j], newDm[k], newXSs[i, j, k]);
                        Console.WriteLine(" ");
                        Console.WriteLine(" Coordinates x2={0}, y2={1}, z2={2}  -> INTERPOLATED VALUE: '{3:E5}' ", x2, y2, z2, newXSs[i, j, k]);
                        Console.WriteLine(" ");
                        Console.WriteLine(" Coordinates x2={0}, y2={1}, z2={2}  -> Calculated   VALUE: '{3:E5}' ", x2, y2, z2, testF );
                        //Console.WriteLine(" Coordinates {0}, {1}, {2}     -> Function value is    : '{3:E5}' ", x1, y1, z1, xsValues[ix1, iy1, iz1]);

                        interpolationCounter++;
                    }
                }
            }
        }

        // Methods used in MAIN

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

        static public double NormalizeDistance(double[] poitsToInterpolate, double[] newPoints, int a0, int a1, int counter)
        {
            double norm;
            if ( Math.Abs(poitsToInterpolate[a1] - poitsToInterpolate[a0]) < 1e-5 )
            {
                norm = 0;
            }
            else
            {
                norm = (newPoints[counter] - poitsToInterpolate[a0]) / (poitsToInterpolate[a1] - poitsToInterpolate[a0]);
            }
            return norm;
        }

    }
}
