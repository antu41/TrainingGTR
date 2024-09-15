using Microsoft.EntityFrameworkCore;
using Training2._1.Repo.Implement;
using Training2._1.Repo.Interface;

namespace Training2._1.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly MyDbContext _db;

        public IStudentRepo stdRepository { get; set; }
        public IDepartmentRepo dptRepo { get; set; }
        public ITeacherRepo teacherRepo { get; set; }

        public UnitOfWork(MyDbContext context)
        {
            _db = context;
            stdRepository = new StudentRepo(context);
            dptRepo = new DepartmentRepo(context);
            teacherRepo = new TeacherRepo(context);

        }

        public virtual void Dispose() => _db?.Dispose();

        public virtual async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
