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
        private string username;
        private string firstName;
        private string lastName;
        private string dob;
        private List<Address> addresses;

        public int Id { get => id; set => id = value; }
        public string Username { get => username; set => username = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string DOB { get => dob; set => dob = value; }
        public List<Address> Addresses { get => addresses; set => addresses = value; }

        // set most properties to null as the default so that when we retrive people from the Database
        // we don't have to put all the business logic within the constructor. But can still create a new User object, modify its properites
        // In most cases the DB will handle the ID autoincrement and the assignment of the ID so just ignore it in the constructor
        public Person(string username = null, string firstName = null, string lastName = null, string dob = null, SqlConnection dbConnection = null)
        {
            this.dbConnection = dbConnection;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Addresses = new List<Address>();
        }

        public static List<Person> PersonAddresses(SqlConnection userConnection)
        {
            string allQuery = "SELECT people.id AS personID, people.username, people.firstName, people.lastName, people.dob, " +
                "addresses.id AS addID, addresses.streetOne, addresses.streetTwo, addresses.city, addresses.state, addresses.zipCode " +
                "FROM people LEFT JOIN addresses ON people.id = addresses.personID;";
            var people = new List<Person>();
            var reader = new SqlCommand(allQuery, userConnection);

            userConnection.Open();

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
                            Id = Convert.ToInt32(result["userID"]),
                            Username = result["username"].ToString(),
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

            userConnection.Close();
            return people;
        }

        public bool Save()
        {
            int rowsAffected;
            string insertQuery;

            // data validation
            if (Username == "" || Username == null)
            {
                System.Diagnostics.Debug.WriteLine("Username cannot be undefined or empty!");
                return false;
            }

            if (FirstName == "" || FirstName == null)
            {
                System.Diagnostics.Debug.WriteLine("First Name cannot be undefined or empty!");
                return false;
            }

            if (LastName == "" || LastName == null)
            {
                System.Diagnostics.Debug.WriteLine("Last Name cannot be undefined or empty!");
                return false;
            }

            if (DOB == "" || DOB == null)
            {
                System.Diagnostics.Debug.WriteLine("DOB cannot be undefined or empty!");
            }

            insertQuery = $"INSERT INTO people (username, firstName, lastName, dob) Values('{Username}', '{FirstName}', '{LastName}', '01/11/1997');";

            try
            {
                var insertCommand = new SqlCommand(insertQuery, dbConnection);
                dbConnection.Open();

                rowsAffected = (int)insertCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine(rowsAffected);
                insertCommand.Connection.Close();
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error);
                return false;
            }

            return true;
        }
    }
}
