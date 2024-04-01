using Training2._1.Data;
using Training2._1.Models;
using Training2._1.Repo.Interface;

namespace Training2._1.Repo.Implement
{
    public class TeaherRepo : MainRepo<Teacher>, ITeacherRepo
    {
        public TeaherRepo(MyDbContext db) : base(db)
        {
        }
    }
}
