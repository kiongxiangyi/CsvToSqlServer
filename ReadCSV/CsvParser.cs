//Exercise 1: You would have a class that reads the data from CSV and returns the list of Synopresult object

using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace ReadCSV;

public class CsvParser(string filePath)
{
    private readonly string _filePath = filePath; // Private field to store the file path

    public List<Synopresult> ParseCsv() // Method to parse the CSV file and return a list of Synopresult objects
    {
        List<Synopresult> records = []; // Initialize an empty list to store parsed records

        try
        {
            // Configure CsvHelper to handle CSV parsing with specified options
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",", // Set the delimiter to comma
                PrepareHeaderForMatch = args => args.Header.ToUpper(), // Convert header names to uppercase
            };

            // Open the CSV file for reading using StreamReader and CsvReader
            using var reader = new StreamReader(_filePath); // Open the file for reading
            using var csv = new CsvReader(reader, config); // Create CsvReader with the specified configuration

            // Parse the CSV file and convert each record to a Synopresult object, then convert to a list
            records = csv.GetRecords<Synopresult>().ToList();

        }
        catch (Exception ex)
        {
            // If an exception occurs during CSV parsing, print an error message
            Console.WriteLine("An error occurred while parsing CSV: " + ex.Message);
        }

        return records; // Return the list of parsed records
    }
}