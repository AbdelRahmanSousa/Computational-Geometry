using CGUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    internal class PartitioningComparer : IComparer<PartitioningPolygonPoint>
    {
        public int Compare(PartitioningPolygonPoint x, PartitioningPolygonPoint y)
        {
            if (x.p.Y > y.p.Y) return -1;
            else if (x.p.Y < y.p.Y) return 1;
            else if (x.p.X < y.p.X) return -1;
            else if (x.p.X > y.p.X) return 1;
            
            return 0;
        }
    }
    internal enum VertexType { 
        Start,
        End,
        Merge,
        Split,
        Regular
    }
    internal class PartitioningPolygonPoint {
        public Point p;
        public int lineIndex;
        public VertexType type;
        private double calculateOrientation(Point p, Point c, Point n)
        {
            Point p2p1 = new Point((c.X - p.X), (c.Y - p.Y));
            Point p3p2 = new Point((n.X - c.X), (n.Y - c.Y));
            double val = p2p1.Y * p3p2.X - p2p1.X * p3p2.Y;
            return val;
        }
        private VertexType determineVertexType(Line l1, Line l2) {
            //A positive value or right turn is towards the polygon's interior, merge or split
            //A negative value or left turn is towards the polygon's exterior, regular or start or end
            double turn = calculateOrientation(l1.Start,l1.End,l2.End);
            if (turn > 0)
            {
                //if it is below its two neighbors it is a merge vertex
                if (l1.Start.Y > l1.End.Y && l2.End.Y > l1.End.Y)
                {
                    return VertexType.Merge;
                }
                //if it is above its two neighbors it is a split vertex
                else if (l1.Start.Y < l1.End.Y && l2.End.Y < l1.End.Y)
                {
                    return VertexType.Split;
                }
            }
            else
            {

                //if it is below its two neighbors it is an End Vertex
                if (l1.Start.Y > l1.End.Y && l2.End.Y > l1.End.Y)
                {
                    return VertexType.End;
                }
                //if it is above its two neighbors it is a Start Vertex
                else if (l1.Start.Y < l1.End.Y && l2.End.Y < l1.End.Y)
                {
                    return VertexType.Start;
                }
            }
            return VertexType.Regular;
        }
        public PartitioningPolygonPoint(Line l1, Line l2, int lineIndex)
        {
            this.p = l1.End;
            this.type = determineVertexType(l1, l2);
            this.lineIndex = lineIndex;
        }
    }
    internal class PartitioningPolygonLine {
        public Line l;
        public PartitioningPolygonPoint helper;
        public PartitioningPolygonLine(Line l , PartitioningPolygonPoint helper) { 
            this.l = l; 
            this.helper = helper;  
        }
        
    }
    internal class PolygonInfo { 
        public Queue<PartitioningPolygonPoint> sorted_vertices { get; private set; }
        public List<Line> connected_lines { get; private set; }
        private bool arePointsEqual(Point x, Point y) { 
            if(x.X == y.X && x.Y == y.Y) return true;
            return false;
        }
        private Line transformLineToCCW(Line l)
        {
           double deltaY = l.Start.Y - l.End.Y;
            if (deltaY == 0)
            {
                if (l.Start.X < l.End.X) return new Line(l.Start, l.End);
                else return new Line(l.End, l.Start);
            }
            else if (deltaY < 0) { return new Line(l.End, l.Start); }
            else { return new Line(l.Start, l.End); }
        }
        /*private PolygonInfo MergeSortLines(List<Line> polygon) { 
            //Merge Sort

        }
        private PolygonInfo() { 
        
        }*/
        public PolygonInfo(List<Line> polygon) {
            polygon = new List<Line>(polygon);
            SortedSet<PartitioningPolygonPoint> points = new SortedSet<PartitioningPolygonPoint>(new PartitioningComparer());
            double minX = int.MaxValue;
            Line currentLine = null;
            //Get the line that is left most O(n)
            for (int i = 0; i < polygon.Count; i++) {
                if (polygon[i].Start.X < minX) { 
                    currentLine = polygon[i];
                    minX = polygon[i].Start.X;
                }
                else if (polygon[i].End.X < minX)
                {
                    currentLine = polygon[i];
                    minX = polygon[i].End.X;
                }
            }
            //Make connected_lines list
            connected_lines = new List<Line>
            {
                transformLineToCCW(currentLine)
            };
            //Sort polygon lines CCW O(n2)
            while (polygon.Count > connected_lines.Count)
            {
                Line last_line = connected_lines.Last();
                for (int i = 0; i <= polygon.Count; i++)
                {
                    
                    if (arePointsEqual(polygon[i].Start, last_line.End))
                    {
                        if (arePointsEqual(polygon[i].End, last_line.Start)) continue;
                        connected_lines.Add(new Line(last_line.End, polygon[i].End));
                        //points.Add(new PartitioningPolygonPoint(last_line, polygon[i], connected_lines.Count - 1));
                        break;
                    }
                    else if (arePointsEqual(polygon[i].End, last_line.End))
                    {
                        if (arePointsEqual(polygon[i].Start, last_line.Start)) continue;
                        connected_lines.Add(new Line(last_line.End, polygon[i].Start));
                       // points.Add(new PartitioningPolygonPoint(last_line, polygon[i], connected_lines.Count - 1));
                        break;
                    }
                }
            }
            for (int i = 0; i < connected_lines.Count; i++) {
                points.Add(new PartitioningPolygonPoint(connected_lines[i], connected_lines[(i + 1) % connected_lines.Count], (i+1) % connected_lines.Count));
            }
            sorted_vertices = new Queue<PartitioningPolygonPoint>(points);
        }
    }
    public class MonotonePartitioning : Algorithm
    {
        private PartitioningPolygonLine get_ej(PolygonInfo info, Dictionary<int, PartitioningPolygonLine> statusTree, Point vertex) {

            PartitioningPolygonLine line = null;
            foreach (var entry in statusTree) {
                
                    if (line == null || Math.Abs(vertex.X - line.helper.p.X) > Math.Abs(vertex.X - entry.Value.helper.p.X))
                    {
                        line = entry.Value;
                    }
                
            }
            return line;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            PolygonInfo info = new PolygonInfo(lines.Count > 0? lines : polygons[0].lines);
            Dictionary<int, PartitioningPolygonLine> statusTree = new Dictionary<int,PartitioningPolygonLine>();
            
            while (info.sorted_vertices.Count > 0)
            {
                PartitioningPolygonPoint vi = info.sorted_vertices.Dequeue();
               
                switch (vi.type)
                {
                    case VertexType.Start:
                    {
                            Line ownerLine = info.connected_lines[vi.lineIndex];
                            statusTree.Add(vi.lineIndex, new PartitioningPolygonLine(ownerLine, vi));       
                            break;
                    }
                    case VertexType.End:
                    {
                            //Index of the previous line in status tree
                            int previousLineIndex = (vi.lineIndex - 1) % info.connected_lines.Count;
                            if (!statusTree.ContainsKey(previousLineIndex)) {
                                break;
                            }
                            PartitioningPolygonLine previousLine = statusTree[previousLineIndex];
                            //if the helper of previous edge is a merge vertex then it vi-h is a diagonal
                            if (previousLine.helper.type == VertexType.Merge) {
                                outLines.Add(new Line(vi.p, previousLine.helper.p));
                            }
                            //remove previous line from status tree
                            statusTree.Remove(previousLineIndex);
                            break;
                    }
                    case VertexType.Split:
                    {
                            Line ownerLine = info.connected_lines[vi.lineIndex];
                            PartitioningPolygonLine ej = get_ej(info, statusTree, vi.p);
                            if (ej == null) break;
                            outLines.Add(new Line(ej.helper.p, vi.p));
                            ej.helper = vi;
                            statusTree.Add(vi.lineIndex, new PartitioningPolygonLine(ownerLine, vi));
                            break;
                    }
                    case VertexType.Merge:
                    {
                            int previousLineIndex = (vi.lineIndex - 1) % info.connected_lines.Count;
                            if (statusTree.ContainsKey(previousLineIndex))
                            {
                                PartitioningPolygonLine previousLine = statusTree[previousLineIndex];
                                if (statusTree[previousLineIndex].helper.type == VertexType.Merge)
                                {
                                    outLines.Add(new Line(previousLine.helper.p, vi.p));
                                }
                                statusTree.Remove(previousLineIndex);
                            }
                            PartitioningPolygonLine ej = get_ej(info, statusTree, vi.p);
                            if(ej.helper.type == VertexType.Merge)
                            {
                                outLines.Add(new Line(ej.helper.p, vi.p));    
                            }
                            ej.helper = vi;
                            break;
                    }
                    case VertexType.Regular:
                        {
                            //If interior is to the line's right
                            PartitioningPolygonLine ej = get_ej(info, statusTree, vi.p);
                            if (ej == null)
                            {
                                int previousLineIndex = (vi.lineIndex - 1) % info.connected_lines.Count;
                                if (!statusTree.ContainsKey(previousLineIndex)) break;
                                PartitioningPolygonLine previousLine = statusTree[previousLineIndex];
                                if (previousLine.helper.type == VertexType.Merge)
                                {
                                    outLines.Add(new Line(previousLine.helper.p, vi.p));
                                    statusTree.Remove(previousLineIndex);
                                    statusTree.Add(vi.lineIndex, new PartitioningPolygonLine(info.connected_lines[vi.lineIndex], vi));
                                }
                            }
                            else
                            {
                                if (ej.helper.type == VertexType.Merge)
                                {
                                    outLines.Add(new Line(ej.helper.p, vi.p));
                                }
                                ej.helper = vi;
                            }
                            

                            break;
                        }
                    default: { break; }
                }
            }
        }
        public override string ToString()
        {
            return "Monotone Partitioning";
        }
    }
}