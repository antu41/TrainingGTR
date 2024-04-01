using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Training2._1.Data;
using Training2._1.Repo.Interface;

namespace Training2._1.Repo.Implement
{
    public class MainRepo<T> : IMainRepo<T> where T : class
    {
        protected readonly MyDbContext db;

        public MainRepo(MyDbContext db)
        {
            this.db = db;
        }

        public void Add(T item)
        {
            db.Set<T>().Add(item);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var item = db.Set<T>().SingleOrDefault(predicate);
            if (item != null)
            {
                db.Set<T>().Remove(item);
            }
        }



        public List<T> GetAll()
        {
            return db.Set<T>().ToList();
        }

        public T GetBysingle(Expression<Func<T, bool>> predicate)
        {
            return db.Set<T>().SingleOrDefault(predicate);
        }



        public void Save()
        {
            db.SaveChanges();
        }        
        public void Update(T item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        //public void Single()
        //{
        //    throw new NotImplementedException();
        //}



        //public void Any()
        //{
        //    throw new NotImplementedException();
        //}

        //public int Count()
        //{
        //    return db.Set<T>().Count();
        //}  

        //public void FirstOrDefault()
        //{
        //    throw new NotImplementedException();
        //}      
        //public void Include(Expression<Func<T, bool>> predicate)
        //{

        //}
    }

}
