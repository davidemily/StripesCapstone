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
                    $"({request.RideId}, {request.PatronName}, {request.PhoneNumber}, '1', {request.Pickup}, {request.Destination}, 'waiting', NOW(), 'iphone', 1)";
                if (comm.ExecuteNonQuery() != 1)
                {
                    Console.WriteLine("did not modify database");
                    throw new Exception("did not modify database");
                }
                CloseConnection();
            }
        }

    }
}
