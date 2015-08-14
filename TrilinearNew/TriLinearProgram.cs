namespace ThreeLinearInterpolation
{
    internal class TriLinearProgram
    {
        public static void Main()
        {
            var inputReader = new InputReader();
            var interpolator = new InterpolateXS();
            var dataInitializer = new DataInitializer();
            var outputToFile = new OutputToFile();

            // Initial Mesh Values
            interpolator.xAxisPoints = dataInitializer.XAxisPoints;
            interpolator.yAxisPoints = dataInitializer.YAxisPoints;
            interpolator.zAxisPoints = dataInitializer.ZAxisPoints;

            // New Mesh Values
            interpolator.xAxisNewPoints = dataInitializer.XAxisNewPoints;
            interpolator.yAxisNewPoints = dataInitializer.YAxisNewPoints;
            interpolator.zAxisNewPoints = dataInitializer.ZAxisNewPoints;

            outputToFile.xAxisNewPoints = dataInitializer.XAxisNewPoints;
            outputToFile.yAxisNewPoints = dataInitializer.YAxisNewPoints;
            outputToFile.zAxisNewPoints = dataInitializer.ZAxisNewPoints;

            System.Console.WriteLine("Data interpolation process in progress...");

            // loop over input data files
            for (int i = 0; i < inputReader.InputFiles.Length; i++)
            {
                dataInitializer.InputDataAsText = inputReader.ReadFromFile(i);
                dataInitializer.ConvertTextToNumbers();
                dataInitializer.DistributionOfInputValues();

                //// initial values to interpolate from
                interpolator.Initial3DValues = dataInitializer.Initial3DValues;

                interpolator.LinearInterpolation();

                //// print real set of XS
                outputToFile.Interpolated3DValues = interpolator.Interpolated3DValues;
                outputToFile.InputDataFileName = inputReader.InputFiles[i];
                outputToFile.PrintTheOutputInFile();

                //// Print mini core XS set
                //// outputToFile.PrintTheOutputInMiniFormatInFile();
            }

            System.Console.WriteLine("End of data interpolation process.");
        }
    }
}
