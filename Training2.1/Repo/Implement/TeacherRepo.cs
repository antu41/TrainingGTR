using Training2._1.Data;
using Training2._1.Models;
using Training2._1.Repo.Interface;

namespace Training2._1.Repo.Implement
{
    public class TeacherRepo : MainRepo<Teacher>, ITeacherRepo
    {
        public TeacherRepo(MyDbContext db) : base(db)
        {
        }
    }
}
