namespace ThreeLinearInterpolation
{
    using System;
    using System.Linq;
    using System.Globalization;

    internal class DataInitializer
    {
        private double[] xAxisPoints = new double[5] { 0.4700000E+03, 0.6000000E+03, 0.8000000E+03, 0.1100000E+04, 0.1300000E+04 };
        private double[] yAxisPoints = new double[5] { 0.470000E+03, 0.500000E+03, 0.540000E+03, 0.580000E+03, 0.620000E+03 };
        private double[] zAxisPoints = new double[6] { 0.500000E+02, 0.100000E+03, 0.3000E+03, 0.6000E+03, 0.74000E+03, 0.885000E+03 };

        // new parameters' values
        private double[] xAxisNewPoints = new double[5] { 0.470000E+03, 0.852500E+03, 0.123500E+04, 0.161750E+04, 0.200000E+04 };
        private double[] yAxisNewPoints = new double[3] { 0.470000E+03, 0.545000E+03, 0.620000E+03 };
        private double[] zAxisNewPoints = new double[13] 
        { 
            0.500000E+02, 0.760000E+02, 0.102000E+03, 0.128000E+03, 0.154000E+03, 0.207000E+03, 0.259000E+03,
            0.311000E+03, 0.363000E+03, 0.467500E+03, 0.572000E+03, 0.676000E+03, 0.885000E+03
        };

        public DataInitializer()
        {
            this.Initial3DValues = new double[this.xAxisPoints.Length, this.yAxisPoints.Length, this.zAxisPoints.Length];
        }

        public double[] XAxisPoints
        { 
            get 
            { 
                return this.xAxisPoints; 
            } 
        }

        public double[] YAxisPoints
        {
            get
            {
                return this.yAxisPoints;
            }
        }

        public double[] ZAxisPoints
        {
            get
            {
                return this.zAxisPoints;
            }
        }

        public double[] XAxisNewPoints
        {
            get
            {
                return this.xAxisNewPoints;
            }
        }

        public double[] YAxisNewPoints
        {
            get
            {
                return this.yAxisNewPoints;
            }
        }

        public double[] ZAxisNewPoints
        {
            get
            {
                return this.zAxisNewPoints;
            }
        }

        public double[, ,] Initial3DValues { get; set; }

        public double[] InputValues { get; set; }

        public string InputDataAsText { get; set; }

        internal void ConvertTextToNumbers()
        {
            var controlChars = from c in this.InputDataAsText.ToCharArray() where char.IsControl(c) select c;
            foreach (char c in controlChars)
            {
                this.InputDataAsText = this.InputDataAsText.Replace(c.ToString(), string.Empty);
            }

            char delimiterChars = ' ';

            //// array to store readed separate values
            string[] words;
            words = this.InputDataAsText.Split(delimiterChars);

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
            parametersSum = this.xAxisPoints.Length + this.yAxisPoints.Length + this.zAxisPoints.Length;
            double[] parameters = new double[parametersSum];

            Console.WriteLine(" Sum of the parameters {0} ", parametersSum);

            for (l = 0; l < parametersSum; l++)
            {
                parameters[l] = this.InputValues[l];
            }

            l = parametersSum;
            for (int k = 0; k < this.zAxisPoints.Length; k++)
            {
                for (int j = 0; j < this.yAxisPoints.Length; j++)
                {
                    for (int i = 0; i < this.xAxisPoints.Length; i++)
                    {
                        this.Initial3DValues[i, j, k] = this.InputValues[l];
                        Console.WriteLine("Iteration {4} - Value for points Ft {1}, Mt {2} and Dm {3}  is: '{0:E5}' ",
                            this.Initial3DValues[i, j, k], i + 1, j + 1, k + 1, l - 15);
                        l++;
                    }
                }
            }
        }
    }
}
