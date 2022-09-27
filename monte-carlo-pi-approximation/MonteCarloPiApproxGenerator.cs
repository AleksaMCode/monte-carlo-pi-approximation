using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monte_carlo_pi_approximation
{
    public class MonteCarloPiApproxGenerator
    {
        public int NumberOfPoints = 0;
        public int NumberOfPointsInsideCircle = 0;
        public int Iterationcount = 0;

        public void GeneratePoint()
        {
            var rnd = new Random();
            var point = new Point(rnd.NextDouble(), rnd.NextDouble());
            ++NumberOfPoints;

            if (Math.Sqrt(point.X * point.X + point.Y * point.Y) <= 1.0)
            {
                ++NumberOfPointsInsideCircle;
            }
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
