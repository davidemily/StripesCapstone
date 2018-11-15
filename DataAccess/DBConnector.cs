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
                Console.WriteLine(ex);
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

        public bool IsNightActive(string todaysDate)
        {
            string query = $"Select * FROM NIGHTS WHERE Night='{todaysDate}';";
            
            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                var response = comm.ExecuteReader();
                if (response.FieldCount > 0)
                {
                    CloseConnection();
                    return true;       
                }
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
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                var result1 = comm.ExecuteScalar().ToString();
                result = int.Parse(result1);
                CloseConnection();
            }
            return result;
        }

        public int GetAssignedRides()
        {
            string query = "SELECT COUNT(*) FROM RIDES WHERE Status = 'assigned';";
            int result = 0;
            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                var result1 = comm.ExecuteScalar().ToString();
                Console.WriteLine(result1);
                result = int.Parse(result1);
                CloseConnection();
            }
            return result;
        }

        public int GetRidingRides()
        {
            string query = "SELECT COUNT(*) FROM RIDES WHERE Status = 'riding';";
            int result = 0;
            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                var result1 = comm.ExecuteScalar().ToString();
                result = int.Parse(result1);
                CloseConnection();
            }

            return result;
        }

        public int GetNumberCarsRunning()
        {
            string query = "SELECT COUNT(*) FROM CARS WHERE IsActive = '1';";
            int result = 0;
            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                var result1 = comm.ExecuteScalar().ToString();
                result = int.Parse(result1);
                CloseConnection();
            }
            return result;
        }

        public void InsertNewCar(int carId, int carNumber, string phoneNumber, byte isActive)
        {
            string query = $"INSERT INTO CARS (CarId, CarNumber, PhoneNumber, IsActive) VALUES ('{carId}', '{carNumber}', '{phoneNumber}', '{isActive}'";

            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                if (comm.ExecuteNonQuery() != 1)
                {
                    Console.WriteLine("Did not modify database");
                    throw new Exception("did not modify database");
                }
                CloseConnection();
            }
            return;
        }
    }
}
