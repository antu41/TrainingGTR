using System.Linq.Expressions;


namespace Training2._1.Repo.Interface
{
    public interface IMainRepo<T> where T : class
    {
        List<T> GetAll();
        T GetBysingle(Expression<Func<T, bool>> predicate);
        void Add(T item);
        void Update(T item);
        void Delete(Expression<Func<T, bool>> predicate);
        void Save();

    }
}
