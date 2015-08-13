namespace ThreeLinearInterpolation
{
    using System.IO;

    internal class InputReader
    {
        // Read the file as a string.
        private string[] inputMatrix = { 
                "841_abso_1gr.lib", "841_abso_2gr.lib", "841_diff_1gr.lib", "841_diff_2gr.lib", "841_scat_12gr.lib",
                "842_abso_1gr.lib", "842_abso_2gr.lib", "842_diff_1gr.lib", "842_diff_2gr.lib", "842_scat_12gr.lib",
                "843_abso_1gr.lib", "843_abso_2gr.lib", "843_diff_1gr.lib", "843_diff_2gr.lib", "843_scat_12gr.lib" 
                                       };

        public string[] InputMatrix 
        {
            get
            { 
                return this.inputMatrix; 
            } 
        }

        internal string ReadFromFile(int inputFileInitializator)
        {
            string inputDataFileName = inputMatrix[inputFileInitializator];
            string text = File.ReadAllText(@"..\..\Input\" + inputDataFileName);

            return text;
        }
    }
}
