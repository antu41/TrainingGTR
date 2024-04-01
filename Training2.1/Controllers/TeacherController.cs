using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Training2._1.Data;
using Training2._1.Models;
using Training2._1.Repo.Interface;

namespace Training2._1.Controllers
{
    public class TeacherController : Controller
    {
        protected readonly ITeacherRepo mainRepo;

        public TeacherController(ITeacherRepo MainRepo)
        {
            this.mainRepo = MainRepo;
        }

        public IActionResult Index()
        {
            var list = mainRepo.GetAll();
            return View(list);
        }

        [HttpPost]
        public IActionResult Create(Teacher item)
        {
            mainRepo.Add(item);
            mainRepo.Save();
            return View();
        }

        public IActionResult Delete(int Id)
        {
            Expression<Func<Teacher, bool>> predicate = s => s.Id == Id;
            mainRepo.Delete(predicate);
            return View();

        }

        public IActionResult Edit(int Id)
        {
            Expression<Func<Teacher, bool>> predicate = s => s.Id == Id;
            var student = mainRepo.GetBysingle(predicate);
            return View(student);

        }

        public IActionResult Update(Teacher item)
        {
            mainRepo.Update(item);
            mainRepo.Save();
            return View();
        }
    }
}
