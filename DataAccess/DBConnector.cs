using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);
                // When handling errors, we can show the application's response based on the error number.
                // The two most common error numbers when connecting are as follows:
                // 0: Cannot connect to server.
                // 1045: Invalid username and/or password.
                switch (ex.Number)
                {
                    case 0: 
                        Console.WriteLine("Cannot connect to server.");
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password");
                        break;
                }
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
        public void InsertRide() 
        {
            string query = "INSERT INTO tableinfo (name, age) VALUES('John Smith', '33')";

            // open connection
            if (this.OpenConnection() == true) 
            {
                // create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                // execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        // Update statement 
        public void Update()
        {
            string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

            if (this.OpenConnection() == true) 
            {
                // open connection
                MySqlCommand cmd = new MySqlCommand();
                // assign the query using CommandText
                cmd.CommandText = query;
                // assign the connection using Connection
                cmd.Connection = connection;
                // execute the query
                cmd.ExecuteNonQuery();

                // close connection
                this.CloseConnection();
            }
        }

        public List<string>[] Select()
        {
            string query = "SELECT * FROM CARS";

            // Creating a list to store the result
            List<string>[] list = new List<string>[4];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();

            // open connection
            if (this.OpenConnection() == true) 
            {
                // create command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                // create a data reader and execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                // read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["CarId"] + "");
                    list[1].Add(dataReader["CarNumber"] + "");
                    list[2].Add(dataReader["PhoneNumber"] + "");
                    list[3].Add(dataReader["IsActive"] + "");
                }

                // close data reader
                dataReader.Close();
                // close connection
                this.CloseConnection();
                // return list
                return list;            
            }
            else 
            {
                return list;
            }
        }

    }
}
