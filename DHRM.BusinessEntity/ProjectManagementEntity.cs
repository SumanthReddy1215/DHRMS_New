using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace DHRM.BusinessEntity
{
    public class Project
    {
        public int project_Id { get; set; }

        public string project_code { get; set; }

        public int client_id { get; set; }

        public int category_id { get; set; }

        public string project_name { get; set; }

        public string project_description { get; set; }

        public byte[] project_banner { get; set; }

        public DateTime project_start_date { get; set; }

        public DateTime project_end_date { get; set; }

        public string project_remarks { get; set; }

        public DateTime Trans_Date { get; set; }

        public int User_Id { get; set; }

        public string ImagePath { get; set; }

    }

    public class ProjectCategory
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public int UserID { get; set; }
    }

    public class ProjectMemberAssisgnment
    {
        public int ProjectDetailsId {get;set;}

        public int ProjectId { get; set; }

        public int ProjectMemberId { get; set; }

        public int UserId { get; set; }
    }

    public class Employee
    {
        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }
    }

    public class AssignedProjectEmployeeDetails
    {
        public Project Project { get; set; }

        public Employee Employee { get; set; }
    }
}
