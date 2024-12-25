using System;


namespace CSV
{
    public class CsvParser
    {
        public static List<List<string>> ParseCsv(string filePath, char delimiter = ';')
        {
            var result = new List<List<string>>();

            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    result.Add(ParseLine(line, delimiter));
                }
            }

            return result;
        }

        private static List<string> ParseLine(string line, char delimiter)
        {
            var values = new List<string>();
            var current = new List<char>();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"' && (i + 1 < line.Length && line[i + 1] == '"'))
                {

                    current.Add('"');
                    i++;
                }
                else if (c == '"')
                {

                    inQuotes = !inQuotes;
                }
                else if (c == delimiter && !inQuotes)
                {

                    values.Add(new string(current.ToArray()));
                    current.Clear();
                }
                else
                {

                    current.Add(c);
                }
            }


            if (current.Count > 0)
            {
                values.Add(new string(current.ToArray()));
            }

            return values;
        }



    }
}

