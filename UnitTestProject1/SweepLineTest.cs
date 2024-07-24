using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CGAlgorithms.Algorithms.SegmentIntersection;
using CGAlgorithms;
using CGUtilities;
using System.Collections.Generic;

namespace CGAlgorithmsUnitTest
{
    
    [TestClass]
    public class SweepLineTest : segmentintersectionTest
    {
        [TestMethod]
        public void SweepLineTestCase1()
        {
            segmentintersectionTester = new SweepLine();
            TestCase1();
        }
        [TestMethod]
        public void SweepLineTestCase2()
        {
            segmentintersectionTester = new SweepLine();
            TestCase2();
        }
        [TestMethod]
        public void SweepLineTestCase3()
        {
            segmentintersectionTester = new SweepLine();
            TestCase3();
        }
        [TestMethod]
        public void SweepLineTestCase4()
        {
            segmentintersectionTester = new SweepLine();
            TestCase4();
        }
    }
}