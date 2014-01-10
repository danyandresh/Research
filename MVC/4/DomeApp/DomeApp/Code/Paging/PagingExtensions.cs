using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomeApp.Code.Paging
{
    public static class PagingExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageSize, int currentPage = 1)
        {
            return new PagedList<T>(source, pageSize, currentPage);
        }
    }
}