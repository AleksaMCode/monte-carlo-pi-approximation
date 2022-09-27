using System;
using System.Collections.Generic;
using OxyPlot;

namespace monte_carlo_pi_approximation
{
    public class MonteCarloPiApproxGenerator
    {
        private DataPoint point;
        private Random rnd = new Random();
        private const double CIRCLE_ANGLE_DELTA = 0.001;

        public static readonly double ScalingFactor = 0.35;
        public int NumberOfPoints = 0;
        public int NumberOfPointsInsideCircle = 0;
        public int Iterationcount = 0;
        public DataPoint ScaledPoint => new DataPoint(point.X * ScalingFactor, point.Y * ScalingFactor);
        public IList<DataPoint> PointsInCircle { get; private set; } = new List<DataPoint>();
        public IList<DataPoint> PointsNotInCircle { get; private set; } = new List<DataPoint>();

        public void GeneratePoint()
        {
            point = new DataPoint(rnd.NextDouble(), rnd.NextDouble());
            ++NumberOfPoints;

            if (Math.Sqrt(point.X * point.X + point.Y * point.Y) <= 1.0)
            {
                ++NumberOfPointsInsideCircle;
                PointsInCircle.Add(ScaledPoint);
            }
            else
            {
                PointsNotInCircle.Add(ScaledPoint);
            }
        }

        /// <summary>
        /// Generates quarter of a circle in a upper right quadrant using points.
        /// </summary>
        /// <returns>Quarter of a circle composed of individual points</returns>
        public static HashSet<DataPoint> GenerateQuarterCircleUpperRightQuadrant()
        {
            var QuarterCircle = new HashSet<DataPoint>();

            // angle * 180 / π is a conversion of Radian to Degree ! For the first quadrant angle∈[0,90]
            for (var angle = 0d; angle * 180 / Math.PI <= 90; angle += CIRCLE_ANGLE_DELTA)
            {
                QuarterCircle.Add(new DataPoint(Math.Cos(angle) * ScalingFactor, Math.Sin(angle) * ScalingFactor));
            }

            return QuarterCircle;
        }

        public double CalculatePiValue(int decimalPlaces = 8)
        {
            return Floor(4 * (NumberOfPointsInsideCircle / (double)NumberOfPoints), decimalPlaces);
        }

        public static double Floor(double value, int decimalPlaces)
        {
            return Math.Floor(value * Math.Pow(10, decimalPlaces)) / Math.Pow(10, decimalPlaces);
        }
    }
}
