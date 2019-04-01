using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CodingProject
{
    public class Address
    {
        private SqlConnection dbConnection;
        private int id;
        private string streetOne;
        private string streetTwo;
        private string city;
        private string state;
        private string zipCode;
        private int userID;

        public int Id { get => id; set => id = value; }
        public string StreetOne { get => streetOne; set => streetOne = value; }
        public string StreetTwo { get => streetTwo; set => streetTwo = value; }
        public string City { get => city; set => city = value; }
        public string State { get => state; set => state = value; }
        public string ZipCode { get => zipCode; set => zipCode = value; }
        public int UserID { get => userID; set => userID = value; }

        public Address(string streetOne = null, string streetTwo = null, string City = null, string State = null, string ZipCode = null, SqlConnection dbConnection = null)
        {
            this.dbConnection = dbConnection;
            StreetOne = streetOne;
            StreetTwo = streetTwo;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public bool Save()
        {
            int rowsAffected;
            string insertQuery;

            return false;
        }

        public static List<Address> All(SqlConnection addConnection)
        {
            string allQuery = "SELECT * FROM address;";
            var addresses = new List<Address>();
            var reader = new SqlCommand(allQuery, addConnection);


            SqlDataReader result = reader.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    var address = new Address
                    {
                        Id = Convert.ToInt32(result["id"]),
                        StreetOne = result["streetOne"].ToString(),
                        StreetTwo = result["streetTwo"].ToString(),
                        City = result["city"].ToString(),
                        State = result["state"].ToString(),
                        ZipCode = result["zipCode"].ToString()
                    };
                    addresses.Add(address);
                }
            }
            addConnection.Close();
            return addresses;
        }
    }
}
