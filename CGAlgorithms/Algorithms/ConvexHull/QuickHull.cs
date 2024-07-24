using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> QuickHull = new List<Point>();
            List<Point> RightPoints = new List<Point>();
            List<Point> LeftPoints = new List<Point>();
            List<int> TwoValues = GetMax(points);
            int i = 0;
            do
            {
                Line line = new Line(points[TwoValues[0]], points[TwoValues[1]]);
                if (HelperMethods.CheckTurn(line, points[i]) == Enums.TurnType.Left)
                    LeftPoints.Add(points[i]);
                else if (HelperMethods.CheckTurn(line, points[i]) == Enums.TurnType.Right)
                    RightPoints.Add(points[i]);
            } while ((i += 1) < points.Count);
            outPoints.Add(points[TwoValues[0]]);
            outPoints.Add(points[TwoValues[1]]);

            Hull(QuickHull, points[TwoValues[0]], points[TwoValues[1]], LeftPoints);
            Hull(QuickHull, points[TwoValues[1]], points[TwoValues[0]], RightPoints);

            outPoints = QuickHull;
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
        private int GetIndexWithMax(List<Point> points, Point p1, Point p2)
        {
            int i = -1;
            int IndexWithMax = -2;
            double max = 0;
            while ((i += 1) < points.Count)
            {
                Point p = points[i];
                double f1 = (p2.Y - p1.Y) * p.X;
                double f2 = (p2.X - p1.X) * p.Y;
                double f3 = p2.X * p1.Y;
                double f4 = p2.Y * p1.X;
                double f5 = Math.Pow(p2.Y - p1.Y, 2);
                double f6 = Math.Pow(p2.X - p1.X, 2);
                double distance = Math.Abs(f1 - f2 + f3 - f4) / Math.Sqrt(f5 + f6);
                if (distance > max)
                {
                    IndexWithMax = i;
                    max = distance;
                }
            }
            return IndexWithMax;
        }
        private List<Point> check(List<Point> QuickHull, Point p1, Point p2)
        {

            if (QuickHull.Contains(p1) == false)
                QuickHull.Add(p1);
            if (QuickHull.Contains(p2) == false)
                QuickHull.Add(p2);
            return QuickHull;
        }
        private void Hull(List<Point> QuickHull, Point p1, Point p2, List<Point> points)
        {
            List<Point> leftPoints = new List<Point>();
            List<Point> rightPoints = new List<Point>();
            int IndexWithMax = GetIndexWithMax(points, p1, p2);
            if (IndexWithMax < 0)
            {
                QuickHull = check(QuickHull, p1, p2);
                return;
            }
            int i = -1;
            while ((i += 1) < points.Count)
            {
                Line line = new Line(p2, points[IndexWithMax]);
                if (HelperMethods.CheckTurn(line, points[i]) == Enums.TurnType.Right)
                    rightPoints.Add(points[i]);
                else
                {
                    line = new Line(p1, points[IndexWithMax]);
                    if (HelperMethods.CheckTurn(line, points[i]) == Enums.TurnType.Left)
                    {
                        leftPoints.Add(points[i]);
                    }
                }
            }
            Hull(QuickHull, points[IndexWithMax], p2, rightPoints);
            Hull(QuickHull, p1, points[IndexWithMax], leftPoints);
        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}