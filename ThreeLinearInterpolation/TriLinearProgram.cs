namespace ThreeLinearInterpolation
{
    internal class TriLinearProgram
    {
        public static void Main()
        {
            var inputReader = new InputReader();
            var dataInitializer = new DataInitializer();
            var interpolator = new TriLinear(
                dataInitializer.XAxisPoints, dataInitializer.YAxisPoints, dataInitializer.ZAxisPoints,
                dataInitializer.XAxisNewPoints, dataInitializer.YAxisNewPoints, dataInitializer.ZAxisNewPoints 
                );
            var outputToFile = new OutputToFile();

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

                interpolator.LinearInterpolationOf3DDataInput();

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
