using Training2._1.Models;

namespace Training2._1.Repo.Interface
{
    public interface IStudentRepo : IMainRepo<Student>
    {
        //List<Student> GetIdByT10();
        int GetStudentCount();
        bool AnyStudent();

        List<Student> WithDept();
    }
}
