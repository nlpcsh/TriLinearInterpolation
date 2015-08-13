namespace ThreeLinearInterpolation
{
    internal class TriLinearProgram
    {
        public static void Main()
        {
            var inputReader = new InputReader();
            var interpolator = new InterpolateXS();
            var dataInitializator = new DataInitializer();
            var outputToFile = new OutputToFile();

            // Initial Mesh Values
            interpolator.fT = dataInitializator.FT;
            interpolator.mT = dataInitializator.MT;
            interpolator.dM = dataInitializator.DM;

            // New Mesh Values
            interpolator.newFt = dataInitializator.NewFT;
            interpolator.newMt = dataInitializator.NewMT;
            interpolator.newDm = dataInitializator.NewDM;

            outputToFile.newFt = dataInitializator.NewFT;
            outputToFile.newMt = dataInitializator.NewMT;
            outputToFile.newDm = dataInitializator.NewDM;

            // loop over input data files
            for (int i = 0; i < inputReader.InputFilesMatrix.Length; i++)
            {
                dataInitializator.InputText = inputReader.ReadFromFile(i);
                dataInitializator.ConvertTextToNumbers();
                dataInitializator.DistributionOfInputValues();

                //// initial values to interpolate from
                interpolator.xsValues = dataInitializator.xsValues;

                interpolator.LinearInterpolation();

                //// print real set of XS
                outputToFile.newXSs = interpolator.newXSs;
                outputToFile.InputDataFileName = inputReader.InputFilesMatrix[i];
                outputToFile.PrintTheOutputInFile();

                //// Print mini core XS set
                //// outputToFile.PrintTheOutputInMiniFormatInFile();
            }
        }
    }
}
