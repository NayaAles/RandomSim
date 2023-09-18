using System.Reflection;
using System.Text.RegularExpressions;

namespace RandomSim
{
    public static class ReaderWriterCsv
    {
        // if class.fieldName.Contains("Date") => parse to DateTime
        // if class.fieldName.Contains("Id") => not included
        public static List<T> ReadFromCsv<T>(string path, char separator)
        {
            var inDatas = new List<T>();

            var fields = GetFields<T>()
                .Where(x => !x.Contains("<DegreeOfInfluence>"))
                .ToList();
            var fieldsCount = fields.Count;

            using (var reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!String.IsNullOrEmpty(line))
                        inDatas.Add(ConvertToData<T>(fields, fieldsCount, line, separator));
                }
            }

            return inDatas;
        }

        private static T ConvertToData<T>(List<string> fields, int fieldsCount, string line, char separator)
        {
            var item = (T)Activator.CreateInstance(typeof(T));

            var array = Regex.Matches(Regex.Replace(line, @$"{separator}{separator}", $"{separator} {separator}")
            .ToString(), $"\"([^\"]+).|[^{separator}]+")
            .Cast<Match>()
            .Select(x => Regex.Replace(x.ToString(), "\"", string.Empty))
            .ToArray();

            for (int i = 0; i < fieldsCount; i++)
            {
                var rightData = array[i].Equals(" ") ? null : array[i]
                    .Trim();
                if (fields[i].Contains("Date"))
                    item.GetType()
                        .GetField(fields[i], BindingFlags.Instance | BindingFlags.NonPublic)
                        .SetValue(item, DateTime.Parse(rightData));
                else if (fields[i].Contains("DegreeOfInfluence"))
                    item.GetType()
                        .GetField(fields[i], BindingFlags.Instance | BindingFlags.NonPublic)
                        .SetValue(item, 0.0);
                else
                    item.GetType()
                        .GetField(fields[i], BindingFlags.Instance | BindingFlags.NonPublic)
                        .SetValue(item, rightData);
            }

            return item;
        }

        // Экранирование if class.field.Contains(separator)
        public static void SaveToCsv<T>(List<T> outDatas, string pathOut, char separator)
        {
            var fields = GetFields<T>()
                .Where(x => !x.Contains("<Id>"))
                .ToList();
            var fieldsCount = GetFields<T>().Count();

            using (var writer = new StreamWriter(pathOut))
            {
                string titles = String.Join(separator, fields.Select(x => Regex.Match(x, @"<([^>])+")
                        .Groups[1]
                        .ToString()));

                writer.WriteLine(titles);

                foreach (var data in outDatas)
                {
                    string exit = "";

                    for (int i = 0; i < fieldsCount; i++)
                    {
                        string value = data.GetType()
                            .GetField(fields[i], BindingFlags.Instance | BindingFlags.NonPublic)
                            .GetValue(data)
                            .ToString();

                        if (i == fieldsCount - 1)
                            exit = String.Concat(exit, value.Contains(separator) ? $"\"{value}\"" : value);
                        else
                            exit = String.Concat(exit, value.Contains(separator) ? $"\"{value}\"" : value, separator);
                    }

                    writer.WriteLine(exit);
                }
            }
        }

        private static string[] GetFields<T>()
        {
            var fields = typeof(T).GetRuntimeFields()
                .Select(x => x.Name)
                .ToArray();

            return fields;
        }
    }
}
