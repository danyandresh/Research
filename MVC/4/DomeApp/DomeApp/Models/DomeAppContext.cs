using System.Data;
using System.Data.Entity;
using System.Linq;

namespace DomeApp.Models
{
    public class DomeAppContext : DbContext, IRepository
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<DomeApp.Models.DomeAppContext>());

        public DomeAppContext()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>();
        }

        public void Add<T>(T entity) where T : class
        {
            Set<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void Remove<T>(T entity) where T : class
        {
            Set<T>().Remove(entity);
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }
    }
}
