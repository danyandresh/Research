using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DomeApp.Models
{
    public interface IRepository : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;

        void Add<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        void Remove<T>(T entity) where T : class;

        void SaveChanges();
    }
}