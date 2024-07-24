using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CGAlgorithms.Algorithms.ConvexHull
{

    public class GrahamScan : Algorithm
    {
        private class GrahamComparer : IComparer<Point>
        {
            private Point P0;
            public GrahamComparer(Point p0)
            {
                this.P0 = p0;
            }
            public static double calculateOrientation(Point p, Point c, Point n)
            {
                Point p2p1 = new Point((c.X - p.X), (c.Y - p.Y));
                Point p3p2 = new Point((n.X - c.X), (n.Y - c.Y));
                double val = p2p1.Y * p3p2.X - p2p1.X * p3p2.Y;
                return val;
            }
            public int Compare(Point x, Point y)
            {
                double val = GrahamComparer.calculateOrientation(P0, x, y);
                if (val == 0)
                {
                    double dist1 = Math.Pow((x.X - P0.X), 2) + Math.Pow(x.Y - P0.Y, 2);
                    double dist2 = Math.Pow((y.X - P0.X), 2) + Math.Pow((y.Y - P0.Y), 2);
                    if (dist1 > dist2) return 1; else if (dist2 > dist1) return -1; else return 0;
                }
                return val < 0 ? -1 : 1;
            }
        }
        private Point getStackSecondTop(Stack<Point> stack)
        {
            Point temp = stack.Pop();
            Point secondTop = stack.Peek();
            stack.Push(temp);
            return secondTop;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count < 3)
            {
                outPoints = new List<Point>(points);
                return;
            }
            List<Point> modPoints = new List<Point>(points);
            Point lowest = modPoints[0];
            for (int i = 1; i < modPoints.Count; i++)
            {
                if (modPoints[i].Y < lowest.Y)
                {
                    lowest = modPoints[i];
                }
                else if (modPoints[i].Y == lowest.Y)
                {
                    lowest = modPoints[i].X < lowest.X ? modPoints[i] : lowest;
                }
            }
            modPoints.Remove(lowest);
            modPoints.Insert(0, lowest);
            modPoints.Sort(new GrahamComparer(lowest));
            for (int i = 1; i < modPoints.Count; i++)
            {
                while (i < modPoints.Count - 1 && GrahamComparer.calculateOrientation(modPoints[0], modPoints[i], modPoints[i + 1]) == 0)
                    modPoints.RemoveAt(i);
            }
            if (modPoints.Count < 3)
            {
                outPoints = new List<Point>(modPoints);
                return;
            }
            Stack<Point> stack = new Stack<Point>();
            stack.Push(modPoints[0]); stack.Push(modPoints[1]); stack.Push(modPoints[2]);
            for (int x = 3; x < modPoints.Count; x++)
            {
                while (GrahamComparer.calculateOrientation(getStackSecondTop(stack), stack.Peek(), modPoints[x]) >= 0)
                {
                    stack.Pop();
                }
                stack.Push(modPoints[x]);
            }
            outPoints = new List<Point>(stack);
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}