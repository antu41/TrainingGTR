using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System.Data;
using System.Data.OleDb;
using System.Linq.Expressions;
using Training2._1.Data;
using Training2._1.Models;
using Training2._1.Repo.Interface;


namespace Training2._1.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepo repo;
        private readonly IDepartmentRepo drepo;
        private readonly IConfiguration config;

        public StudentController(IStudentRepo Repo, IDepartmentRepo Drepo, IConfiguration config)
        {
            this.repo = Repo;
            drepo = Drepo;
            this.config = config;
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
            //var studentsWithDept = repo.WithDept().ToList();
            //return View(studentsWithDept);
            return View();
        }


        public IActionResult Create()
        {
            ViewBag.Dept = new SelectList(drepo.GetAll(), "Id", "Name");
            return View();
        }

        [HttpGet]
        public IActionResult LoadData(int page = 1, decimal size = 5)
        {
            // Calculate the number of records to skip
            var skipCount = (page - 1) * (int)size;

            // Retrieve only the data for the current page
            var std = repo.WithDept()
                           .Skip(skipCount)
                           .Take((int)size)
                           .ToList();

            // Calculate total record count
            decimal totalRecordCount = repo.GetAll().Count();

            // Calculate total number of pages
            var pageCount = Math.Ceiling(totalRecordCount / size);

            return Json(new { last_page = pageCount, std });
        }


        [HttpPost]
        public IActionResult Create(Student item)
        {
            if (ModelState.IsValid)
            {
                repo.Add(item);
                repo.Save();
                return RedirectToAction("Index");
            }

            return View(item);

        }



        //public IActionResult Delete(int Id)
        //{
        //    //Expression<Func<Student, bool>> predicate = s => s.Id == Id;
        //    //repo.Delete(predicate);
        //    //repo.Save();
        //    //return RedirectToAction("Index");


        //}


        public IActionResult Upload()
        {

            return View();
        }


        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    // Define the path to save the uploaded file
                    var mainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel");
                    if (!Directory.Exists(mainPath))
                    {
                        Directory.CreateDirectory(mainPath);
                    }
                    var filePath = Path.Combine(mainPath, file.FileName);

                    // Save the uploaded file to the server
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    // Define connection string based on file extension
                    string extension = Path.GetExtension(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls":
                            conString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={filePath};Extended Properties='Excel 8.0;HDR=Yes;'";
                            break;
                        case ".xlsx":
                            conString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties='Excel 12.0 Xml;HDR=Yes;'";
                            break;
                        default:
                            throw new Exception("Invalid file format");
                    }

                    // Read data from Excel file into DataTable
                    DataTable dt = new DataTable();
                    using (OleDbConnection conExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = conExcel;
                                conExcel.Open();
                                DataTable dtExcelSchema = conExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                cmdExcel.CommandText = $"SELECT * FROM [{sheetName}]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                conExcel.Close();
                            }
                        }
                    }

                    // Create a mapping of department names to IDs
                    Dictionary<string, int> departmentMapping = new Dictionary<string, int>();
                    string dbConString = config.GetConnectionString("dbcs");
                    using (SqlConnection con = new SqlConnection(dbConString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM Departments", con))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    departmentMapping.Add(reader["Name"].ToString(), (int)reader["Id"]);
                                }
                            }
                        }
                    }

                    // Transform the DataTable
                    foreach (DataRow row in dt.Rows)
                    {
                        string departmentName = row["Department"].ToString();
                        if (departmentMapping.ContainsKey(departmentName))
                        {
                            row["Department"] = departmentMapping[departmentName];
                        }
                        else
                        {
                            throw new Exception($"Department '{departmentName}' not found in the database.");
                        }
                    }

                    // Perform the bulk copy
                    using (SqlConnection con = new SqlConnection(dbConString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            sqlBulkCopy.DestinationTableName = "Students";
                            sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                            sqlBulkCopy.ColumnMappings.Add("Department", "DptId");

                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }

                    ViewBag.message = "Uploaded and Saved!";
                    return RedirectToAction("Create");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                string msg = ex.Message;
                ViewBag.message = "An error occurred: " + msg;
            }

            return View();
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
