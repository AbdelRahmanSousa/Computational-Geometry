using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            Point leftmost = GetMaxLeft(points, points[0]);
            Point current = leftmost;
            do
            {
                outPoints.Add(current);
                current = GetNext(points, current, points[0]);
            } while (current != leftmost);

        }
        private static double GetDistance(Point current, Point next)
        {
            return Math.Sqrt(Math.Pow(current.X - next.X, 2) + Math.Pow(current.Y - next.Y, 2));
        }
        private Point GetNext(List<Point> points, Point current, Point next)
        {
            int i = -1;
            while ((i += 1) < points.Count)
            {
                Line line = new Line(current, next);
                next = next == current || HelperMethods.CheckTurn(line, points[i]) == Enums.TurnType.Left ? points[i] : next;

                if (HelperMethods.CheckTurn(line, points[i]) == Enums.TurnType.Colinear)
                    next = GetDistance(current, points[i]) > GetDistance(current, next) ? points[i] : next;
            }
            return next;
        }
        private Point GetMaxLeft(List<Point> points, Point leftmost)
        {
            int i = 0;
            do
            {
                leftmost = points[i].Y < leftmost.Y ? points[i] : leftmost;
            } while ((i += 1) < points.Count);
            return leftmost;
        }


        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }

}
