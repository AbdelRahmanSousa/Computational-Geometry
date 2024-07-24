using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;
using static CGUtilities.Enums;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            List<int> TwoValues = GetMax(points);
            List<Point> upperHullPoints = DivideAndConquerAlgorithm(points, TwoValues[0], TwoValues[1], Enums.TurnType.Left);
            List<Point> lowerHullPoints = DivideAndConquerAlgorithm(points, TwoValues[0], TwoValues[1], Enums.TurnType.Right);
            upperHullPoints.Add(points[TwoValues[0]]);
            lowerHullPoints.Add(points[TwoValues[1]]);
            outPoints.AddRange(upperHullPoints);
            outPoints.AddRange(lowerHullPoints);
            outPoints = outPoints.Distinct().ToList();
        }
        private List<int> GetMax(List<Point> points)
        {
            int leftPointOnLine = 0;
            int rightPointOnLine = 0;
            int i = 0;
            do
            {
                rightPointOnLine = points[i].Y > points[rightPointOnLine].Y ? i : rightPointOnLine;
                leftPointOnLine = points[i].Y < points[leftPointOnLine].Y ? i : leftPointOnLine;
            } while ((i += 1) < points.Count);
            List<int> TwoValues = new List<int>();
            TwoValues.Add(leftPointOnLine);
            TwoValues.Add(rightPointOnLine);
            return TwoValues;
        }
        private static int GetIndexWithMax(List<Point> points, int left, int right, Enums.TurnType turnType)
        {
            int i = -1;
            int IndexWithMax = -1;
            double max = 0;
            while ((i += 1) < points.Count)
            {
                Point p = points[i];
                Line line = new Line(points[left], points[right]);
                Point p1 = points[left];
                Point p2 = points[right];
                double f1 = (p2.Y - p1.Y) * p.X;
                double f2 = (p2.X - p1.X) * p.Y;
                double f3 = p2.X * p1.Y;
                double f4 = p2.Y * p1.X;
                double f5 = Math.Pow(p2.Y - p1.Y, 2);
                double f6 = Math.Pow(p2.X - p1.X, 2);
                double distance = Math.Abs(f1 - f2 + f3 - f4) / Math.Sqrt(f5 + f6);
                Enums.TurnType turnType2 = HelperMethods.CheckTurn(line, points[i]);
                if (turnType2 == turnType)
                    if (distance > max)
                    {
                        IndexWithMax = i;
                        max = distance;
                    }
            }
            return IndexWithMax;
        }
        public static List<Point> DivideAndConquerAlgorithm(List<Point> points, int left, int right, Enums.TurnType turnType1)
        {
            List<Point> DivideAndConquer = new List<Point>();
            int IndexWithMax = GetIndexWithMax(points, left, right, turnType1);
            if (IndexWithMax == -1)
                return DivideAndConquer;
            DivideAndConquer.Add(points[IndexWithMax]);
            List<Point> leftHull = DivideAndConquerAlgorithm(points, left, IndexWithMax, turnType1);
            List<Point> rightHull = DivideAndConquerAlgorithm(points, IndexWithMax, right, turnType1);
            DivideAndConquer.AddRange(leftHull);
            DivideAndConquer.AddRange(rightHull);
            return DivideAndConquer;
        }
        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}