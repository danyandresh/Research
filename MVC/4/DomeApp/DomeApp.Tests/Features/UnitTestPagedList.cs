using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomeApp.Code.Paging;
using System.Linq;

namespace DomeApp.Tests.Features
{
    [TestClass]
    public class UnitTestPagedList
    {
        [TestMethod]
        public void TestPagedListPageRangeStartsFrom1()
        {
            var items = Enumerable.Range(1, 200).AsQueryable();
            IPagedList pagedList = PagingExtensions.ToPagedList(items, 20);

            var pageRange = pagedList.GetPageRange(10);

            Assert.AreEqual(1, pageRange.First());
        }

        [TestMethod]
        public void TestPagedListPageRangeEndsAtExpected()
        {
            var items = Enumerable.Range(1, 200).AsQueryable();
            var randomizer = new Random();
            IPagedList pagedList = PagingExtensions.ToPagedList(items, 20, 10);

            var expectedPagesInRange = randomizer.Next(7, 10);
            var pageRange = pagedList.GetPageRange(expectedPagesInRange);

            Assert.AreEqual(10, pageRange.Last());
        }

        [TestMethod]
        public void TestPagedListPageRangeContainsAllPagesWhenCurrentPageIsAtBegining()
        {
            var items = Enumerable.Range(1, 200).AsQueryable();
            var randomizer = new Random();
            IPagedList pagedList = PagingExtensions.ToPagedList(items, 20);

            var expectedPagesInRange = randomizer.Next(7, 10);
            var pageRange = pagedList.GetPageRange(expectedPagesInRange);
            var expectedRange = Enumerable.Range(1, expectedPagesInRange);
            Assert.IsTrue(expectedRange.SequenceEqual(pageRange));
        }

        [TestMethod]
        public void TestPagedListPageRangeContainsAllPagesWhenCurrentPageIsAtEnd()
        {
            var items = Enumerable.Range(1, 200).AsQueryable();
            var randomizer = new Random();
            IPagedList pagedList = PagingExtensions.ToPagedList(items, 20, 10);

            var expectedPagesInRange = randomizer.Next(7, 10);
            var pageRange = pagedList.GetPageRange(expectedPagesInRange);
            var expectedRange = Enumerable.Range(10 - expectedPagesInRange+1, expectedPagesInRange);
            Assert.IsTrue(expectedRange.SequenceEqual(pageRange));
        }

        [TestMethod]
        public void TestPagedListHalfOfPageRangeIsBeforeCurrentPageOddPageRangeCount()
        {
            var items = Enumerable.Range(1, 200).AsQueryable();
            var randomizer = new Random();
            var currentPage = randomizer.Next(90, 110);
            IPagedList pagedList = PagingExtensions.ToPagedList(items, 1, currentPage);

            var expectedPagesInRange = 3;
            var pageRange = pagedList.GetPageRange(expectedPagesInRange);

            Assert.AreEqual(currentPage - 1, pageRange.First());
        }

        [TestMethod]
        public void TestPagedListHalfOfPageRangeIsBeforeCurrentPageEvenPageRangeCount()
        {
            var items = Enumerable.Range(1, 200).AsQueryable();
            var randomizer = new Random();
            var currentPage = randomizer.Next(90, 110);
            IPagedList pagedList = PagingExtensions.ToPagedList(items, 1, currentPage);

            var expectedPagesInRange = 4;
            var pageRange = pagedList.GetPageRange(expectedPagesInRange);

            Assert.AreEqual(currentPage - 2, pageRange.First());
        }

        [TestMethod]
        public void TestPagedListHalfOfPageRangeIsAfterCurrentPageOddPageRangeCount()
        {
            var items = Enumerable.Range(1, 200).AsQueryable();
            var randomizer = new Random();
            var currentPage = randomizer.Next(197, 200);
            IPagedList pagedList = PagingExtensions.ToPagedList(items, 1, currentPage);

            var expectedPagesInRange = 3;
            var pageRange = pagedList.GetPageRange(expectedPagesInRange);

            Assert.AreEqual(currentPage + 1, pageRange.Last());
        }

        [TestMethod]
        public void TestPagedListHalfOfPageRangeIsAfterCurrentPageEvenPageRangeCount()
        {
            var items = Enumerable.Range(1, 200).AsQueryable();
            var randomizer = new Random();
            var currentPage = randomizer.Next(90, 110);
            IPagedList pagedList = PagingExtensions.ToPagedList(items, 1, currentPage);

            var expectedPagesInRange = 4;
            var pageRange = pagedList.GetPageRange(expectedPagesInRange);

            Assert.AreEqual(currentPage + 1, pageRange.Last());
        }
    }
}
