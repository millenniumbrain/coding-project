using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CodingProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string masterCat = "Initial Catalog=master;";
            string dataSrc = "Data Source=(local);";
            string intSec = "Integrated Security=SSPI;";
            string masterConnString = masterCat + dataSrc + intSec;
            try
            {
                System.Diagnostics.Debug.WriteLine("Connecting to SQL Server...");
                using (SqlConnection masterConnection = new SqlConnection(masterConnString))
                {
                    CreateInterviewDB(masterConnection);
                }
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error);
            }
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        // check if interview database exists and if not create it...
        // in order to do this a connection to the master db needs to be established
        // after that we can create the database or just 
        private static void CreateInterviewDB(SqlConnection dbConnection)
        {
            System.Diagnostics.Debug.WriteLine("Checking if interview database exists.");
            try
            {
                string selectInventory = "SELECT * FROM master.dbo.sysdatabases WHERE name = 'interview'";
                var cmd = new SqlCommand(selectInventory, dbConnection);
                dbConnection.Open();
                object result = cmd.ExecuteScalar();
                dbConnection.Close();

                if (result == null)
                {
                    System.Diagnostics.Debug.WriteLine("interview table does not exist! Creating.....");
                    dbConnection.Open();
                    SqlCommand createInterview = dbConnection.CreateCommand();
                    createInterview.CommandText = "CREATE DATABASE interview";
                    createInterview.ExecuteNonQuery();
                    dbConnection.Close();

                    System.Diagnostics.Debug.WriteLine("interview created! Creating tables!");
                    string createInterviewScript = File.ReadAllText(@"InterviewDB.sql");
                    dbConnection.Open();
                    SqlCommand createTables = dbConnection.CreateCommand();
                    createTables.CommandText = createInterviewScript;
                    createTables.ExecuteNonQuery();
                    dbConnection.Close();

                    System.Diagnostics.Debug.WriteLine("Added users and addresses table to interview!");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("interview database already exists!");
                }
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error);
            }
        }
    }
}
