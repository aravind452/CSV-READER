using CSV_READER.PARSER;

namespace CSV_READER
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string filePath = @"D:\C SHARP PROJECTS - LIB\CSV-READER\Example-CSV\example.csv";


            string connectionString = "mongodb://localhost:27017";
            string dbName = "csv";
            string collectionName = "examplecsv";

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            CsvParser.InsertCsvDataIntoMongoDB(filePath, connectionString, dbName, collectionName);



        }
    }
}