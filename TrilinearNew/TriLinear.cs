namespace ThreeLinearInterpolation
{
    using System;

    internal class InterpolateXS
    {
        public InterpolateXS()
        {
        }

        // Initial mesh of parameters' values
        public double[] xAxisPoints { get; set; }

        public double[] yAxisPoints { get; set; }

        public double[] zAxisPoints { get; set; }

        // new parameters' values
        public double[] xAxisNewPoints { get; set; }

        public double[] yAxisNewPoints { get; set; }

        public double[] zAxisNewPoints { get; set; }

        public double[, ,] Initial3DValues { get; set; }

        public double[, ,] Interpolated3DValues { get; set; }

        public void LinearInterpolation()
        {
            this.Interpolated3DValues = new double[this.xAxisNewPoints.Length, this.yAxisNewPoints.Length, this.zAxisNewPoints.Length];

            int interpolationCounter = 0;
            //// indexes of the coordinates
            int ix0, ix1, iy0, iy1, iz0, iz1;
            //// coordinates' values
            double x0, x1, y0, y1, z0, z1, x2, y2, z2;

            for (int i = 0; i < this.xAxisNewPoints.Length; i++)
            {
                //// process fT to find 2 points to interpolate inbetween
                int[] outputCoeffArray = this.FindPointsToInterpolate(this.xAxisPoints, this.xAxisNewPoints, i);
                ix0 = outputCoeffArray[0];
                ix1 = outputCoeffArray[1];

                for (int j = 0; j < this.yAxisNewPoints.Length; j++)
                {
                    //// process Mt to find 2 points to interpolate inbetween
                    outputCoeffArray = this.FindPointsToInterpolate(this.yAxisPoints, this.yAxisNewPoints, j);
                    iy0 = outputCoeffArray[0];
                    iy1 = outputCoeffArray[1];

                    for (int k = 0; k < this.zAxisNewPoints.Length; k++)
                    {
                        //// process Dm to find 2 points to interpolate inbetween
                        outputCoeffArray = this.FindPointsToInterpolate(this.zAxisPoints, this.zAxisNewPoints, k);
                        iz0 = outputCoeffArray[0];
                        iz1 = outputCoeffArray[1];

                        // Console.WriteLine(" Interp. step {0}", interpolationCounter);

                        //// points to interpolate from:
                        x0 = this.xAxisPoints[ix0];
                        y0 = this.yAxisPoints[iy0];
                        z0 = this.zAxisPoints[iz0];

                        x1 = this.xAxisPoints[ix1];
                        y1 = this.yAxisPoints[iy1];
                        z1 = this.zAxisPoints[iz1];

                        //// current points:
                        x2 = this.xAxisNewPoints[i];
                        y2 = this.yAxisNewPoints[j];
                        z2 = this.zAxisNewPoints[k];

                        //// FIRST VARIANT OF INTERPOLATION
                        this.FirstInterpolationVariant(ix0, ix1, iy0, iy1, iz0, iz1, x0, x1, y0, y1, z0, z1, x2, y2, z2, i, j, k);

                        //// SECOND VARIANT OF INTERPOLATION
                        //// this.SecondInterpolationVariant(ix0, ix1, iy0, iy1, iz0, iz1, i, j, k);

                        //// printings to verify values and points
                        //this.PrintingOfPointsAndValues(ix0, ix1, iy0, iy1, iz0, iz1, x0, x1, y0, y1, z0, z1, x2, y2, z2, i, j, k);

                        interpolationCounter++;
                    }
                }
            }
        }

        private void PrintingOfPointsAndValues(int ix0, int ix1, int iy0, int iy1, int iz0, int iz1, double x0, double x1, double y0, double y1, double z0, double z1, double x2, double y2, double z2, int i, int j, int k)
        {
            Console.WriteLine(" Coordinates x0={0}, y0={1}, z0={2}  -> Function value is : '{3:E5}' ", x0, y0, z0, this.Initial3DValues[ix0, iy0, iz0]);
            Console.WriteLine(" Coordinates x0={0}, y0={1}, z1={2}  -> Function value is : '{3:E5}' ", x0, y0, z1, this.Initial3DValues[ix0, iy0, iz1]);
            Console.WriteLine(" Coordinates x0={0}, y1={1}, z0={2}  -> Function value is : '{3:E5}' ", x0, y1, z0, this.Initial3DValues[ix0, iy1, iz0]);
            Console.WriteLine(" Coordinates x0={0}, y1={1}, z1={2}  -> Function value is : '{3:E5}' ", x0, y1, z1, this.Initial3DValues[ix0, iy1, iz1]);
            Console.WriteLine(" Coordinates x1={0}, y0={1}, z0={2}  -> Function value is : '{3:E5}' ", x1, y0, z0, this.Initial3DValues[ix1, iy0, iz0]);
            Console.WriteLine(" Coordinates x1={0}, y0={1}, z1={2}  -> Function value is : '{3:E5}' ", x1, y0, z1, this.Initial3DValues[ix1, iy0, iz1]);
            Console.WriteLine(" Coordinates x1={0}, y1={1}, z0={2}  -> Function value is : '{3:E5}' ", x1, y1, z0, this.Initial3DValues[ix1, iy1, iz0]);
            Console.WriteLine(" Coordinates x1={0}, y1={1}, z1={2}  -> Function value is : '{3:E5}' ", x1, y1, z1, this.Initial3DValues[ix1, iy1, iz1]);
            Console.WriteLine(" Coordinates x2={0}, y2={1}, z2={2}  -> INTERPOLATED VALUE: '{3:E5}' ", x2, y2, z2, this.Interpolated3DValues[i, j, k]);
        }

        private void SecondInterpolationVariant(int ix0, int ix1, int iy0, int iy1, int iz0, int iz1, int i, int j, int k)
        {
            double fT_norm, mT_norm, dM_norm, f00, f10, f01, f11, f0, f1;
            //// set the coordinates of the new interpolated value
            fT_norm = this.NormalizeDistance(this.xAxisPoints, this.xAxisNewPoints, ix0, ix1, i);
            mT_norm = this.NormalizeDistance(this.yAxisPoints, this.yAxisNewPoints, iy0, iy1, j);
            dM_norm = this.NormalizeDistance(this.zAxisPoints, this.zAxisNewPoints, iz0, iz1, k);

            //// Interpolation block ....
            //// first step of the 3-linear interpolation
            f00 = (this.Initial3DValues[ix0, iy0, iz0] * Math.Abs(1 - fT_norm)) + (this.Initial3DValues[ix1, iy0, iz0] * fT_norm);
            f10 = (this.Initial3DValues[ix0, iy1, iz0] * Math.Abs(1 - fT_norm)) + (this.Initial3DValues[ix1, iy1, iz0] * fT_norm);
            f01 = (this.Initial3DValues[ix0, iy0, iz1] * Math.Abs(1 - fT_norm)) + (this.Initial3DValues[ix1, iy0, iz1] * fT_norm);
            f11 = (this.Initial3DValues[ix0, iy1, iz1] * Math.Abs(1 - fT_norm)) + (this.Initial3DValues[ix1, iy1, iz1] * fT_norm);

            //// second step of the 3-linear interpolation
            f0 = (f00 * Math.Abs(1 - mT_norm)) + (f10 * mT_norm);
            f1 = (f01 * Math.Abs(1 - mT_norm)) + (f11 * mT_norm);

            // Console.WriteLine(" dM_norm = {0}, mT_norm={1}, fT_norm,={2} ", dM_norm, mT_norm, fT_norm);

            // Console.WriteLine(" f00={0:E5}, f10={1:E5}, f01={2:E5}, f11={3:E5} ", f00, f10, f01, f11);

            // Console.WriteLine(" f0={0:E5}, f1={1:E5}", f0, f1);

            //// final step of the 3-linear interpolation
            this.Interpolated3DValues[i, j, k] = (f0 * Math.Abs(1 - dM_norm)) + (f1 * dM_norm);
        }

        private void FirstInterpolationVariant(int ix0, int ix1, int iy0, int iy1, int iz0, int iz1, double x0, double x1, double y0, double y1, double z0, double z1, double x2, double y2, double z2, int i, int j, int k)
        {
            //// Normalized volumes:
            double Na, Nb, Nc, Nd, Ne, Nf, Ng, Nh;
            //// current summation
            double x12, x20, y12, y20, z12, z20, total;
            total = (x1 - x0) * (y1 - y0) * (z1 - z0);

            x12 = x1 - x2;
            x20 = x2 - x0;
            y12 = y1 - y2;
            y20 = y2 - y0;
            z12 = z1 - z2;
            z20 = z2 - z0;

            Na = (x12 * y12 * z20) / total;
            Nb = (x12 * y20 * z20) / total;

            Nc = (x20 * y12 * z20) / total;
            Nd = (x20 * y20 * z20) / total;

            Ne = (x12 * y12 * z12) / total;
            Nf = (x12 * y20 * z12) / total;

            Ng = (x20 * y12 * z12) / total;
            Nh = (x20 * y20 * z12) / total;

            ////    f8           f0                              f1
            this.Interpolated3DValues[i, j, k] = (this.Initial3DValues[ix0, iy0, iz1] * Na) + (this.Initial3DValues[ix0, iy1, iz1] * Nb) +
                ////                 f2                              f3
                              (this.Initial3DValues[ix1, iy0, iz1] * Nc) + (this.Initial3DValues[ix1, iy1, iz1] * Nd) +
                ////                 f4                              f5
                              (this.Initial3DValues[ix0, iy0, iz0] * Ne) + (this.Initial3DValues[ix0, iy1, iz0] * Nf) +
                ////                 f6                              f7
                              (this.Initial3DValues[ix1, iy0, iz0] * Ng) + (this.Initial3DValues[ix1, iy1, iz0] * Nh);
        }

        private int[] FindPointsToInterpolate(double[] pointsToInterpolate, double[] newPoints, int counter)
        {
            int[] coordinates = new int[2];
            double delta = 0;

            //// searching for points before and after the desired point to interpolate
            for (int h = 0; h < pointsToInterpolate.Length; h++)
            {
                delta = pointsToInterpolate[h] - newPoints[counter];
                //// if delta == zero:
                if (Math.Abs(delta) < 1e-5)
                {
                    if (h == 0)
                    {
                        coordinates[0] = h;
                        coordinates[1] = h + 1;
                    }
                    else
                    {
                        coordinates[0] = h - 1;
                        coordinates[1] = h;
                    }

                    break;
                }
                else if (delta > 0)
                {
                    coordinates[0] = h - 1;
                    coordinates[1] = h;
                    break;
                }
                else if ((h == pointsToInterpolate.Length - 1) && (delta < 0))
                {
                    coordinates[0] = h - 1;
                    coordinates[1] = h;
                }
            }

            return coordinates;
        }

        private double NormalizeDistance(double[] pointsToInterpolate, double[] newPoints, int a0, int a1, int counter)
        {
            double norm;
            if (Math.Abs(pointsToInterpolate[a1] - pointsToInterpolate[a0]) < 1e-5)
            {
                Console.WriteLine(" Warning! Points to interpolate are the same!!!! ");
                norm = 0;
            }
            else
            {
                norm = (newPoints[counter] - pointsToInterpolate[a0]) / (pointsToInterpolate[a1] - pointsToInterpolate[a0]);
            }

            return norm;
        }
    }
}