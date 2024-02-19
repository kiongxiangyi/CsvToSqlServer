﻿using CsvHelper;
using System.Globalization;
using System.Data.SqlClient;
using CsvHelper.Configuration; // Add this line

public class Synopresult
{
    public string ProgramName { get; set; }
    public string Feature { get; set; }
    public string MonitoredSignals { get; set; }
    public long SerialNr { get; set; }
    public long StartTime { get; set; }
    public int Duration { get; set; }
    public bool Completed { get; set; }
    public decimal Stability { get; set; }
    public string Comment { get; set; }
    //public string Comment2 { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=.\\SQL2016;Initial Catalog=GTMS_Test;User Id=sa;Password=freebsd;Integrated Security=True";

        // Path to your CSV file
        string csvFilePath = "D:\\Synop\\process_monitor\\synop_result\\synop_result.csv";

        try
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                PrepareHeaderForMatch = args => args.Header.ToUpper(),

            };
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<Synopresult>().ToList();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    int id = 15;

                    foreach (var person in records)
                    {
                        string query = @"INSERT INTO [GTMS_Test].[dbo].[tblAicomEreignisse] 
                                        ([ID], [ProgramName], [Feature], [MonitoredSignals], [SerialNr], 
                                         [StartTime], [Duration], [Completed], 
                                         [Stability], [Comment])
                                        VALUES 
                                        (@ID, @ProgramName, @Feature, @MonitoredSignals, @SerialNr, 
                                         @StartTime, @Duration, @Completed, 
                                         @Stability, @Comment)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ID", id);
                        command.Parameters.AddWithValue("@ProgramName", person.ProgramName);
                        command.Parameters.AddWithValue("@Feature", person.Feature);
                        command.Parameters.AddWithValue("@MonitoredSignals", person.MonitoredSignals);
                        command.Parameters.AddWithValue("@SerialNr", person.SerialNr);
                        command.Parameters.AddWithValue("@StartTime", person.StartTime);
                        command.Parameters.AddWithValue("@Duration", person.Duration);
                        command.Parameters.AddWithValue("@Completed", person.Completed);
                        command.Parameters.AddWithValue("@Stability", person.Stability);
                        command.Parameters.AddWithValue("@Comment", person.Comment);
                        command.ExecuteNonQuery();
                        id++;
                    }
                }
            }

            Console.WriteLine("Data inserted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
