using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using API.Models;
using MySql.Data.MySqlClient;

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
//                comm.Parameters.Add("@person", request.PatronName.ToString());
//                comm.Parameters.Add("@phone", request.PhoneNumber.ToString());
//                comm.Parameters.Add("@pickup", request.Pickup.ToString());
//                comm.Parameters.Add("@dropoff", request.Destination.ToString());
//                comm.CommandText = "INSERT INTO RIDES (RideId, PatronName, PhoneNumber, NumberOfPeople, Pickup, Destination, Status, TimeRequested, RequestSource,NIGHTS_NightId) VALUES " +
//                                   "(1, @name, @phone, '1', @pickup, @dropoff, 'waiting', NOW(), 'iphone', 1);";
                comm.CommandText =
                    "INSERT INTO RIDES (RideId, PatronName, PhoneNumber, NumberOfPeople, Pickup, Destination, Status, TimeRequested, RequestSource,NIGHTS_NightId)" +
                    " VALUES " +
                    //"(1, 'Test', '5555555', '1', 'Test', 'test', 'waiting', NOW(), 'iphone', 1)";
                    $"({request.RideId}, '{request.PatronName}', '{request.PhoneNumber}', '1', '{request.Pickup}', '{request.Destination}', 'waiting', NOW(), 'iphone', 1)";
                if (comm.ExecuteNonQuery() != 1)
                {
                    Console.WriteLine("did not modify database");
                    throw new Exception("did not modify database");
                }
                CloseConnection();
            }
        }

        
        public List<string>[] getNightStatus() 
        {
            // creating a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            string query = "SELECT * FROM NIGHTS";
            // get the current date and query the database to see if the night is currently active
            // however for testing purposes going to be using a static entry in the database...

            if (OpenConnection())
            {
                // MySqlCommand comm = connection.CreateCommand();
                // comm.CommandText = query;

                // create the command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                // create a data reader and execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                // read the data and store them in the list
                while(dataReader.Read())
                {
                    list[0].Add(dataReader["NightId"] + "");
                    list[1].Add(dataReader["Night"] + "");
                    list[2].Add(dataReader["IsActive"] + "");
                }

                // close the data reader
                dataReader.Close();
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
    }
}
