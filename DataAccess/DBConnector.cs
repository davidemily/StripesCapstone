using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using API.Models;
using MySql.Data.MySqlClient;
using Swashbuckle.AspNetCore.Swagger;

namespace API.DataAccess
{
    public class DBConnector
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        // Constructor
        public DBConnector() 
        {
            Initialize();
        }

        // Initialize values
        private void Initialize() 
        {
            server = "localhost";
	        uid = "stripessql";
            database = "stripes";
            password = "stripessqldatabase";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "uid=" + uid + ";" + "password=" + password + ";" + "database=" + database + ";" + "sslmode=none;";
        
            connection = new MySqlConnection(connectionString);
        }

        // open connection to database
        public bool OpenConnection()
        {
            try 
            {
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Close connection
        public bool CloseConnection()
        {
            try 
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void InsertRide(RideRequest request) 
        {
            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();

                comm.CommandText =
                    "INSERT INTO RIDES (RideId, PatronName, PhoneNumber, NumberOfPeople, Pickup, Destination, Status, TimeRequested, RequestSource,NIGHTS_NightId)" +
                    " VALUES " +
                    $"({request.RideId}, '{request.PatronName}', '{request.PhoneNumber}', '{request.NumberOfPeople}', '{request.Pickup}', '{request.Destination}', 'waiting', NOW(), 'iphone', 1)";
                if (comm.ExecuteNonQuery() != 1)
                {
                    Console.WriteLine("did not modify database");
                    throw new Exception("did not modify database");
                }
                CloseConnection();
            }
        }

        /* unsure if i need to connect this to database at all
        public void LeaveComment(RideRequest request) 
        {
            //figure out what the heck to do 
        }*/

        
        public List<string> getNightStatus() 
        {
            // creating a list to store the result
            List<string> list = new List<string>();

            string query = "Select * from NIGHTS where Night='2018-10-21'";
            // get the current date and query the database to see if the night is currently active
            // however for testing purposes going to be using a static entry in the database...

            if (OpenConnection())
            {
                // create the command
                try {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    // create a data reader and execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    // read the data and store them in the list
                    while(dataReader.Read())
                    {
                        list.Add(dataReader["NightId"] + "");
                        list.Add(dataReader["Night"] + "");
                        list.Add(dataReader["IsActive"] + "");
                    }
                    // close the data reader
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
                //close the connection
                CloseConnection();
                // return the list
                return list;
            }
            else 
            {
                return list;
            }
        }


        public bool IsNightActive(string todaysDate)
        {
            string query = $"Select * FROM NIGHTS WHERE IsActive='1' AND Night='{todaysDate}';";
            
            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                var response = comm.ExecuteReader();
                if (response.FieldCount > 0)
                {
                    return true;
                }
                CloseConnection();
            }
            return false;
        }

        public void CreateNewNight(string todaysDate)
        {
            string query = $"INSERT INTO NIGHTS (Night, IsActive) VALUES ('{todaysDate}', 1);";

            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                comm.ExecuteNonQuery();
                CloseConnection();
            }
            return;
        }

        public int GetUnassignedRides()
        {
            string query = "SELECT COUNT(*) FROM RIDES WHERE Status = 'waiting';";
            int result = 0;
            if (OpenConnection())
            {
                Console.WriteLine("connected to database");
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                var result1 = comm.ExecuteScalar().ToString();
                Console.WriteLine(result1);
                result = int.Parse(result1);
                CloseConnection();
            }
            else
            {
                Console.WriteLine("bork bork bork");
            }
            Console.WriteLine(result);
            return result;
        }
    }
}
