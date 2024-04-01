using Microsoft.EntityFrameworkCore;
using Training2._1.Data;
using Training2._1.Models;
using Training2._1.Repo.Interface;

namespace Training2._1.Repo.Implement
{
    public class StudentRepo : MainRepo<Student>, IStudentRepo
    {
        public StudentRepo(MyDbContext dbContext) : base(dbContext)
        {

        }

        public bool AnyStudent()
        {
            return db.Set<Student>().Any();
        }

        //public List<Student> GetIdByT10()
        //{
        //    return db.Students.Where(x => x.Id > 10).ToList();
        //}

        public int GetStudentCount()
        {
            return db.Set<Student>().Count();
        }

        public List<Student> WithDept()
        {
            return db.Students.Include(x=>x.Dpt).ToList();
        }
    }
}
