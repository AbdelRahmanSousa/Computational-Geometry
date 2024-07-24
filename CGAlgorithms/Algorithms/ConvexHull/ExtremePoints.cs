using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    
     
    public class ExtremePoints : Algorithm
    {
        public bool arePointsEqual(Point a, Point b) { 
            return a.X == b.X && a.Y == b.Y;
        }
        public double orientation(Point a, Point b, Point c) { 
            double abX = a.X - b.X;
            double abY = a.Y- b.Y;
            double acX = a.X - c.X;
            double acY = a.Y - c.Y;
            return Math.Min(1, Math.Max(-1,abX * acY - acX * abY));
        }
        public bool isPointInTriangle(Point a, Point b, Point c, Point q) {
            double direction1 = orientation(a, b, q);
            
            double direction2 = orientation(b, c, q);
            
            double direction3 = orientation(c, a, q);
           // if(direction1 == 0 && direction2 == 0 && direction3 == 0) return false;
            return direction1 == direction2 && direction2 == direction3;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            outPoints= new List<Point>();
            for(int i =0; i < points.Count; i++)
            {
                bool pointInTriangle = false;
                for(int j = 0; j < points.Count; j++)
                {
                    if(j == i) continue;
                    if (pointInTriangle) break;
                    for (int k = 0; k < points.Count; k++) 
                    { 
                        if(k == i || k == j) continue;
                        if(pointInTriangle) break;
                        for(int z = 0; z < points.Count; z++)
                        {
                            if(z == j || z == k || z == i) continue;
                            pointInTriangle = isPointInTriangle(points[j], points[k], points[z], points[i]);
                            if (pointInTriangle) {
                                break;
                            }
                        }
                    }
                }
                if(!pointInTriangle) outPoints.Add(points[i]);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}

