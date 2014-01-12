﻿using System.Collections.Generic;
using System.Linq;

namespace DomeApp.Code.Paging
{
    public class PagedList<T> : IEnumerable<T>, IPagedList
    {
        private IQueryable<T> source;

        public int TotalPages
        {
            get
            {
                return (int)(1 + source.Count() / PageSize);
            }
        }

        public int CurrentPage
        {
            get;
            set;
        }

        public bool HasNextPage
        {
            get
            {
                return CurrentPage < TotalPages;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }

        public void MoveNextPage()
        {
            if (!HasNextPage)
            {
                return;
            }

            CurrentPage += 1;
        }

        public void MovePreviousPage()
        {
            if (!HasPreviousPage)
            {
                return;
            }

            CurrentPage -= 1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var query = source;
            if (CurrentPage > 1)
            {
                query = query.Skip(CurrentPage * PageSize);
            }

            return query.Take(PageSize).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int PageSize
        {
            get;
            set;
        }

        public PagedList(IQueryable<T> source, int pageSize, int currentPage)
        {
            // TODO: Complete member initialization
            this.source = source;
            this.PageSize = pageSize;
            this.CurrentPage = currentPage;
        }
    }
}