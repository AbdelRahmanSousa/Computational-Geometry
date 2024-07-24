using CGUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithmsUnitTest
{
    internal class CG_TestCase
    {
        public List<Line> lines { get; private set; }
        public List<Polygon> polygons { get; private set; }
        public List<Point> points { get; private set; }

        public List<Line> expected_lines { get; private set; }
        public List<Polygon> expected_polygons { get; private set; }
        public List<Point> expected_points { get; private set; }

        public CG_TestCase(List<Line> lines, List<Polygon> polygons, List<Point> points, List<Line> expected_lines, List<Polygon> expected_polygons, List<Point> expected_points)
        {
            this.lines = lines;
            this.polygons = polygons;
            this.points = points;
            this.expected_lines = expected_lines;
            this.expected_polygons = expected_polygons;
            this.expected_points = expected_points;
        }
        public void Test(List<Line> predicted_lines, List<Polygon> predicted_polygons, List<Point> predicted_points) {
            CollectionAssert.AreEqual(expected_lines, predicted_lines);
            CollectionAssert.AreEqual(expected_points, predicted_points);
            CollectionAssert.AreEqual(expected_polygons, predicted_polygons);
        }
    }
}
