using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
         private int getLowestIndex(List<Point> points)
         {
             double X = points.Min(x => x.X);
             return points.FindIndex(x => x.X == X);
         }
        private int iterate(int currentIndex, ref List<Point> points, ref List<Point> outPoints)
        {
            int q = currentIndex + 1;
            q = q >= points.Count ? q - points.Count : q;
            for (int i = 0; i < points.Count; i++)
            {
                if (i == currentIndex) { continue; }
                Point p3p1 = new Point(points[q].X - points[currentIndex].X, points[q].Y - points[currentIndex].Y);
                Point p2p1 = new Point(points[i].X - points[currentIndex].X, points[i].Y - points[currentIndex].Y);
                double crossproductZ = p3p1.X * p2p1.Y - p2p1.X * p3p1.Y;
                if (crossproductZ > 0 || (crossproductZ == 0 && Math.Sqrt(Math.Pow(p2p1.X, 2) + Math.Pow(p2p1.Y, 2)) > Math.Sqrt(Math.Pow(p3p1.X, 2) + Math.Pow(p3p1.Y, 2)))) q = i;
            }
            outPoints.Add(points[q]);
            return q;
        }
      
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
             int firstIndex = getLowestIndex(points);
             int currentIndex = iterate(firstIndex, ref points, ref outPoints);
             while (currentIndex != firstIndex)
             {
                 currentIndex = iterate(currentIndex, ref points, ref outPoints);
             }
           
         
        }



        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}