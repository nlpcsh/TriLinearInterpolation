namespace ThreeLinearInterpolation
{
    using System.IO;

    internal class OutputToFile
    {
        private string inputDataFileName;

        // new parameters' values
        public double[] xAxisNewPoints { get; set; }

        public double[] yAxisNewPoints { get; set; }

        public double[] zAxisNewPoints { get; set; }

        // new interpolated values
        public double[, ,] Interpolated3DValues { get; set; }

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

        internal void PrintTheOutputInMiniFormatInFile()
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
                        file.WriteLine(" {0:E5} ", this.xAxisNewPoints[index]);
                    }
                    else
                    {
                        file.Write(" {0:E5} ", this.xAxisNewPoints[index]);
                    }

                    m++;
                }

                foreach (int index in i_mini_Mt)
                {
                    //// add new line each 5-th element 
                    if (m % 5 == 0)
                    {
                        file.WriteLine(" {0:E5} ", this.yAxisNewPoints[index]);
                    }
                    else
                    {
                        file.Write(" {0:E5} ", this.yAxisNewPoints[index]);
                    }

                    m++;
                }

                foreach (int index in i_mini_Dm)
                {
                    //// add new line each 5-th element 
                    if (m % 5 == 0)
                    {
                        file.WriteLine(" {0:E5} ", this.zAxisNewPoints[index]);
                    }
                    else
                    {
                        file.Write(" {0:E5} ", this.zAxisNewPoints[index]);
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
                                file.WriteLine(" {0:E5} ", this.Interpolated3DValues[i_mini_Ft[k], i_mini_Mt[j], i_mini_Dm[i]]);
                            }
                            else
                            {
                                file.Write(" {0:E5} ", this.Interpolated3DValues[i_mini_Ft[k], i_mini_Mt[j], i_mini_Dm[i]]);
                            }

                            m++;
                        }
                    }
                }
            }
        }

        internal void PrintTheOutputInFile()
        {
            using (StreamWriter file = new StreamWriter(@"..\..\Output\Interpolated_" + this.InputDataFileName))
            {
                int m = 1;

                // Print the state parameters points
                foreach (double digit in this.xAxisNewPoints)
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

                foreach (double digit in this.yAxisNewPoints)
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

                foreach (double digit in this.zAxisNewPoints)
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
                for (int i = 0; i < this.zAxisNewPoints.Length; i++)
                {
                    for (int j = 0; j < this.yAxisNewPoints.Length; j++)
                    {
                        for (int k = 0; k < this.xAxisNewPoints.Length; k++)
                        {
                            if (m % 5 == 0)
                            {
                                file.WriteLine(" {0:E5} ", this.Interpolated3DValues[k, j, i]);
                            }
                            else
                            {
                                file.Write(" {0:E5} ", this.Interpolated3DValues[k, j, i]);
                            }

                            m++;
                        }
                    }
                }
            }
        }
    }
}
