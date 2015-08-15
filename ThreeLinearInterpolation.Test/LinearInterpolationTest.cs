namespace ThreeLinearInterpolation.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ThreeLinearInterpolation;


    [TestClass]
    public class LinearInterpolationTest
    {
        [TestMethod]
        public void XDirection_Only_SholdBeInterpolated()
        {
            double[] XAxisPoints = new double[2] { 0, 1 };
            double[] YAxisPoints = new double[2] { 0, 1 };
            double[] ZAxisPoints = new double[2] { 0, 1 };

            double[] XAxisNewPoints = new double[2] { 0, 0.5 };
            double[] YAxisNewPoints = new double[2] { 0, 0 };
            double[] ZAxisNewPoints = new double[2] { 0, 0 };

            var interpolator = new TriLinear(
                XAxisPoints, YAxisPoints, ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            double[, ,] Initial3DValues = new double[XAxisPoints.Length, YAxisPoints.Length, ZAxisPoints.Length];

            Initial3DValues[0, 0, 0] = 0;
            Initial3DValues[1, 1, 1] = Math.Sqrt(2 + 1);

            Initial3DValues[1, 0, 0] = 1;
            Initial3DValues[1, 0, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[1, 1, 0] = Math.Sqrt(1 + 1);

            Initial3DValues[0, 1, 0] = 1;
            Initial3DValues[0, 1, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[0, 0, 1] = 1;

            interpolator.Initial3DValues = Initial3DValues;

            interpolator.LinearInterpolation();

            Assert.AreEqual(0.5, interpolator.Interpolated3DValues[1, 1, 1], "Problem with interpolated point in X direction!");
        }

        [TestMethod]
        public void YDirection_Only_SholdBeInterpolated()
        {
            double[] XAxisPoints = new double[2] { 0, 1 };
            double[] YAxisPoints = new double[2] { 0, 1 };
            double[] ZAxisPoints = new double[2] { 0, 1 };

            double[] XAxisNewPoints = new double[2] { 0, 0 };
            double[] YAxisNewPoints = new double[2] { 0, 0.5 };
            double[] ZAxisNewPoints = new double[2] { 0, 0 };

            var interpolator = new TriLinear(
                XAxisPoints, YAxisPoints, ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            double[, ,] Initial3DValues = new double[XAxisPoints.Length, YAxisPoints.Length, ZAxisPoints.Length];

            Initial3DValues[0, 0, 0] = 0;
            Initial3DValues[1, 1, 1] = Math.Sqrt(2 + 1);

            Initial3DValues[1, 0, 0] = 1;
            Initial3DValues[1, 0, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[1, 1, 0] = Math.Sqrt(1 + 1);

            Initial3DValues[0, 1, 0] = 1;
            Initial3DValues[0, 1, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[0, 0, 1] = 1;

            interpolator.Initial3DValues = Initial3DValues;

            interpolator.LinearInterpolation();

            Assert.AreEqual(0.5, interpolator.Interpolated3DValues[1, 1, 1], "Problem with interpolated point in Y direction!");
        }

        [TestMethod]
        public void ZDirection_Only_SholdBeInterpolated()
        {
            double[] XAxisPoints = new double[2] { 0, 1 };
            double[] YAxisPoints = new double[2] { 0, 1 };
            double[] ZAxisPoints = new double[2] { 0, 1 };

            double[] XAxisNewPoints = new double[2] { 0, 0 };
            double[] YAxisNewPoints = new double[2] { 0, 0 };
            double[] ZAxisNewPoints = new double[2] { 0, 0.5 };

            var interpolator = new TriLinear(
                XAxisPoints, YAxisPoints, ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            double[, ,] Initial3DValues = new double[XAxisPoints.Length, YAxisPoints.Length, ZAxisPoints.Length];

            Initial3DValues[0, 0, 0] = 0;
            Initial3DValues[1, 1, 1] = Math.Sqrt(2 + 1);

            Initial3DValues[1, 0, 0] = 1;
            Initial3DValues[1, 0, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[1, 1, 0] = Math.Sqrt(1 + 1);

            Initial3DValues[0, 1, 0] = 1;
            Initial3DValues[0, 1, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[0, 0, 1] = 1;

            interpolator.Initial3DValues = Initial3DValues;

            interpolator.LinearInterpolation();

            Assert.AreEqual(0.5, interpolator.Interpolated3DValues[1, 1, 1], "Problem with interpolated point in Z direction!");
        }

        [TestMethod]
        public void XYDirection_Only_SholdBeInterpolated()
        {
            double[] XAxisPoints = new double[2] { 0, 1 };
            double[] YAxisPoints = new double[2] { 0, 1 };
            double[] ZAxisPoints = new double[2] { 0, 1 };

            double[] XAxisNewPoints = new double[2] { 0, 0.5 };
            double[] YAxisNewPoints = new double[2] { 0, 0.5 };
            double[] ZAxisNewPoints = new double[2] { 0, 0 };

            var interpolator = new TriLinear(
                XAxisPoints, YAxisPoints, ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            double[, ,] Initial3DValues = new double[XAxisPoints.Length, YAxisPoints.Length, ZAxisPoints.Length];

            Initial3DValues[0, 0, 0] = 0;
            Initial3DValues[1, 1, 1] = Math.Sqrt(2 + 1);

            Initial3DValues[1, 0, 0] = 1;
            Initial3DValues[1, 0, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[1, 1, 0] = Math.Sqrt(1 + 1);

            Initial3DValues[0, 1, 0] = 1;
            Initial3DValues[0, 1, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[0, 0, 1] = 1;

            interpolator.Initial3DValues = Initial3DValues;

            interpolator.LinearInterpolation();

            Assert.AreEqual(Math.Sqrt(2 * 0.5 * 0.5), interpolator.Interpolated3DValues[1, 1, 1], "Problem with interpolated point in X-Y direction!");
        }

        [TestMethod]
        public void XZDirection_Only_SholdBeInterpolated()
        {
            double[] XAxisPoints = new double[2] { 0, 1 };
            double[] YAxisPoints = new double[2] { 0, 1 };
            double[] ZAxisPoints = new double[2] { 0, 1 };

            double[] XAxisNewPoints = new double[2] { 0, 0.5 };
            double[] YAxisNewPoints = new double[2] { 0, 0 };
            double[] ZAxisNewPoints = new double[2] { 0, 0.5 };

            var interpolator = new TriLinear(
                XAxisPoints, YAxisPoints, ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            double[, ,] Initial3DValues = new double[XAxisPoints.Length, YAxisPoints.Length, ZAxisPoints.Length];

            Initial3DValues[0, 0, 0] = 0;
            Initial3DValues[1, 1, 1] = Math.Sqrt(2 + 1);

            Initial3DValues[1, 0, 0] = 1;
            Initial3DValues[1, 0, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[1, 1, 0] = Math.Sqrt(1 + 1);

            Initial3DValues[0, 1, 0] = 1;
            Initial3DValues[0, 1, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[0, 0, 1] = 1;

            interpolator.Initial3DValues = Initial3DValues;

            interpolator.LinearInterpolation();

            Assert.AreEqual(Math.Sqrt(2 * 0.5 * 0.5), interpolator.Interpolated3DValues[1, 1, 1], "Problem with interpolated point in X-Z direction!");
        }

        [TestMethod]
        public void YZDirection_Only_SholdBeInterpolated()
        {
            double[] XAxisPoints = new double[2] { 0, 1 };
            double[] YAxisPoints = new double[2] { 0, 1 };
            double[] ZAxisPoints = new double[2] { 0, 1 };

            double[] XAxisNewPoints = new double[2] { 0, 0 };
            double[] YAxisNewPoints = new double[2] { 0, 0.5 };
            double[] ZAxisNewPoints = new double[2] { 0, 0.5 };

            var interpolator = new TriLinear(
                XAxisPoints, YAxisPoints, ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            double[, ,] Initial3DValues = new double[XAxisPoints.Length, YAxisPoints.Length, ZAxisPoints.Length];

            Initial3DValues[0, 0, 0] = 0;
            Initial3DValues[1, 1, 1] = Math.Sqrt(2 + 1);

            Initial3DValues[1, 0, 0] = 1;
            Initial3DValues[1, 0, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[1, 1, 0] = Math.Sqrt(1 + 1);

            Initial3DValues[0, 1, 0] = 1;
            Initial3DValues[0, 1, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[0, 0, 1] = 1;

            interpolator.Initial3DValues = Initial3DValues;

            interpolator.LinearInterpolation();

            Assert.AreEqual(Math.Sqrt(2 * 0.5 * 0.5), interpolator.Interpolated3DValues[1, 1, 1], "Problem with interpolated point in Y-Z direction!");
        }

        [TestMethod]
        public void AllDirections_SholdBeInterpolated()
        {
            double[] XAxisPoints = new double[2] { 0, 1 };
            double[] YAxisPoints = new double[2] { 0, 1 };
            double[] ZAxisPoints = new double[2] { 0, 1 };

            double[] XAxisNewPoints = new double[2] { 0, 0.5 };
            double[] YAxisNewPoints = new double[2] { 0, 0.5 };
            double[] ZAxisNewPoints = new double[2] { 0, 0.5 };

            var interpolator = new TriLinear(
                XAxisPoints, YAxisPoints, ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            double[, ,] Initial3DValues = new double[XAxisPoints.Length, YAxisPoints.Length, ZAxisPoints.Length];

            Initial3DValues[0, 0, 0] = 0;
            Initial3DValues[1, 1, 1] = Math.Sqrt(2 + 1);

            Initial3DValues[1, 0, 0] = 1;
            Initial3DValues[1, 0, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[1, 1, 0] = Math.Sqrt(1 + 1);

            Initial3DValues[0, 1, 0] = 1;
            Initial3DValues[0, 1, 1] = Math.Sqrt(1 + 1);
            Initial3DValues[0, 0, 1] = 1;

            interpolator.Initial3DValues = Initial3DValues;

            interpolator.LinearInterpolation();

            Assert.AreEqual(Math.Sqrt(3) / 2, interpolator.Interpolated3DValues[1, 1, 1], "Problem with interpolated point in ALL directions!");
        }
    }
}
