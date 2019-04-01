using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodingProject
{
    [Route("/person/recent")]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            string dataSrc = "Data Source=(local);";
            string intSec = "Integrated Security=SSPI;";
            string interviewConnString = "Initial Catalog=interview;" + dataSrc + intSec;
            using (var interviewConn = new SqlConnection(interviewConnString))
            {
                string json = JsonConvert.SerializeObject(Person.Last(interviewConn));
                return Content(json, "application/json");
            }
        }
    }
}
