namespace ThreeLinearInterpolation
{
    //using System;
    using System.IO;

    internal class InputReader
    {
        // Read the file as a string.
        private string[] inputFilesMatrix = 
        { 
            "841_abso_1gr.lib", "841_abso_2gr.lib", "841_diff_1gr.lib", "841_diff_2gr.lib", "841_scat_12gr.lib",
            "842_abso_1gr.lib", "842_abso_2gr.lib", "842_diff_1gr.lib", "842_diff_2gr.lib", "842_scat_12gr.lib",
            "843_abso_1gr.lib", "843_abso_2gr.lib", "843_diff_1gr.lib", "843_diff_2gr.lib", "843_scat_12gr.lib" 
        };

        public string[] InputFilesMatrix
        {
            get
            {
                return this.inputFilesMatrix;
            }
        }

        public string InputText { get; set; }

        internal string ReadFromFile(int inputFileInitializator)
        {
            string inputDataFileName = this.InputFilesMatrix[inputFileInitializator];
            this.InputText = File.ReadAllText(@"..\..\Input\" + inputDataFileName);

            return this.InputText;
        }
    }
}
