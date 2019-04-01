using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CodingProject
{
    public class Person
    {
        private SqlConnection dbConnection;
        private int id;
        private string firstName;
        private string lastName;
        private string dob;
        private List<Address> addresses;

        public int Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string DOB { get => dob; set => dob = value; }
        public List<Address> Addresses { get => addresses; set => addresses = value; }
        public SqlConnection DBConnection { get => dbConnection; set => dbConnection = value; }

        // set most properties to null as the default so that when we retrive people from the Database
        // we don't have to put all the business logic within the constructor. But can still create a new User object, modify its properites
        // In most cases the DB will handle the ID autoincrement and the assignment of the ID so just ignore it in the constructor
        public Person(string firstName = null, string lastName = null, string dob = null, SqlConnection dbConnection = null)
        {
            FirstName = firstName;
            LastName = lastName;
            DOB = dob;
            DBConnection = dbConnection;
            Addresses = new List<Address>();
        }

        public static List<Person> PersonAddresses(SqlConnection peopleConnection)
        {
            string allQuery = "SELECT people.id AS personID, people.firstName, people.lastName, people.dob, " +
                "addresses.id AS addID, addresses.streetOne, addresses.streetTwo, addresses.city, addresses.state, addresses.zipCode " +
                "FROM people LEFT JOIN addresses ON people.id = addresses.personID;";
            var people = new List<Person>();
            var reader = new SqlCommand(allQuery, peopleConnection);

            peopleConnection.Open();

            try
            {
                SqlDataReader result = reader.ExecuteReader();
                if (result.HasRows)
                {
                    int currentUserID = 0;
                    int userPos = 0;
                    while (result.Read())
                    {
                        var person = new Person
                        {
                            Id = Convert.ToInt32(result["personID"]),
                            FirstName = result["firstName"].ToString(),
                            LastName = result["lastName"].ToString(),
                            DOB = result["DOB"].ToString()
                        };

                        if (Convert.IsDBNull(result["addID"]) != true)
                        {
                            Address address = new Address()
                            {
                                Id = Convert.ToInt32(result["addID"]),
                                StreetOne = result["streetOne"].ToString(),
                                StreetTwo = result["streetTwo"].ToString(),
                                City = result["city"].ToString(),
                                State = result["state"].ToString(),
                                ZipCode = result["zipCode"].ToString()
                            };

                            if (currentUserID == person.Id)
                            {
                                people[userPos - 1].Addresses.Add(address);
                            }
                            else
                            {
                                people.Add(person);
                                people[userPos].Addresses.Add(address);
                                userPos++;
                                currentUserID = person.Id;
                            }
                        }
                        else
                        {
                            people.Add(person);
                        }

                    }
                }
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error);
            }

            peopleConnection.Close();
            return people;
        }

        public Person Save()
        {
            int rowsAffected;

            // data validation
            if (FirstName == "" || FirstName == null)
            {
                System.Diagnostics.Debug.WriteLine("First Name cannot be undefined or empty!");
                return null;
            }

            if (LastName == "" || LastName == null)
            {
                System.Diagnostics.Debug.WriteLine("Last Name cannot be undefined or empty!");
                return null;
            }

            if (DOB == "" || DOB == null)
            {
                System.Diagnostics.Debug.WriteLine("DOB cannot be undefined or empty!");
            }

            string insertQuery = $"INSERT INTO people (firstName, lastName, dob) Values('{FirstName}', '{LastName}', '{DOB}');";

            try
            {
                var insertCommand = new SqlCommand(insertQuery, DBConnection);
                DBConnection.Open();

                rowsAffected = (int)insertCommand.ExecuteNonQuery();

                if (rowsAffected == 1)
                {
                    string lastQuery = "SELECT TOP 1 people.id AS personID, people.firstName, people.lastName, people.dob FROM people ORDER BY personID DESC;";
                    Person person = null;
                    try
                    {
                        var reader = new SqlCommand(lastQuery, DBConnection);
                        SqlDataReader result = reader.ExecuteReader();
                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                person = new Person
                                {
                                    Id = Convert.ToInt32(result["personID"]),
                                    FirstName = result["firstName"].ToString(),
                                    LastName = result["lastName"].ToString(),
                                    DOB = result["DOB"].ToString()
                                };
                                System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(person, Formatting.Indented));
                                DBConnection.Close();
                                return person;

                            }
                        }
                    }
                    catch (Exception error)
                    {
                        System.Diagnostics.Debug.WriteLine(error);
                    }
                }

                DBConnection.Close();
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(FirstName + " " + LastName + " " + DOB);
                System.Diagnostics.Debug.WriteLine(error);
                return null;
            }

            return null;
        }

        public static Person Last(SqlConnection personConnection)
        {
            string allQuery = "SELECT TOP 1 people.id AS personID, people.firstName, people.lastName, people.dob FROM people ORDER BY personID DESC;";
            Person person = null;
            var reader = new SqlCommand(allQuery, personConnection);

            personConnection.Open();

            try
            {
                SqlDataReader result = reader.ExecuteReader();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        person = new Person
                        {
                            Id = Convert.ToInt32(result["personID"]),
                            FirstName = result["firstName"].ToString(),
                            LastName = result["lastName"].ToString(),
                            DOB = result["DOB"].ToString()
                        };
                        personConnection.Close();

                        return person;

                    }
                }
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error);
            }
            personConnection.Close();

            return person;
        }
    }
}
