
namespace ThreeLinearInterpolation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;

    internal class DataInitializer
    {
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

        public double[, ,] xsValues { get; set; }
        //private double[,,] newXSs;

        public DataInitializer()
        {
            //this.newXSs = new double[this.newFt.Length, this.newMt.Length, this.newDm.Length];
            this.xsValues = new double[this.fT.Length, this.mT.Length, this.dM.Length];
        }

        public double[] FT
        { 
            get 
            { 
                return this.fT; 
            } 
        }

        public double[] MT
        {
            get
            {
                return this.mT;
            }
        }

        public double[] DM
        {
            get
            {
                return this.dM;
            }
        }

        public double[] NewFT
        {
            get
            {
                return this.newFt;
            }
        }

        public double[] NewMT
        {
            get
            {
                return this.newMt;
            }
        }

        public double[] NewDM
        {
            get
            {
                return this.newDm;
            }
        }

        public double[] InputValues { get; set; }

        public string InputText { get; set; }

        internal void ConvertTextToNumbers()
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

        internal void DistributionOfInputValues()
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
    }
}
