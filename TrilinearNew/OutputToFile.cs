namespace ThreeLinearInterpolation
{
    using System.IO;

    internal class OutputToFile
    {
        private string inputDataFileName;

        // new parameters' values
        public double[] newFt { get; set; }

        public double[] newMt { get; set; }

        public double[] newDm { get; set; }

        // new interpolated values
        public double[, ,] newXSs { get; set; }

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

        internal void PrintTheOutputInFile()
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
    }
}
