using Training2._1.Repo.Interface;

namespace Training2._1.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ITeacherRepo teacherRepo { get; }
        IStudentRepo stdRepository { get; }
        IDepartmentRepo dptRepo { get; }
        Task Save();
    }
}
