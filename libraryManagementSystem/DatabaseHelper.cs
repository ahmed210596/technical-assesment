using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;

namespace libraryManagementSystem
{
    public class DatabaseHelper
    {
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LibraryDBConnection"].ConnectionString;

        // Method to execute a query (e.g., INSERT, UPDATE, DELETE)
        public static int ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }
        public static object ExecuteScalar(string query, SqlParameter[] parameters)
        {
            object result = null;

            // Get the connection string from web.config

            // Use a using statement to ensure the connection is disposed of properly
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the database connection
                    connection.Open();

                    // Create a SqlCommand object with the query and connection
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the command (if any)
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        // Execute the query and get the result
                        result = command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (log or rethrow)
                    throw new Exception("Database error: " + ex.Message, ex);
                }
                // The connection is automatically closed when the using block exits
            }

            return result;
        }

        // Method to retrieve data (e.g., SELECT)
        public static DataTable GetData(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

    }
}