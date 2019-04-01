using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CodingProject
{
    [Route("/person")]
    public class PersonController : Controller
    {
        // GET: /person
        [HttpGet]
        public string Get()
        {
            string dataSrc = "Data Source=(local);";
            string intSec = "Integrated Security=SSPI;";
            string interviewConnString = "Initial Catalog=interview;" + dataSrc + intSec;
            using (var interviewConn = new SqlConnection(interviewConnString))
            {
                string json = JsonConvert.SerializeObject(Person.PersonAddresses(interviewConn), Formatting.Indented);
                return json;
            }
        }

        // POST: /person
        [HttpPost]
        public string Post(IFormCollection formData)
        {
            return formData["firstName"].ToString() + " " + formData["lastName"].ToString();
        }
    }
}
