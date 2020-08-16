using DHRM.BusinessEntity;
using DHRM.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DHRM.Controllers
{
    public class ProjectmanagementController : BaseController
    {
        ProjectManagementContext objDB = new DataAccess.Context.ProjectManagementContext();

        [HttpGet]
        public ActionResult Index()
        {
            var projects = objDB.GetAllProjects();
            return View(projects.AsEnumerable());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(FormCollection fc)
        {
            var f = fc;

            Project model = new Project();
            model.project_name = fc["ProjectName"];
            model.project_description = fc["ProjectDescription"];
            model.project_code = fc["ProjectCode"];
            model.project_start_date = Convert.ToDateTime(fc["StartDate"]);
            model.project_end_date = Convert.ToDateTime(fc["EndDate"]);
            model.client_id = getCompanyID(fc["CompanyName"].ToString());
            model.category_id = Convert.ToInt32(fc["CategoryName"]);

            Byte[] bytes = null;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                if (Request.Files.AllKeys[i].ToStr().ToUpper() == "LOGO")
                {
                    Stream fs = Request.Files[i].InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    bytes = br.ReadBytes((Int32)fs.Length);
                }
            }

            model.project_banner = bytes;


            bool result = objDB.SaveProject(model);
            if (result)
                return Json("Project Created");
            else
                return Json("Project not Created");
        }

        [HttpGet]
        public ActionResult AddProjectCategory()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AssignEmployee(int id)
        {
            ViewBag.ProjectID = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddProjectCategory(FormCollection fc)
        {
            var f = fc;

            ProjectCategory model = new ProjectCategory();
            model.CategoryName = fc["CategoryName"];
            model.CategoryDescription = fc["CategoryDescription"];

            bool result = objDB.SaveProjectCategory(model);

            if (result)
                return Json("Project Category Saved");
            else
                return Json("Project Category Not Saved");
        }

        public int getCompanyID(string companyName)
        {
            int ID = 1;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DHRM_COREEntities"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    con.Open();
                    cmd = new SqlCommand("select ID from tblCompany where CompanyName = @CompanyName", con);
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = companyName;
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ID = Convert.ToInt32(dr["ID"]);
                    }
                    con.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            return ID;
        }

        [HttpPost]
        public JsonResult getCompaniesList()
        {
            var result = objDB.GetCompaniesList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult getProjectCategory()
        {
            var result = objDB.GetProjectCategory();
            return Json(result);
        }

        [HttpPost]
        public JsonResult getEmployeesList()
        {
            List<Employee> result = new List<Employee>();
            try
            {
                result = objDB.GetEmployeeList();
            }
            catch (Exception ex)
            {
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult AssignedEmployees(FormCollection fc)
        {
            bool result = false;
            try
            {
                var SelectedEmployees = fc["SelectedEmployees"].Split(',');
                var ProjectID = Convert.ToInt32(fc["ProjectID"].ToString());

                foreach (string employeeID in SelectedEmployees)
                {
                    ProjectMemberAssisgnment model = new ProjectMemberAssisgnment();
                    model.ProjectId = ProjectID;
                    model.ProjectMemberId = Convert.ToInt32(employeeID);
                    result = objDB.SaveAssignedEmployees(model);
                }
            }
            catch (Exception ex)
            {

            }
            if (result)
                return Json("Project Category Saved");
            else
                return Json("Project Category Not Saved");
        }

        [HttpGet]
        public ActionResult EmployeeDetails(int id)
        {
            try
            {
                List<AssignedProjectEmployeeDetails> projects = objDB.GetAssignedProjectEmployeeDetails(id);
                ViewBag.ProjectName = projects.FirstOrDefault().Project.project_name;
                return View(projects);
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpGet]
        public ActionResult DeleteProject(int id)
        {
            ViewBag.ProjectID = id;
            return View();
        }

        [HttpPost]
        [ActionName("DeleteProject")]
        public ActionResult DeleteProjectPost(int id)
        {
            return View("Index");
        }

    }
}
