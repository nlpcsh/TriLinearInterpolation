namespace PrintState
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Globalization;

    class PrintState
    {
        static void Main(string[] args)
        {
            //// Read the file as a string.
            string[] inputMatrix = { //// "841_diff_1gr.lib", "841_diff_2gr.lib", "841_abso_1gr.lib", "841_abso_2gr.lib", "841_scat_12gr.lib",
                                      "842_diff_1gr.lib", "842_abso_1gr.lib", "842_scat_12gr.lib", "842_diff_2gr.lib",  "842_abso_2gr.lib" //,
                                     //// "843_diff_1gr.lib", "843_diff_2gr.lib", "843_abso_1gr.lib", "843_abso_2gr.lib", "843_scat_12gr.lib"
                                   };

            string s1, s2, s3;
            int state1, state2, state3;

            Console.Write(" Enter state value 1 (1-5): ");
            s1 = Console.ReadLine();
            state1 = Int32.Parse(s1);

            Console.Write(" Enter state value 2 (1-5): ");
            s2 = Console.ReadLine();
            state2 = Int32.Parse(s2);

            Console.Write(" Enter state value 3 (1-6): ");
            s3 = Console.ReadLine();
            state3 = Int32.Parse(s3);

            double[] allXS = new Double[inputMatrix.Length];

            for (int i = 0; i < inputMatrix.Length; i++)
            {
                string inputName = inputMatrix[i];
                string text = File.ReadAllText(@"..\..\Input\" + inputName);

                double[] inputValues = ConvertTextToNumbers(text);

                double[] fT = new double[5] { 0.470000E+03, 0.600000E+03, 0.800000E+03, 0.110000E+04, 0.130000E+04 };
                double[] mT = new double[5] { 0.470000E+03, 0.500000E+03, 0.540000E+03, 0.580000E+03, 0.620000E+03 };
                double[] Dm = new double[6] { 0.500000E+02, 0.100000E+03, 0.3000E+03, 0.6000E+03, 0.74000E+03, 0.885000E+03 };

                double[,,] xsValues = DistributionOfInputValues(fT, mT, Dm, inputValues);

                allXS[i] = xsValues[state1 - 1, state2 - 1, state3 - 1];
                Console.WriteLine(" Current state to print {0}, {1}, {2} - value is: {3} ", state1, state2, state3, allXS[i]);
            }

            using (StreamWriter file = new StreamWriter(@"..\..\Output_state_" + s1 + "_" + s2 + "_" + s3 + ".txt"))
            {
                // Print in file corresponding to the state parameters - XS values
                file.WriteLine(" Gr   Diffusion    Absorption    Scattering  ");
                file.WriteLine(" 1  {0:E5}  {1:E5}  {2:E5} ", allXS[0], allXS[1], allXS[2]);
                file.WriteLine(" 2  {0:E5}  {1:E5} ", allXS[3], allXS[4]);
            }
        }

        static public double[] ConvertTextToNumbers(string text)
        {
            var controlChars = from c in text.ToCharArray() where Char.IsControl(c) select c;
            foreach (char c in controlChars)
            {
                text = text.Replace(c.ToString(), "");
            }

            char delimiterChars = ' ';

            //// array to store readed separate values
            string[] words;
            words = text.Split(delimiterChars);

            //// Console.WriteLine("{0} words in text:", words.Length);

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
                //// convert string to double
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

        static public double[,,] DistributionOfInputValues(double[] fT, double[] mT, double[] Dm, double[] inputValues)
        {
            int l, parametersSum = 0;
            parametersSum = fT.Length + mT.Length + Dm.Length;
            double[] parameters = new double[parametersSum];
            double[,,] xsValues = new double[fT.Length, mT.Length, Dm.Length];

            //// Console.WriteLine(" Sum of the parameters {0} ", parametersSum);

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
                        //Console.WriteLine("Iteration {4} - Value for points Ft {1}, Mt {2} and Dm {3}  is: '{0:E5}' ",
                        //    xsValues[i, j, k], i + 1, j + 1, k + 1, l - 15);
                        l++;
                    }
                }
            }
            return xsValues;
        }
    }
}
