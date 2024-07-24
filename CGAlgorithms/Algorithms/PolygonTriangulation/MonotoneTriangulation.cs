using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    
    internal class MaxYLineComparer : IComparer<Line>
    {
        public int Compare(Line x, Line y)
        {
            Point lineXHighest = null;
            Point lineYHighest = null;
            if (x.Start.Y > x.End.Y) lineXHighest = x.Start;
            else lineXHighest = x.End;
            if (y.Start.Y > y.End.Y) lineYHighest = y.Start;
            else lineYHighest = y.End;

            if (lineXHighest.Y > lineYHighest.Y || (lineXHighest.Y == lineYHighest.Y && lineXHighest.X < lineYHighest.X)) return -1;
            else if (lineXHighest.Y < lineYHighest.Y || (lineXHighest == lineYHighest && lineXHighest.X > lineYHighest.X)) return 1;
            return 0;
        }
    }
    public class MonotoneTriangulation  : Algorithm
    {
        private bool arePointsEqual(Point x, Point y)
        {
            if(x.X == y.X && x.Y == y.Y) return true;
            return false;
        }
        private Point getNextPointInChain(List<Point> visited, Line vi) {
            if (visited.Count == 0) {
                if (vi.Start.Y > vi.End.Y) return vi.Start;
                else if(vi.Start.Y < vi.End.Y) return vi.End; 
                else if(vi.Start.X < vi.End.X) return vi.Start;
                return vi.End;
            }
            if (arePointsEqual(vi.Start,visited.Last())) return vi.End;
            else if(arePointsEqual(vi.End,visited.Last())) return vi.Start;
            return null;
        }
        private double calculateOrientation(Point p, Point c, Point n)
        {
            Point p2p1 = new Point((c.X - p.X), (c.Y - p.Y));
            Point p3p2 = new Point((n.X - c.X), (n.Y - c.Y));
            double val = p2p1.Y * p3p2.X - p2p1.X * p3p2.Y;
            return val;
        }
        private bool isVertexReflex(List<Point> visited, Point vi) {
            int count = visited.Count;
            if (count < 3) {
                return true;
            }
            double refer = calculateOrientation(visited[count - 1], visited[count - 2], visited[count - 3]);
            double query = calculateOrientation(visited[count - 1], visited[count - 2],  vi);
            if (query > 0 && refer > 0) {
                return true;
            }
            else if (query < 0 && refer < 0) {
                return true;
            }
            else if (query != 0 && refer != 0) {
                return false;    
            }
            return true;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            //Polygon is already monotone
            outPolygons = new List<Polygon>();
            List<Point> visited = new List<Point>();
            lines.Sort(new MaxYLineComparer());
            Queue<Line> sortedLines = new Queue<Line>(lines);
            while (sortedLines.Count > 0)
            {
                Line vi = sortedLines.Dequeue();
                Point nextPoint = getNextPointInChain(visited, vi);
                if (nextPoint != null)
                {
                    if (visited.Count >= 2)
                    {
                        //In chain 
                        Point lastPoint = visited.Last();
                        Point penUltimatePoint = visited[visited.Count - 2];
                        if (!isVertexReflex(visited, nextPoint))
                        {
                            //Convex vertex
                            outPolygons.Add(new Polygon(new List<Line>() {
                                    new Line(lastPoint, penUltimatePoint),
                                    new Line(penUltimatePoint, nextPoint),
                                    new Line(nextPoint, lastPoint)
                                }));
                            visited.RemoveAt(visited.Count - 1);
                            //Process Vi again
                            sortedLines.ToList().Insert(0, vi);
                            continue;
                        }
                    }
                }
                else
                {
                    //Opposite chain
                    Point last_vertex = visited[0];
                    visited.RemoveAt(0);
                    while (visited.Count > 0)
                    {
                        Point curr_vertex = visited[0];
                        visited.RemoveAt(0);
                        outPolygons.Add(new Polygon(new List<Line>() {
                                new Line(nextPoint, curr_vertex),
                                new Line(nextPoint,last_vertex),
                                new Line(curr_vertex, last_vertex)
                            }));
                        last_vertex = curr_vertex;
                    }
                    visited.Add(last_vertex);
                }
                visited.Add(nextPoint);
            }
        }

        public override string ToString()
        {
            return "Monotone Triangulation";
        }
    }
}
