using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;
namespace CGAlgorithmsUnitTest
{
    static class Triangulation_Cases
    {
        //Test case 1 - triangle
        static CG_TestCase case_1 = new CG_TestCase(new List<Line>(), new List<Polygon>() { new Polygon(new List<Line>() { }) }, new List<Point>(), new List<Line>(), new List<Polygon>(), new List<Point>());
        //Test case 2 - Square
        static CG_TestCase case_2 = new CG_TestCase(new List<Line>(), new List<Polygon>(), new List<Point>(), new List<Line>(), new List<Polygon>(), new List<Point>());
        //Test case 3 - Non-Convex-self-intersecting
        static CG_TestCase case_3 = new CG_TestCase(new List<Line>(), new List<Polygon>(), new List<Point>(), new List<Line>(), new List<Polygon>(), new List<Point>());
        //Test case 4
        static CG_TestCase case_4 = new CG_TestCase(new List<Line>(), new List<Polygon>(), new List<Point>(), new List<Line>(), new List<Polygon>(), new List<Point>());
        //Test case 5
        static CG_TestCase case_5 = new CG_TestCase(new List<Line>(), new List<Polygon>(), new List<Point>(), new List<Line>(), new List<Polygon>(), new List<Point>());
        //Test case 6
        static CG_TestCase case_6 = new CG_TestCase(new List<Line>(), new List<Polygon>(), new List<Point>(), new List<Line>(), new List<Polygon>(), new List<Point>());
        //Test case 7
        static CG_TestCase case_7 = new CG_TestCase(new List<Line>(), new List<Polygon>(), new List<Point>(), new List<Line>(), new List<Polygon>(), new List<Point>());
        //Test case 8
        static CG_TestCase case_8 = new CG_TestCase(new List<Line>(), new List<Polygon>(), new List<Point>(), new List<Line>(), new List<Polygon>(), new List<Point>());
    }
}
/*
 ## Polygon Triangulation Test Cases with Points and Expected Outputs

**1. Convex Polygon - Triangle:**

**Points:**

```
(1, 1)
(2, 1)
(2, 2)
```

**Expected Output:**

```
[(1, 1), (2, 1), (2, 2)]
```

**2. Convex Polygon - Square:**

**Points:**

```
(1, 1)
(2, 1)
(2, 2)
(1, 2)
```

**Expected Output:**

```
[(1, 1), (2, 1), (2, 2)], [(2, 2), (1, 2), (1, 1)]
```

**3. Non-Convex Polygon - Self-Intersecting:**

**Points:**

```
(0, 0)
(1, 1)
(2, 0)
(1, -1)
```

**Expected Output:**

```
Error message indicating self-intersection
```

**4. Non-Convex Polygon - Polygon with Hole:**

**Points:**

```
(0, 0)
(1, 0)
(1, 1)
(0, 1)
(0.5, 0.5)
```

**Expected Output:**

```
[(0, 0), (1, 0), (0.5, 0.5)], [(1, 0), (1, 1), (0.5, 0.5)]
```

**5. Degenerate Case - Collinear Vertices:**

**Points:**

```
(0, 0)
(1, 0)
(2, 0)
```

**Expected Output:**

```
Error message indicating collinear vertices
```

**6. Minimum Area Triangles:**

**Points:**

```
(0, 0)
(10, 0)
(5, 5)
```

**Expected Output:**

```
[(0, 0), (5, 5), (10, 0)]
```

**7. Maximum Angle Triangles:**

**Points:**

```
(0, 0)
(5, 0)
(5, 5)
```

**Expected Output:**

```
[(0, 0), (5, 0), (5, 5)]
```

**8. Specific Constraints - Avoid Diagonal:**

**Points:**

```
(0, 0)
(1, 0)
(1, 1)
(0, 1)
```

**Expected Output:**

```
[(0, 0), (0, 1), (1, 1)], [(1, 1), (1, 0), (0, 0)]
```

**Note:** These are just a few examples, and you may need to generate additional test cases with more complex points and specific expected outputs depending on your needs and the triangulation algorithm you are testing.

 */
