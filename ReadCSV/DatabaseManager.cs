//Exercise 2: You have another class that accepts the list and insters the data into the database

using System.Data.SqlClient;
public class DatabaseManager
{
    private string _connectionString; // Private field to store the database connection string

    public DatabaseManager(string connectionString) // Constructor to initialize DatabaseManager with a connection string
    {
        _connectionString = connectionString; // Assign the provided connection string to the private field
    }

    public int GetLastId()
    {
        try
        {
            int lastId = 0; // Initialize the variable to hold the last ID value

            // SQL query to select the maximum ID from the table
            string query = "SELECT MAX(ID) FROM [GTMS_Test].[dbo].[tblAicomEreignisse]";

            // Open a connection to the database using the connection string
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open(); // Open the database connection

                // Create a SqlCommand object with the query and connection
                SqlCommand command = new SqlCommand(query, connection);

                // Execute the SqlCommand to retrieve the maximum ID value
                object result = command.ExecuteScalar();

                // Check if the result is not null and convert it to an integer
                if (result != DBNull.Value)
                {
                    lastId = Convert.ToInt32(result);
                }
            }

            return lastId; // Return the last ID value
        }
        catch (Exception ex)
        {
            // If an exception occurs during database operation, print an error message
            Console.WriteLine("An error occurred while getting the last ID: " + ex.Message);
            return 0; // Return 0 indicating failure
        }
    }

    public void InsertRecords(List<Synopresult> records) // Method to insert records into the database
    {
        try
        {
            // Open a connection to the database using the connection string
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open(); // Open the database connection

                int id = GetLastId() + 1; // Get the last ID and increment it to start inserting records

                // Loop through each Synopresult object in the records list
                foreach (var record in records)
                {
                    // SQL query to insert data into the database table
                    string query = @"INSERT INTO [GTMS_Test].[dbo].[tblAicomEreignisse] 
                                    ([ID], [ProgramName], [Feature], [MonitoredSignals], [SerialNr], 
                                     [StartTime], [Duration], [Completed], 
                                     [Stability], [Comment])
                                    VALUES 
                                    (@ID, @ProgramName, @Feature, @MonitoredSignals, @SerialNr, 
                                     @StartTime, @Duration, @Completed, 
                                     @Stability, @Comment)";

                    SqlCommand command = new SqlCommand(query, connection); // Create a SqlCommand object with the query and connection
                    command.Parameters.AddWithValue("@ID", id); // Add parameters to the SqlCommand
                    command.Parameters.AddWithValue("@ProgramName", record.ProgramName);
                    command.Parameters.AddWithValue("@Feature", record.Feature);
                    command.Parameters.AddWithValue("@MonitoredSignals", record.MonitoredSignals);
                    command.Parameters.AddWithValue("@SerialNr", record.SerialNr);
                    command.Parameters.AddWithValue("@StartTime", record.StartTime);
                    command.Parameters.AddWithValue("@Duration", record.Duration);
                    command.Parameters.AddWithValue("@Completed", record.Completed);
                    command.Parameters.AddWithValue("@Stability", record.Stability);
                    command.Parameters.AddWithValue("@Comment", record.Comment);

                    command.ExecuteNonQuery(); // Execute the SqlCommand to insert the record into the database
                    id++; // Increment the ID for the next record
                }
            }

            Console.WriteLine("Data inserted successfully."); // Display success message
        }
        catch (Exception ex)
        {
            // If an exception occurs during database operation, print an error message
            Console.WriteLine("An error occurred while inserting records: " + ex.Message);
        }
    }
}


