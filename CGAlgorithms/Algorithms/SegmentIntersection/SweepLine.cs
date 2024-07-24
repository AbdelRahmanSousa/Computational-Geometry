using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.SegmentIntersection
{
    class compareLines : IComparer<pointsWIndex>
    {
        public int Compare(pointsWIndex x, pointsWIndex y)
        {
            if (x.p.X < y.p.X) return -1;
            else if (x.p.X > y.p.X) return 1;
            else if (x.p.Y > y.p.Y) return -1;
            else return 1;
        }
    }
    enum VertexType { 
        Start, 
        End,
        Intersection
    }
    class pointsWIndex {
        public Point p;
        public int lineIndex;
        public VertexType vertexType;
        public pointsWIndex(Point p, int i, VertexType vertexType)
        {
            this.p = p;
            this.lineIndex = i;
            this.vertexType = vertexType;
        }
    }
    class intersectionWIndex : pointsWIndex
    {
        public int line2Index;
        public intersectionWIndex(Point p, int line1Index, int line2Index) : base(p, line1Index, VertexType.Intersection)
        {
            this.line2Index = line2Index;
        }
    }
    public class SweepLine:Algorithm
    {
        private bool isPointInSegment(Line l1, Point p) {
            
            double segmentMinX = Math.Min(l1.Start.X, l1.End.X);
            double segmentMaxX = Math.Max(l1.Start.X, l1.End.X);
            double segmentMinY = Math.Min(l1.Start.Y, l1.End.Y);
            double segmentMaxY = Math.Max(l1.Start.Y, l1.End.Y);
            double mref = (l1.Start.Y - l1.End.Y) / (l1.Start.X - l1.End.X);
            double m1 = (p.Y - l1.End.Y) / (p.X - l1.End.X);
            bool check = Math.Round(mref,4) == Math.Round(m1,4);
            return (p.X <= segmentMaxX && p.X >= segmentMinX) && (p.Y <= segmentMaxY && p.Y >= segmentMinY && check);
        }
        private Point GetIntersection(Line l1, Line l2) {
            double m1 = (l1.End.Y - l1.Start.Y) / (l1.End.X - l1.Start.X);
            double m2 = (l2.End.Y - l2.Start.Y) / (l2.End.X - l2.Start.X);
            //Same slope = not intersecting
            if (m1 == m2)
            {
                if (isPointInSegment(l1, l2.Start)) { return l2.Start; }
                else if (isPointInSegment(l1, l2.End)) { return l2.End; }
                else if (isPointInSegment(l2, l1.Start)) { return l1.Start; }
                else if (isPointInSegment(l2, l1.End)) { return l1.End; }
                return null;
            }
            else
            {
                double c1 = l1.Start.Y - m1 * l1.Start.X;
                double c2 = l2.Start.Y - m2 * l2.Start.X;
                double x = (c2 - c1) / (m1 - m2);
                double y = m1 * x + c1;
                Point intersection = new Point(x, y);
                if (isPointInSegment(l1, intersection) && isPointInSegment(l2, intersection))
                    return intersection;
                return null;
            }
            
        }
        private int AddToStatusList(List<Line> allLines,List<int> list, int x) {
            double maxSegmentx = Math.Max(allLines[x].Start.Y, allLines[x].End.Y);
            for(int i = 0; i < list.Count; i++)
            {
                double maxSegmentI = Math.Max(allLines[i].Start.Y, allLines[i].End.Y);
                if (maxSegmentx  > maxSegmentI) {
                    list.Insert(i, x);
                    return i;
                }
            }
            list.Add(x);
            return list.Count - 1;
        }
        

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            //A list that contains the indices of current lines in status
            List<int> status = new List<int>();
            //Set of intersection Points
            HashSet<Point> ip = new HashSet<Point>();
            var comparer = new compareLines();
            #region Make event queue
            List<pointsWIndex> allPoints = new List<pointsWIndex>();
            for(int i = 0; i < lines.Count; i++) {
                allPoints.Add(new pointsWIndex(lines[i].Start, i, lines[i].Start.X < lines[i].End.X? VertexType.Start : VertexType.End));
                allPoints.Add(new pointsWIndex(lines[i].End, i, lines[i].Start.X < lines[i].End.X? VertexType.End : VertexType.Start));
            }
            allPoints.Sort(comparer);
            Queue<pointsWIndex> events = new Queue<pointsWIndex>(allPoints);
            #endregion
            
            double sweepX = double.MinValue;
            #region sweep
            while (events.Count > 0) { 
                pointsWIndex ev = events.Dequeue();
                //Event is an intersection
                if (ev.vertexType == VertexType.Intersection) { 
                    intersectionWIndex intersectionPoint = (intersectionWIndex)ev;
                    int statusIndex = status.IndexOf(intersectionPoint.lineIndex);
                    int status2Index = status.IndexOf(intersectionPoint.line2Index);
                    sweepX = ev.p.X > sweepX ? ev.p.X : sweepX;
                    //Swap
                    int temp = status[statusIndex];
                    status[statusIndex] = status[status2Index];
                    status[status2Index] = temp;
                    Point i1 = null; 
                    Point i2 = null; 
                    bool needSort = false;
                    if (statusIndex > 0)
                    {
                        i1 = GetIntersection(lines[status[statusIndex - 1]], lines[status[statusIndex]]);
                        var i1point = new intersectionWIndex(i1, status[statusIndex], status[statusIndex] - 1);

                        if (i1 != null && i1.X >= sweepX && !events.Contains(i1point))
                        {
                            needSort = true;
                            events.Enqueue(i1point);
                            ip.Add(i1);
                        }
                    }
                    if (status2Index < status.Count - 1) { 
                        i2 = GetIntersection(lines[status[status2Index + 1]], lines[status2Index]); 
                        var i2point = new intersectionWIndex(i2, status[status2Index], status[status2Index] + 1);
                        if (i2 != null && i2.X >= sweepX && !events.Contains(i2point))
                        {
                            needSort = true;
                            events.Enqueue(i2point);
                            ip.Add(i2);
                        }
                    }
                    //Add to events 
                    if (needSort)
                    {
                        var templist = events.ToList();
                        templist.Sort(comparer);
                        events = new Queue<pointsWIndex>(templist);
                    }
                }
                //Event is a start point
                else if (ev.vertexType == VertexType.Start) {
                    sweepX = ev.p.X > sweepX ? ev.p.X : sweepX;
                    int i = -1;
                    if (!status.Contains(ev.lineIndex)) i = AddToStatusList(lines,status, ev.lineIndex);
                    Point i1 = null;
                    Point i2 = null;
                    bool needSort = false;
                    if (i > 0) { 
                        i1 = GetIntersection(lines[status[i - 1]], lines[status[i]]); 
                        var i1point = new intersectionWIndex(i1, status[i - 1],status[i]);
                        if (i1 != null && i1.X >= sweepX && !events.Contains(i1point))
                        {
                            needSort = true;
                            events.Enqueue(i1point);
                            ip.Add(i1);
                        }
                    }
                    if (i < status.Count - 1)
                    {
                        i2 = GetIntersection(lines[status[i + 1]], lines[status[i]]);
                        var i2point = new intersectionWIndex(i2, status[i], status[i + 1]);
                        if (i2 != null && i2.X >= sweepX && !events.Contains(i2point))
                        {
                            needSort = true;
                            events.Enqueue(i2point);
                            ip.Add(i2);
                        }
                    }
                    if (needSort) {
                        var temp = events.ToList();
                        temp.Sort(comparer);
                        events = new Queue<pointsWIndex>(temp);
                    }
                }
                //Event is an end point
                else if (ev.vertexType == VertexType.End) {
                    int lineIndex = status.IndexOf(ev.lineIndex);
                    if (lineIndex > 0 && lineIndex < status.Count - 1)
                    {
                        Point I = GetIntersection(lines[status[lineIndex - 1]],lines[status[lineIndex + 1]]);
                        if (I != null && I.X >= sweepX)
                        {
                            ip.Add(I);
                            events.Enqueue(new intersectionWIndex(I, status[lineIndex - 1], status[lineIndex + 1]));
                            var temp = events.ToList();
                            temp.Sort(comparer);
                            events = new Queue<pointsWIndex>(temp);
                        }
                    }
                    status.Remove(ev.lineIndex);
                    sweepX = lines[ev.lineIndex].End.X > sweepX ? lines[ev.lineIndex].End.X : sweepX;
                }
            }
            #endregion
            outPoints = ip.ToList();
        }

        public override string ToString()
        {
            return "Sweep Line";
        }
    }
}
