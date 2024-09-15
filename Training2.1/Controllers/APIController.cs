using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Linq.Expressions;
using Training2._1.Data;
using Training2._1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Training2._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public APIController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("WithDept")]
        public IActionResult WithDept()
        {
            var studentsWithDept = unitOfWork.stdRepository.WithDept();
            return Ok(studentsWithDept);
        }

        [HttpPut]
        [Route("Update")]
        public void Update(Student item)
        {
            unitOfWork.stdRepository.Update(item);
            unitOfWork.Save();
          //  return RedirectToAction("Create");

        }


        [HttpDelete]
        [Route("Delete/{Id}")] // Fix the route format
        public void Delete(int Id)
        {
            Expression<Func<Student, bool>> predicate = s => s.Id == Id;
            unitOfWork.stdRepository.Delete(predicate);
            unitOfWork.stdRepository.Save();
         //   return RedirectToAction("Index");

        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(Student item)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.stdRepository.Add(item);
                unitOfWork.Save();
              //  return RedirectToAction("Create");
            }

            return BadRequest();

        }

    }
}
