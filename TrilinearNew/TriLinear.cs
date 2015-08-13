namespace ThreeLinearInterpolation
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    internal class InterpolateXS
    {
        // Initial mesh of parameters' values
        private double[] fT = new double[5] { 0.4700000E+03, 0.6000000E+03, 0.8000000E+03, 0.1100000E+04, 0.1300000E+04 };
        private double[] mT = new double[5] { 0.470000E+03, 0.500000E+03, 0.540000E+03, 0.580000E+03, 0.620000E+03 };
        private double[] dM = new double[6] { 0.500000E+02, 0.100000E+03, 0.3000E+03, 0.6000E+03, 0.74000E+03, 0.885000E+03 };

        // new parameters' values
        private double[] newFt = new double[5] { 0.470000E+03, 0.852500E+03, 0.123500E+04, 0.161750E+04, 0.200000E+04 };
        private double[] newMt = new double[3] { 0.470000E+03, 0.545000E+03, 0.620000E+03 };
        private double[] newDm = new double[13] 
        { 
            0.500000E+02, 0.760000E+02, 0.102000E+03, 0.128000E+03, 0.154000E+03, 0.207000E+03, 0.259000E+03,
            0.311000E+03, 0.363000E+03, 0.467500E+03, 0.572000E+03, 0.676000E+03, 0.885000E+03
        };

        private double[,,] xsValues;
        private double[,,] newXSs;

        private string inputText;
        private double[] inputValues;
        private string inputDataFileName;

        public InterpolateXS()
        {
            this.newXSs = new double[this.newFt.Length, this.newMt.Length, this.newDm.Length];
            this.xsValues = new double[this.fT.Length, this.mT.Length, this.dM.Length];
        }

        public InterpolateXS(string inputText, string inputDataFileName)
            : this()
        {
            this.InputText = inputText;
            this.inputDataFileName = inputDataFileName;
        }

        public string InputText
        {
            get
            {
                return this.inputText;
            }

            set
            {
                this.inputText = value;
            }
        }

        public double[] InputValues
        {
            get
            {
                return this.inputValues;
            }

            set
            {
                this.inputValues = value;
            }
        }

        public string InputDataFileName
        {
            get
            {
                return this.inputDataFileName;
            }

            set
            {
                this.inputDataFileName = value;
            }
        }

        public void LinearInterpolation()
        {
            //// initialize variable: InputValues from InputText
            this.ConvertTextToNumbers();

            //// initialize variable: xsValues
            this.DistributionOfInputValues();

            Console.WriteLine(" Start to dictribute values: ");

            int interpolationCounter = 0;
            //// indexes of the coordinates
            int ix0, ix1, iy0, iy1, iz0, iz1;
            //// coordinates' values
            double x0, x1, y0, y1, z0, z1, x2, y2, z2;

            for (int i = 0; i < this.newFt.Length; i++)
            {
                //// process fT to find 2 points to interpolate inbetween
                int[] outputCoeffArray = this.FindPointsToInterpolate(this.fT, this.newFt, i);
                ix0 = outputCoeffArray[0];
                ix1 = outputCoeffArray[1];

                for (int j = 0; j < this.newMt.Length; j++)
                {
                    //// process Mt to find 2 points to interpolate inbetween
                    outputCoeffArray = this.FindPointsToInterpolate(this.mT, this.newMt, j);
                    iy0 = outputCoeffArray[0];
                    iy1 = outputCoeffArray[1];

                    for (int k = 0; k < this.newDm.Length; k++)
                    {
                        //// process Dm to find 2 points to interpolate inbetween
                        outputCoeffArray = this.FindPointsToInterpolate(this.dM, this.newDm, k);
                        iz0 = outputCoeffArray[0];
                        iz1 = outputCoeffArray[1];

                        Console.WriteLine(" Interp. step {0}", interpolationCounter);

                        //// points to interpolate from:
                        x0 = this.fT[ix0];
                        y0 = this.mT[iy0];
                        z0 = this.dM[iz0];

                        x1 = this.fT[ix1];
                        y1 = this.mT[iy1];
                        z1 = this.dM[iz1];

                        //// current points:
                        x2 = this.newFt[i];
                        y2 = this.newMt[j];
                        z2 = this.newDm[k];

                        //// FIRST VARIANT OF INTERPOLATION
                        this.FirstInterpolationVariant(ix0, ix1, iy0, iy1, iz0, iz1, x0, x1, y0, y1, z0, z1, x2, y2, z2, i, j, k);

                        //// SECOND VARIANT TO INTERPOLATE
                        //// this.SecondInterpolationVariant(ix0, ix1, iy0, iy1, iz0, iz1, i, j, k);

                        //// printings to verify values and points
                        this.PrintingOfPointsAndValues(ix0, ix1, iy0, iy1, iz0, iz1, x0, x1, y0, y1, z0, z1, x2, y2, z2, i, j, k);

                        interpolationCounter++;
                    }
                }
            }

            //// print real set of XS
            this.PrintTheOutputInFile();

            //// Print mini core XS set
            //// this.PrintTheOutputInMiniFormatInFile();
        }

        private void PrintTheOutputInMiniFormatInFile()
        {
            using (StreamWriter file = new StreamWriter(@"..\..\Output\Mini_Interpolated_" + this.InputDataFileName))
            {
                int m = 1;
                int[] i_mini_Ft = { 0, 1 };
                int[] i_mini_Mt = { 1, 2 };
                int[] i_mini_Dm = { 11, 12 };
                //// Print the state parameters points
                foreach (int index in i_mini_Ft)
                {
                    //// add new line each 5-th element 
                    if (m % 5 == 0)
                    {
                        file.WriteLine(" {0:E5} ", this.newFt[index]);
                    }
                    else
                    {
                        file.Write(" {0:E5} ", this.newFt[index]);
                    }

                    m++;
                }

                foreach (int index in i_mini_Mt)
                {
                    //// add new line each 5-th element 
                    if (m % 5 == 0)
                    {
                        file.WriteLine(" {0:E5} ", this.newMt[index]);
                    }
                    else
                    {
                        file.Write(" {0:E5} ", this.newMt[index]);
                    }

                    m++;
                }

                foreach (int index in i_mini_Dm)
                {
                    //// add new line each 5-th element 
                    if (m % 5 == 0)
                    {
                        file.WriteLine(" {0:E5} ", this.newDm[index]);
                    }
                    else
                    {
                        file.Write(" {0:E5} ", this.newDm[index]);
                    }

                    m++;
                }

                //// printings of the new interpolated XS values...
                for (int i = 0; i < i_mini_Dm.Length; i++)
                {
                    for (int j = 0; j < i_mini_Mt.Length; j++)
                    {
                        for (int k = 0; k < i_mini_Ft.Length; k++)
                        {
                            if (m % 5 == 0)
                            {
                                file.WriteLine(" {0:E5} ", this.newXSs[i_mini_Ft[k], i_mini_Mt[j], i_mini_Dm[i]]);
                            }
                            else
                            {
                                file.Write(" {0:E5} ", this.newXSs[i_mini_Ft[k], i_mini_Mt[j], i_mini_Dm[i]]);
                            }

                            m++;
                        }
                    }
                }
            }
        }

        private void PrintTheOutputInFile()
        {
            using (StreamWriter file = new StreamWriter(@"..\..\Output\Interpolated_" + this.InputDataFileName))
            {
                int m = 1;

                // Print the state parameters points
                foreach (double digit in this.newFt)
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

                foreach (double digit in this.newMt)
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

                foreach (double digit in this.newDm)
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
                for (int i = 0; i < this.newDm.Length; i++)
                {
                    for (int j = 0; j < this.newMt.Length; j++)
                    {
                        for (int k = 0; k < this.newFt.Length; k++)
                        {
                            if (m % 5 == 0)
                            {
                                file.WriteLine(" {0:E5} ", this.newXSs[k, j, i]);
                            }
                            else
                            {
                                file.Write(" {0:E5} ", this.newXSs[k, j, i]);
                            }

                            m++;
                        }
                    }
                }
            }
        }

        private void PrintingOfPointsAndValues(int ix0, int ix1, int iy0, int iy1, int iz0, int iz1, double x0, double x1, double y0, double y1, double z0, double z1, double x2, double y2, double z2, int i, int j, int k)
        {
            Console.WriteLine(" Coordinates x0={0}, y0={1}, z0={2}  -> Function value is : '{3:E5}' ", x0, y0, z0, this.xsValues[ix0, iy0, iz0]);
            Console.WriteLine(" Coordinates x0={0}, y0={1}, z1={2}  -> Function value is : '{3:E5}' ", x0, y0, z1, this.xsValues[ix0, iy0, iz1]);
            Console.WriteLine(" Coordinates x0={0}, y1={1}, z0={2}  -> Function value is : '{3:E5}' ", x0, y1, z0, this.xsValues[ix0, iy1, iz0]);
            Console.WriteLine(" Coordinates x0={0}, y1={1}, z1={2}  -> Function value is : '{3:E5}' ", x0, y1, z1, this.xsValues[ix0, iy1, iz1]);
            Console.WriteLine(" Coordinates x1={0}, y0={1}, z0={2}  -> Function value is : '{3:E5}' ", x1, y0, z0, this.xsValues[ix1, iy0, iz0]);
            Console.WriteLine(" Coordinates x1={0}, y0={1}, z1={2}  -> Function value is : '{3:E5}' ", x1, y0, z1, this.xsValues[ix1, iy0, iz1]);
            Console.WriteLine(" Coordinates x1={0}, y1={1}, z0={2}  -> Function value is : '{3:E5}' ", x1, y1, z0, this.xsValues[ix1, iy1, iz0]);
            Console.WriteLine(" Coordinates x1={0}, y1={1}, z1={2}  -> Function value is : '{3:E5}' ", x1, y1, z1, this.xsValues[ix1, iy1, iz1]);
            Console.WriteLine(" Coordinates x2={0}, y2={1}, z2={2}  -> INTERPOLATED VALUE: '{3:E5}' ", x2, y2, z2, this.newXSs[i, j, k]);
        }

        private void SecondInterpolationVariant(int ix0, int ix1, int iy0, int iy1, int iz0, int iz1, int i, int j, int k)
        {
            double fT_norm, mT_norm, dM_norm, f00, f10, f01, f11, f0, f1;
            //// set the coordinates of the new interpolated value
            fT_norm = this.NormalizeDistance(this.fT, this.newFt, ix0, ix1, i);
            mT_norm = this.NormalizeDistance(this.mT, this.newMt, iy0, iy1, j);
            dM_norm = this.NormalizeDistance(this.dM, this.newDm, iz0, iz1, k);

            //// Interpolation block ....
            //// first step of the 3-linear interpolation
            f00 = (this.xsValues[ix0, iy0, iz0] * Math.Abs(1 - fT_norm)) + (this.xsValues[ix1, iy0, iz0] * fT_norm);
            f10 = (this.xsValues[ix0, iy1, iz0] * Math.Abs(1 - fT_norm)) + (this.xsValues[ix1, iy1, iz0] * fT_norm);
            f01 = (this.xsValues[ix0, iy0, iz1] * Math.Abs(1 - fT_norm)) + (this.xsValues[ix1, iy0, iz1] * fT_norm);
            f11 = (this.xsValues[ix0, iy1, iz1] * Math.Abs(1 - fT_norm)) + (this.xsValues[ix1, iy1, iz1] * fT_norm);

            //// second step of the 3-linear interpolation
            f0 = (f00 * Math.Abs(1 - mT_norm)) + (f10 * mT_norm);
            f1 = (f01 * Math.Abs(1 - mT_norm)) + (f11 * mT_norm);

            Console.WriteLine(" dM_norm = {0}, mT_norm={1}, fT_norm,={2} ", dM_norm, mT_norm, fT_norm);

            Console.WriteLine(" f00={0:E5}, f10={1:E5}, f01={2:E5}, f11={3:E5} ", f00, f10, f01, f11);

            Console.WriteLine(" f0={0:E5}, f1={1:E5}", f0, f1);

            //// final step of the 3-linear interpolation
            this.newXSs[i, j, k] = (f0 * Math.Abs(1 - dM_norm)) + (f1 * dM_norm);
        }

        private void FirstInterpolationVariant(int ix0, int ix1, int iy0, int iy1, int iz0, int iz1, double x0, double x1, double y0, double y1, double z0, double z1, double x2, double y2, double z2, int i, int j, int k)
        {
            //// Normalized volumes:
            double Na, Nb, Nc, Nd, Ne, Nf, Ng, Nh;
            //// current summation
            double x12, x20, y12, y20, z12, z20, total;
            total = (x1 - x0) * (y1 - y0) * (z1 - z0);

            x12 = x1 - x2;
            x20 = x2 - x0;
            y12 = y1 - y2;
            y20 = y2 - y0;
            z12 = z1 - z2;
            z20 = z2 - z0;

            Na = (x12 * y12 * z20) / total;
            Nb = (x12 * y20 * z20) / total;

            Nc = (x20 * y12 * z20) / total;
            Nd = (x20 * y20 * z20) / total;

            Ne = (x12 * y12 * z12) / total;
            Nf = (x12 * y20 * z12) / total;

            Ng = (x20 * y12 * z12) / total;
            Nh = (x20 * y20 * z12) / total;

            ////    f8           f0                              f1
            this.newXSs[i, j, k] = (this.xsValues[ix0, iy0, iz1] * Na) + (this.xsValues[ix0, iy1, iz1] * Nb) +
                ////                 f2                              f3
                              (this.xsValues[ix1, iy0, iz1] * Nc) + (this.xsValues[ix1, iy1, iz1] * Nd) +
                ////                 f4                              f5
                              (this.xsValues[ix0, iy0, iz0] * Ne) + (this.xsValues[ix0, iy1, iz0] * Nf) +
                ////                 f6                              f7
                              (this.xsValues[ix1, iy0, iz0] * Ng) + (this.xsValues[ix1, iy1, iz0] * Nh);
        }

        private void ConvertTextToNumbers()
        {
            var controlChars = from c in this.InputText.ToCharArray() where char.IsControl(c) select c;
            foreach (char c in controlChars)
            {
                this.InputText = this.InputText.Replace(c.ToString(), string.Empty);
            }

            char delimiterChars = ' ';

            //// array to store readed separate values
            string[] words;
            words = this.InputText.Split(delimiterChars);

            Console.WriteLine("{0} words in text:", words.Length);

            foreach (string s in words)
            {
                Console.WriteLine(s);
            }

            NumberStyles styles;
            styles = NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint;

            this.InputValues = new double[words.Length];

            int i = 0;
            foreach (string s in words)
            {
                //// convert string to double
                if (double.TryParse(s, styles, CultureInfo.InvariantCulture, out this.InputValues[i]))
                {
                    i++;
                }
                else
                {
                    Console.WriteLine("Unable to convert '{0}'.", s);
                }
            }
        }

        private void DistributionOfInputValues()
        {
            int l, parametersSum = 0;
            parametersSum = this.fT.Length + this.mT.Length + this.dM.Length;
            double[] parameters = new double[parametersSum];

            Console.WriteLine(" Sum of the parameters {0} ", parametersSum);

            for (l = 0; l < parametersSum; l++)
            {
                parameters[l] = this.InputValues[l];
            }

            l = parametersSum;
            for (int k = 0; k < this.dM.Length; k++)
            {
                for (int j = 0; j < this.mT.Length; j++)
                {
                    for (int i = 0; i < this.fT.Length; i++)
                    {
                        this.xsValues[i, j, k] = this.InputValues[l];
                        Console.WriteLine("Iteration {4} - Value for points Ft {1}, Mt {2} and Dm {3}  is: '{0:E5}' ",
                            this.xsValues[i, j, k], i + 1, j + 1, k + 1, l - 15);
                        l++;
                    }
                }
            }
        }

        private int[] FindPointsToInterpolate(double[] poitsToInterpolate, double[] newPoints, int counter)
        {
            int[] coordinates = new int[2];
            double delta = 0;

            //// searching for points before and after the desired point to interpolate
            for (int h = 0; h < poitsToInterpolate.Length; h++)
            {
                delta = poitsToInterpolate[h] - newPoints[counter];
                //// if delta == zero:
                if (Math.Abs(delta) < 1e-5)
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

        private double NormalizeDistance(double[] poitsToInterpolate, double[] newPoints, int a0, int a1, int counter)
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
    }
}