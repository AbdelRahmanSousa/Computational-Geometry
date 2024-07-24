using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CGAlgorithms.Algorithms.ConvexHull
{
   
    public class ExtremeSegments : Algorithm
    {
       
     
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            
            //Point Pi
            for (int i = 0; i < points.Count; i++) {
                //Point Pj with Pi form a line
                for (int j = 0; j < points.Count; j++) { 
                    //Test line PJ-Pi with all points to see if they are all in the same half plane
                    for(int x=0; x < points.Count; x++)
                    {
                        //Calculate 
                    }
                }
            } 
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}

