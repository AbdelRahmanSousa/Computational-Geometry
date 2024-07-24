using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CGAlgorithms.Algorithms.SegmentIntersection;
using CGAlgorithms;
using CGUtilities;
using System.Collections.Generic;

namespace CGAlgorithmsUnitTest
{
    
    [TestClass]
    public class segmentintersectionTest
    {
        protected Algorithm segmentintersectionTester;
        protected List<Point> inputPoints;
        protected List<Point> outputPoints;
        protected List<Point> desiredPoints;
        protected List<Line> inputLines;
        protected List<Line> outputLines;
        protected List<Polygon> inputPolygons;
        protected List<Polygon> outputPolygons;
        private void InitializeData()
        {
            inputPoints = new List<Point>();
            outputPoints = new List<Point>();
            inputLines = new List<Line>();
            outputLines = new List<Line>();
            desiredPoints = new List<Point>();
            inputPolygons = new List<Polygon>();
            outputPolygons = new List<Polygon>();
        }

        public void TestCase1()
        {
            InitializeData();
            inputLines.Add(new Line(new Point(1, 5), new Point(9, 1)));
            inputLines.Add(new Line(new Point(3, 5), new Point(4, 3)));
            inputLines.Add(new Line(new Point(5, 3.5), new Point(3, 2)));
            inputLines.Add(new Line(new Point(2, 2), new Point(8, 2)));
            desiredPoints.Add(new Point(3.0000000000, 2.0000000000));
            desiredPoints.Add(new Point(3.6666666667, 3.6666666667));
            desiredPoints.Add(new Point(4.6000000000, 3.2000000000));
            desiredPoints.Add(new Point(7.0000000000, 2.0000000000));
            segmentintersectionTester.Run(inputPoints, inputLines, inputPolygons, ref outputPoints, ref outputLines, ref outputPolygons);
            bool success = CompareDesiredWithActual(desiredPoints, outputPoints);
            Assert.IsTrue(success, "Fails in " + segmentintersectionTester.ToString() + ": Sweep Line Test Case");

        }


        public void TestCase2()
        {
            InitializeData();
            inputLines.Add(new Line(new Point(3, 3), new Point(1, 1)));
            inputLines.Add(new Line(new Point(1, 2), new Point(2, 1)));
            desiredPoints.Add(new Point(1.5000000000, 1.5000000000));
            segmentintersectionTester.Run(inputPoints, inputLines, inputPolygons, ref outputPoints, ref outputLines, ref outputPolygons);
            bool success = CompareDesiredWithActual(desiredPoints, outputPoints);
            Assert.IsTrue(success, segmentintersectionTester.ToString() + ": Sweep Line Test Case 2");
        }

        public void TestCase3()
        {
            InitializeData();
            inputLines.Add(new Line(new Point(1, 1), new Point(9, 9)));
            inputLines.Add(new Line(new Point(1, 1), new Point(9, -9)));
            inputLines.Add(new Line(new Point(4, 7), new Point(6, -7)));
            desiredPoints.Add(new Point(1.0000000000, 1.0000000000));
            desiredPoints.Add(new Point(4.3750000000, 4.3750000000));
            desiredPoints.Add(new Point(5.6956521739, -4.8695652174));
            segmentintersectionTester.Run(inputPoints, inputLines, inputPolygons, ref outputPoints, ref outputLines, ref outputPolygons);
            bool success = CompareDesiredWithActual(desiredPoints, outputPoints);
            Assert.IsTrue(success, segmentintersectionTester.ToString() + ": Sweep Line Test Case 3");
        }

        public void TestCase4()
        {
            InitializeData();
            inputLines.Add(new Line(new Point(1, 1), new Point(9, 9)));
            inputLines.Add(new Line(new Point(2, 3), new Point(7, 8)));
            inputLines.Add(new Line(new Point(3, 2), new Point(7, 8)));
            desiredPoints.Add(new Point(5.0000000000, 5.0000000000));
            desiredPoints.Add(new Point(7.0000000000, 8.0000000000));
            segmentintersectionTester.Run(inputPoints, inputLines, inputPolygons, ref outputPoints, ref outputLines, ref outputPolygons);
            bool success = CompareDesiredWithActual(desiredPoints, outputPoints);
            Assert.IsTrue(success, segmentintersectionTester.ToString() + ": Sweep Line Test Case 4");
        }



        private bool CompareDesiredWithActual(List<Point> _desiredPoints, List<Point> _outputPoints)
        {
            if (_outputPoints == null && _desiredPoints != null)
                return false;

            if (_desiredPoints.Count != _outputPoints.Count)
                return false;

            for (int i = 0; i < _desiredPoints.Count; i++)
            {
                bool isFound = false;

                for (int j = 0; j < _outputPoints.Count; j++)
                {
                    if (_desiredPoints[i].Equals(_outputPoints[j]))
                    {
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                    return false;
            }

            return true;
        }
    }
}
