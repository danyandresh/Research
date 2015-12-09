using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
            try
            {
                base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}
