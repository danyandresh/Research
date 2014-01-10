using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomeApp.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ToPagedList<T>(this IQueryable<T> source, int pageCount, int pageSize)
        {
            return source.Skip((pageCount - 1) * pageSize).Take(pageSize);
        }
    }
}