using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Training2._1.Repo.Interface;

namespace Training2._1.Controllers
{
    public class DepartmentController : Controller
    {
        protected readonly IDepartmentRepo repo;

        public DepartmentController(IDepartmentRepo Repo)
        {
            repo = Repo;
        }
        public IActionResult Index()
        {
            ViewBag.Dept = new SelectList(repo.GetAll(), "Id", "Name");
            return View();
        }
    }
}
