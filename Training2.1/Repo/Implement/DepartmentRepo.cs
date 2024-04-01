using Training2._1.Data;
using Training2._1.Models;
using Training2._1.Repo.Interface;

namespace Training2._1.Repo.Implement
{
    public class DepartmentRepo : MainRepo<Department>, IDepartmentRepo
    {
        public DepartmentRepo(MyDbContext db) : base(db)
        {
        }
    }
}
