namespace ThreeLinearInterpolation.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ThreeLinearInterpolation;


    [TestClass]
    public class LinearInterpolationTest
    {
        private double[] XAxisPoints = new double[2] { 0, 10 };
        private double[] YAxisPoints = new double[2] { 0, 10 };
        private double[] ZAxisPoints = new double[2] { 0, 10 };

        // Linear equation's coefficients:
        private const double a = 2;
        private const double b = 1.5;
        private const double c = 3;

        private double[, ,] Initial3DValues = new double[2, 2, 2]
           { 
                { 
                    { 0, 10 * c }, { 10 * b, b * 10 + c * 10 } 
                }, 
                { 
                    { 10 * a, a * 10 + c * 10}, { a * 10 + b * 10, a * 10 + b * 10 + c * 10 } 
                }
           };

        [TestMethod]
        public void SamePoint_ShouldGive_SameOutput()
        {
            double[] XAxisNewPoints = new double[1] { 10 };
            double[] YAxisNewPoints = new double[1] { 10 };
            double[] ZAxisNewPoints = new double[1] { 10 };

            var interpolator = new TriLinear(
                this.XAxisPoints, this.YAxisPoints, this.ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            interpolator.Initial3DValues = this.Initial3DValues;

            interpolator.LinearInterpolation();

            double calculatedFunctionValue = (a * XAxisNewPoints[0]) + (b * YAxisNewPoints[0]) + (c * ZAxisNewPoints[0]);

            Assert.AreEqual(calculatedFunctionValue, interpolator.Interpolated3DValues[0, 0, 0], "Problem with same input -> same output!");
        }

        [TestMethod]
        public void XDirection_Only_SholdBeInterpolated()
        {
            double[] XAxisNewPoints = new double[1] { 5 };
            double[] YAxisNewPoints = new double[1] { 0 };
            double[] ZAxisNewPoints = new double[1] { 0 };

            var interpolator = new TriLinear(
                this.XAxisPoints, this.YAxisPoints, this.ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            interpolator.Initial3DValues = this.Initial3DValues;

            interpolator.LinearInterpolation();

            double calculatedFunctionValue = (a * XAxisNewPoints[0]) + (b * YAxisNewPoints[0]) + (c * ZAxisNewPoints[0]);

            Assert.AreEqual(calculatedFunctionValue, interpolator.Interpolated3DValues[0, 0, 0], "Problem with interpolated point in X direction!");
        }

        [TestMethod]
        public void YDirection_Only_SholdBeInterpolated()
        {
            double[] XAxisNewPoints = new double[1] { 0 };
            double[] YAxisNewPoints = new double[1] { 5 };
            double[] ZAxisNewPoints = new double[1] { 0 };

            var interpolator = new TriLinear(
                 this.XAxisPoints, this.YAxisPoints, this.ZAxisPoints,
                 XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                 );

            interpolator.Initial3DValues = this.Initial3DValues;

            interpolator.LinearInterpolation();

            double calculatedFunctionValue = (a * XAxisNewPoints[0]) + (b * YAxisNewPoints[0]) + (c * ZAxisNewPoints[0]);

            Assert.AreEqual(calculatedFunctionValue, interpolator.Interpolated3DValues[0, 0, 0], "Problem with interpolated point in Y direction!");
        }

        [TestMethod]
        public void ZDirection_Only_SholdBeInterpolated()
        {
            double[] XAxisNewPoints = new double[1] { 0 };
            double[] YAxisNewPoints = new double[1] { 0 };
            double[] ZAxisNewPoints = new double[1] { 5 };

            var interpolator = new TriLinear(
                this.XAxisPoints, this.YAxisPoints, this.ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            interpolator.Initial3DValues = this.Initial3DValues;

            interpolator.LinearInterpolation();

            double calculatedFunctionValue = (a * XAxisNewPoints[0]) + (b * YAxisNewPoints[0]) + (c * ZAxisNewPoints[0]);

            Assert.AreEqual(calculatedFunctionValue, interpolator.Interpolated3DValues[0, 0, 0], "Problem with interpolated point in Z direction!");
        }

        [TestMethod]
        public void XYDirection_Only_SholdBeInterpolated()
        {
            double[] XAxisNewPoints = new double[1] { 5 };
            double[] YAxisNewPoints = new double[1] { 5 };
            double[] ZAxisNewPoints = new double[1] { 0 };

            var interpolator = new TriLinear(
                this.XAxisPoints, this.YAxisPoints, this.ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            interpolator.Initial3DValues = this.Initial3DValues;

            interpolator.LinearInterpolation();

            double calculatedFunctionValue = (a * XAxisNewPoints[0]) + (b * YAxisNewPoints[0]) + (c * ZAxisNewPoints[0]);

            Assert.AreEqual(calculatedFunctionValue, interpolator.Interpolated3DValues[0, 0, 0], "Problem with interpolated point in X-Y direction!");
        }

        [TestMethod]
        public void XZDirection_Only_SholdBeInterpolated()
        {
            double[] XAxisNewPoints = new double[1] { 5 };
            double[] YAxisNewPoints = new double[1] { 0 };
            double[] ZAxisNewPoints = new double[1] { 5 };

            var interpolator = new TriLinear(
                this.XAxisPoints, this.YAxisPoints, this.ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            interpolator.Initial3DValues = this.Initial3DValues;

            interpolator.LinearInterpolation();

            double calculatedFunctionValue = (a * XAxisNewPoints[0]) + (b * YAxisNewPoints[0]) + (c * ZAxisNewPoints[0]);

            Assert.AreEqual(calculatedFunctionValue, interpolator.Interpolated3DValues[0, 0, 0], "Problem with interpolated point in X-Z direction!");
        }

        [TestMethod]
        public void YZDirection_Only_SholdBeInterpolated()
        {
            double[] XAxisNewPoints = new double[1] { 0 };
            double[] YAxisNewPoints = new double[1] { 5 };
            double[] ZAxisNewPoints = new double[1] { 5 };

            var interpolator = new TriLinear(
                this.XAxisPoints, this.YAxisPoints, this.ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            interpolator.Initial3DValues = this.Initial3DValues;

            interpolator.LinearInterpolation();

            double calculatedFunctionValue = (a * XAxisNewPoints[0]) + (b * YAxisNewPoints[0]) + (c * ZAxisNewPoints[0]);

            Assert.AreEqual(calculatedFunctionValue, interpolator.Interpolated3DValues[0, 0, 0], "Problem with interpolated point in Y-Z direction!");
        }

        [TestMethod]
        public void AllDirections_SholdBeInterpolated()
        {
            double[] XAxisNewPoints = new double[1] { 5 };
            double[] YAxisNewPoints = new double[1] { 5 };
            double[] ZAxisNewPoints = new double[1] { 5 };

            var interpolator = new TriLinear(
                this.XAxisPoints, this.YAxisPoints, this.ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            interpolator.Initial3DValues = this.Initial3DValues;

            interpolator.LinearInterpolation();

            double calculatedFunctionValue = (a * XAxisNewPoints[0]) + (b * YAxisNewPoints[0]) + (c * ZAxisNewPoints[0]);

            Assert.AreEqual(calculatedFunctionValue, interpolator.Interpolated3DValues[0, 0, 0], "Problem with interpolated point in ALL directions!");
        }

        [TestMethod]
        public void XPointOutsideTheInitialRange_SholdBeExtrapolated()
        {
            double[] XAxisNewPoints = new double[1] { 15 };
            double[] YAxisNewPoints = new double[1] { 5 };
            double[] ZAxisNewPoints = new double[1] { 5 };

            var interpolator = new TriLinear(
                this.XAxisPoints, this.YAxisPoints, this.ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            interpolator.Initial3DValues = this.Initial3DValues;

            interpolator.LinearInterpolation();

            double calculatedFunctionValue = (a * XAxisNewPoints[0]) + (b * YAxisNewPoints[0]) + (c * ZAxisNewPoints[0]);

            Assert.AreEqual(calculatedFunctionValue, interpolator.Interpolated3DValues[0, 0, 0], "Problem with extrapolated point in X direction!");
        }

        [TestMethod]
        public void YPointOutsideTheInitialRange_SholdBeExtrapolated()
        {
            double[] XAxisNewPoints = new double[1] { 5 };
            double[] YAxisNewPoints = new double[1] { 15 };
            double[] ZAxisNewPoints = new double[1] { 5 };

            var interpolator = new TriLinear(
                this.XAxisPoints, this.YAxisPoints, this.ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            interpolator.Initial3DValues = this.Initial3DValues;

            interpolator.LinearInterpolation();

            double calculatedFunctionValue = (a * XAxisNewPoints[0]) + (b * YAxisNewPoints[0]) + (c * ZAxisNewPoints[0]);

            Assert.AreEqual(calculatedFunctionValue, interpolator.Interpolated3DValues[0, 0, 0], "Problem with extrapolated point in Y direction!");
        }

        [TestMethod]
        public void ZPointOutsideTheInitialRange_SholdBeExtrapolated()
        {
            double[] XAxisNewPoints = new double[1] { 5 };
            double[] YAxisNewPoints = new double[1] { 5 };
            double[] ZAxisNewPoints = new double[1] { 15 };

            var interpolator = new TriLinear(
                this.XAxisPoints, this.YAxisPoints, this.ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            interpolator.Initial3DValues = this.Initial3DValues;

            interpolator.LinearInterpolation();

            double calculatedFunctionValue = (a * XAxisNewPoints[0]) + (b * YAxisNewPoints[0]) + (c * ZAxisNewPoints[0]);

            Assert.AreEqual(calculatedFunctionValue, interpolator.Interpolated3DValues[0, 0, 0], "Problem with extrapolated point in Z direction!");
        }

        [TestMethod]
        public void AllPointsOutsideTheInitialRange_SholdBeExtrapolated()
        {
            double[] XAxisNewPoints = new double[1] { 15 };
            double[] YAxisNewPoints = new double[1] { 15 };
            double[] ZAxisNewPoints = new double[1] { 15 };

            var interpolator = new TriLinear(
                this.XAxisPoints, this.YAxisPoints, this.ZAxisPoints,
                XAxisNewPoints, YAxisNewPoints, ZAxisNewPoints
                );

            interpolator.Initial3DValues = this.Initial3DValues;

            interpolator.LinearInterpolation();

            double calculatedFunctionValue = (a * XAxisNewPoints[0]) + (b * YAxisNewPoints[0]) + (c * ZAxisNewPoints[0]);

            Assert.AreEqual(calculatedFunctionValue, interpolator.Interpolated3DValues[0, 0, 0], "Problem with extrapolated points in ALL direction!");
        }
    }
}
