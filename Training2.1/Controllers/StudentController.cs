using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using Training2._1.Models;
using Training2._1.Repo.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Training2._1.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepo repo;
        private readonly IDepartmentRepo drepo;

        public StudentController(IStudentRepo Repo, IDepartmentRepo Drepo)
        {
            this.repo = Repo;
            drepo = Drepo;
        }
        //public IActionResult Index()
        //{
        //    var std = repo.GetAll();
        //     var list = repo.WithDept();
        //    return Ok(new{ std,list});
        //}
        [HttpGet]
        public IActionResult Index()
        {
            var studentsWithDept = repo.WithDept().ToList();
            return View(studentsWithDept);
        }


        public IActionResult Create()
        {
            ViewBag.Dept = new SelectList(drepo.GetAll(), "Id", "Name");
            return View();
        }

        [HttpGet]
        public IActionResult LoadData(int page =1, decimal size = 10 )
        {
            var std = repo.WithDept().ToList();
            decimal TotalRecordCount = repo.GetAll().Count();
            var PageCountabc = decimal.Parse((TotalRecordCount / size).ToString());
            var PageCount = Math.Ceiling(PageCountabc);
            return Json(new{ last_page = PageCount, std });
        }

        [HttpPost]
        public IActionResult Create(Student item)
        {
            if(ModelState.IsValid)
            {     
            repo.Add(item);
            repo.Save();
            return RedirectToAction("Create");
            }

            return View(item);
            
        }



        public IActionResult Delete(int Id)
        {
            Expression<Func<Student,bool>>predicate = s => s.Id == Id;
            repo.Delete(predicate);
            repo.Save();
            return RedirectToAction("Index");

        }

        public IActionResult Edit(int Id)
        {   
            Expression<Func<Student,bool>> predicate = s => s.Id == Id;
            var student = repo.GetBysingle(predicate);
            ViewBag.Student = student;
            ViewBag.Dept = new SelectList(drepo.GetAll(), "Id", "Name");
            return View();

        }

        //public IActionResult Edit()
        //{
        //    ViewBag.Dept = new SelectList(drepo.GetAll(), "Id", "Name");
        //    return View();
        //}

        [HttpPost]
        public IActionResult Edit(Student item)
        {
            repo.Update(item);
            repo.Save();
            return RedirectToAction("Create"); 

        }

        public IActionResult StdExists()
        {
            bool std = repo.AnyStudent();
            return Ok(std);
        }

        public IActionResult TotalStd()
        {
            var std = repo.GetStudentCount();
            return Ok(std);
        }
    }
}
