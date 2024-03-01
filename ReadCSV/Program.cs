//Exercise 3: The Program.cs file is there to facilitate the interaction between the two classes: it first instantiates both objects: the CSV reader and the DB writer. In then invokes the method on the CSV reader object to get the list from the CSV file. Then it passes this list to the DB writer.

using ReadCSV;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

var csvFilePath = config["CsvFilePath"];
var connectionString = config["ConnectionString"];

// Create an instance of CsvParser and initialize it with the CSV file path
CsvParser csvParser = new(csvFilePath);

// Parse the CSV file and get a list of Synopresult objects
// Parse the CSV file and get a list of Synopresult objects
List<Synopresult> records = csvParser.ParseCsv();

// Create an instance of DatabaseManager and initialize it with the database connection string
DatabaseManager dbManager = new(connectionString);

// Insert the parsed records into the database
dbManager.InsertRecords(records);

// Print a message to the console
Console.WriteLine("Data inserted successfully.");
