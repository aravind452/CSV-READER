using MongoDB.Bson;
using MongoDB.Driver;


namespace CSV_READER.PARSER
{
    public class CsvParser
    {
        public static List<Dictionary<string, string>> ParseCsv(string filePath, char delimiter)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File Path can not be empty or null ", nameof(filePath));
            }

            var result = new List<Dictionary<string, string>>();

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    var headers = reader.ReadLine()?.Split(delimiter);

                    if (headers == null || headers.Length == 0)
                    {
                        throw new Exception("CSV File is empty or has no headers");
                    }

                    while (!reader.EndOfStream)
                    {
                        var values = reader.ReadLine()?.Split(delimiter);
                        if (values == null || values.Length != headers.Length)
                        {
                            throw new Exception("Row does not match the header format");
                        }
                        var record = headers.Zip(values, (header, value) => new { header, value }).ToDictionary(x => x.header, x => x.value);
                        result.Add(record);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading CSV : {ex.Message}");
            }
            return result;
        }

        //private static List<string> ParseLine(string line, char delimiter)
        //{
        //    var values = new List<string>();
        //    var current = new List<char>();
        //    bool inQuotes = false;

        //    for (int i = 0; i < line.Length; i++)
        //    {
        //        char c = line[i];

        //        if (c == '"' && (i + 1 < line.Length && line[i + 1] == '"'))
        //        {

        //            current.Add('"');
        //            i++;
        //        }
        //        else if (c == '"')
        //        {

        //            inQuotes = !inQuotes;
        //        }
        //        else if (c == delimiter && !inQuotes)
        //        {

        //            values.Add(new string(current.ToArray()));
        //            current.Clear();
        //        }
        //        else
        //        {

        //            current.Add(c);
        //        }
        //    }


        //    if (current.Count > 0)
        //    {
        //        values.Add(new string(current.ToArray()));
        //    }

        //    return values;
        //}

        public static void InsertCsvDataIntoMongoDB(string filePath, string connectionString, string dbName, string collectionName)
        {

            //var client = new MongoClient(connectionString);
            //var database = client.GetDatabase(dbName);
            //var collection = database.GetCollection<BsonDocument>(collectionName);


            //var records = ParseCsv(filePath);


            //foreach (var record in records)
            //{
            //    var bsonDocument = new BsonDocument();
            //    foreach (var field in record)
            //    {
            //        bsonDocument.Add(field.Key, field.Value);
            //    }

            //    collection.InsertOne(bsonDocument);
            //}

            //Console.WriteLine("Data inserted into MongoDB successfully!");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("MongoDB connection string cannot be null or empty!");
            }
            if (string.IsNullOrWhiteSpace(dbName))
            {
                throw new ArgumentException("Database name cannot be null or empty!");
            }
            if (string.IsNullOrWhiteSpace(collectionName))
            {
                throw new ArgumentException("Collection name cannot be null or empty!");
            }

            try
            {
                var records = ParseCsv(filePath, ';');

                var client = new MongoClient(connectionString);

                var database = client.GetDatabase(dbName);

                var collection = database.GetCollection<BsonDocument>(collectionName);

                var bsonDocuments = records.Select(record =>
                {
                    var bson = new BsonDocument();
                    foreach (var field in record)
                        bson.Add(field.Key, field.Value);
                    return bson;
                }).ToList();

                collection.InsertMany(bsonDocuments);

                Console.WriteLine("Data Inserted");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error inserting data : {ex.Message}");
            }
        }



    }
}




/*
  public static List<Dictionary<string, string>> ParseCsv(string filePath, char delimiter = ';')
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path can not be empty!", nameof(filePath));
            }
            var result = new List<Dictionary<string, string>>();
            string[]? headers = null;

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    string line;
                    bool isHeader = true;

                    while ((line = reader.ReadLine()) != null)
                    {
                        var values = ParseLine(line, delimiter);
                        if (isHeader)
                        {
                            headers = values.ToArray();
                            isHeader = false;

                        }
                        else
                        {
                            var record = new Dictionary<string, string>();
                            for (int i = 0; i < values.Count; i++)
                            {
                                record[headers[i]] = values[i];
                            }
                            result.Add(record);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading csv : {ex.Message}");
            }



            return result;
        }*/