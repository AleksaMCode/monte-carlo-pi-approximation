using Microsoft.VisualStudio.TestTools.UnitTesting;
using monte_carlo_pi_approximation;
using System;

namespace UnitTests
{
    [TestClass]
    public class MonteCarloPiGeneratorTest
    {
        private static MonteCarloPiApproxGenerator gen;

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            gen = new MonteCarloPiApproxGenerator();
            MonteCarloPiApproxGenerator.ScalingFactor = 1.0;

            for (var i = 0; i < 1_000; ++i)
            {
                gen.GeneratePoint();
            }
        }

        [DataTestMethod]
        [DataRow(8.889, 2, 8.88)]
        [DataRow(8.884, 2, 8.88)]
        [DataRow(108.79892, 1, 108.7)]
        public void TestFloor(double value, int decimal_places, double expected)
        {
            var actual = MonteCarloPiApproxGenerator.Floor(value, decimal_places);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPointCount()
        {
            Assert.AreEqual(gen.PointsInCircle.Count, gen.NumberOfPointsInsideCircle);
            Assert.AreEqual(gen.PointsNotInCircle.Count, gen.NumberOfPoints - gen.NumberOfPointsInsideCircle);
        }

        [TestMethod]
        public void TestPointsPlacement()
        {
            for (var i = 0; i < 100; ++i)
            {
                gen.GeneratePoint();
                var point = gen.Point;

                if (Math.Sqrt(point.X * point.X + point.Y * point.Y) <= 1.0)
                {
                    Assert.IsTrue(gen.PointsInCircle.Contains(gen.Point));
                }
                else
                {
                    Assert.IsTrue(gen.PointsNotInCircle.Contains(gen.Point));
                }
            }
        }
    }
}
