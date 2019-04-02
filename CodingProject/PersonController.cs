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
        public ActionResult Get()
        {
            string dataSrc = "Data Source=(local);";
            string intSec = "Integrated Security=SSPI;";
            string interviewConnString = "Initial Catalog=interview;" + dataSrc + intSec;
            using (var interviewConn = new SqlConnection(interviewConnString))
            {
                string json = JsonConvert.SerializeObject(Person.PersonAddresses(interviewConn), Formatting.Indented);
                return Content(json, "application/json");
            }
        }

        // POST: /person
        [HttpPost]
        public ActionResult Post(IFormCollection formData)
        {
            string dataSrc = "Data Source=(local);";
            string intSec = "Integrated Security=SSPI;";
            string interviewConnString = "Initial Catalog=interview;" + dataSrc + intSec;
            using (var interviewConn = new SqlConnection(interviewConnString))
            {   try
                {
                    var person = new Person
                    {
                        DBConnection = interviewConn,
                        FirstName = formData["firstName"],
                        LastName = formData["lastName"],
                        DOB = formData["dob"]
                    };
                    Person savedPerson = person.Save();
                    var address = new Address
                    {
                        DBConnection = interviewConn,
                        StreetOne = formData["streetOne"].ToString(),
                        StreetTwo = formData["streetTwo"].ToString(),
                        City = formData["city"].ToString(),
                        State = formData["state"].ToString(),
                        ZipCode = formData["zipCode"].ToString(),
                        PersonID = savedPerson.Id
                    };
                    savedPerson.Addresses.Add(address.Save());
                    var jsonMsg = new Dictionary<string, string>
                    {
                        {"msg","Personal Info was added to the database"}
                    };
                    string json = JsonConvert.SerializeObject(jsonMsg);
                    return Content(json, "application/json");
                }
                catch (Exception error)
                {
                    System.Diagnostics.Debug.WriteLine(error);
                    var jsonMsg = new Dictionary<string, string>
                    {
                        {"msg","Error adding info to database. Please try again."}
                    };
                    string json = JsonConvert.SerializeObject(jsonMsg);
                    return Content(json, "application/json");
                }
            }
        }
    }
}
