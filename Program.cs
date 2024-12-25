

namespace CSV
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string filePath = @"D:\C SHARP PROJECTS - LIB\CSV-READER\Example-CSV\example.csv";

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            var rows = CsvParser.ParseCsv(filePath);


            foreach (var row in rows)
            {
                Console.WriteLine(string.Join("|", row));
            }
        }
    }
}