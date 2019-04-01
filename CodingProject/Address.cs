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
        private int personID;

        public int Id { get => id; set => id = value; }
        public string StreetOne { get => streetOne; set => streetOne = value; }
        public string StreetTwo { get => streetTwo; set => streetTwo = value; }
        public string City { get => city; set => city = value; }
        public string State { get => state; set => state = value; }
        public string ZipCode { get => zipCode; set => zipCode = value; }
        public int PersonID { get => personID; set => personID = value; }
        public SqlConnection DBConnection { get => dbConnection; set => dbConnection = value; }

        public Address(int personID = 0, string streetOne = null, string streetTwo = null, string City = null, string State = null, string ZipCode = null, SqlConnection dbConnection = null)
        {
            this.DBConnection = dbConnection;
            StreetOne = streetOne;
            StreetTwo = streetTwo;
            City = city;
            State = state;
            ZipCode = zipCode;
            PersonID = personID;
        }

        public Address Save()
        {
            int rowsAffected;

            string insertQuery = $"INSERT INTO addresses (streetOne, streetTwo, city, state, zipCode, personID) Values('{StreetOne}', '{StreetTwo}', '{City}', '{State}', '{zipCode}', '{PersonID}');";

            try
            {
                var insertCommand = new SqlCommand(insertQuery, DBConnection);
                DBConnection.Open();

                rowsAffected = (int)insertCommand.ExecuteNonQuery();

                if (rowsAffected == 1)
                {
                    string lastQuery = "SELECT TOP 1 id AS addID, streetOne, streetTwo, city, state, zipCode, personID FROM addresses ORDER BY personID DESC;";
                    Address address = null;
                    try
                    {
                        var reader = new SqlCommand(lastQuery, DBConnection);
                        SqlDataReader result = reader.ExecuteReader();
                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                address = new Address
                                {
                                    Id = Convert.ToInt32(result["personID"]),
                                    StreetOne = result["streetOne"].ToString(),
                                    StreetTwo = result["streetTwo"].ToString(),
                                    City = result["city"].ToString(),
                                    State = result["state"].ToString(),
                                    ZipCode = result["zipCode"].ToString(),
                                    PersonID = Convert.ToInt32(result["personID"])
                                };
                                DBConnection.Close();
                                return address;

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
                System.Diagnostics.Debug.WriteLine(error);
                return null;
            }

            return null;
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
