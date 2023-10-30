
using System.Text.RegularExpressions;

namespace RandomSim
{
    public static class Prediction
    {
        public static string Directory = CurrentDirectory.Get(4);
        public static string EventsPath = Directory + @"\events.csv";
        public static string AdjectivesPath = Directory + @"\adjectives.csv";

        public static void Menu(string fullName)
        {
            var key = ConsoleKey.Z;
            while (key != ConsoleKey.G || key != ConsoleKey.S)
            {
                Console.WriteLine("\nДля включения режима генерации нажмите <G>\n" +
                "Для включения режима поиска по событиям нажмите <S>\n");

                key = Console.ReadKey().Key;
                if (key == ConsoleKey.G)
                {
                    Prediction.Confirm(fullName, null);
                }
                else if (key == ConsoleKey.S)
                {
                    Console.WriteLine("\n\nВведите текст для поиска:");
                    var textToSearch = Regex.Replace(Console.ReadLine().ToString().ToLower(), @"[^а-я\s]+", string.Empty);

                    Prediction.Confirm(fullName, textToSearch);
                }
                else
                {
                    Console.WriteLine("\nВведен неверный ключ\n");
                }
            }
        }

        private static string Get(string fullName, string? textToSearch)
        {
            var random = new Random();
            var events = RandomPermutaion(ReaderWriterCsv.ReadFromCsv<Event>(EventsPath, ';'))
                .Where(z => !String.IsNullOrEmpty(z.Name))
                .GroupBy(x => x.Name)
                .Select(y => y.FirstOrDefault())
                .ToList();

            var adjectives = RandomPermutaion(ReaderWriterCsv.ReadFromCsv<Adjective>(AdjectivesPath, ';'))
                .Where(z => !String.IsNullOrEmpty(z.RandomAdjective))
                .GroupBy(x => x.RandomAdjective)
                .Select(y => y.FirstOrDefault())
                .ToList();

            var selectedEvent = new Event();
            if (!String.IsNullOrEmpty(textToSearch))
            {
                var eventName = events.Where(x => x.Name.Contains(textToSearch))
                    .Select(x => x.Name);
                
                if (eventName.Count() > 0)
                {
                    selectedEvent.Name = eventName.First();
                }
                else
                {
                    Console.WriteLine("\nError: Событие не обнаружено!\n" +
                        "Для продолжения нажмите <Enter>\n");

                    var next = Console.ReadKey().Key;
                    if (next == ConsoleKey.Enter)
                    {
                        Console.WriteLine("\n\nВведите текст для поиска:");
                        textToSearch = Regex.Replace(Console.ReadLine().ToString().ToLower(), @"[^а-я\s]+", string.Empty);

                        Prediction.Confirm(fullName, textToSearch);
                    }
                    else
                    {
                        Menu(fullName);
                    }
                }
            }
            else
            {
                selectedEvent = events[random.Next(0, events.Count - 1)];
            }

            var selectedAdjective = adjectives[random.Next(0, adjectives.Count)];

            selectedEvent.DegreeOfInfluence = Math.Round(random.NextDouble() * 100.0, 2);
            selectedEvent.NumberOfDescendants = random.Next(1, 5);

            var exit = $"\n\nПолучено предсказание для персонажа: {fullName}\n\n" +
                $"Событие: {selectedEvent.Name}, степень влияния: {selectedEvent.DegreeOfInfluence}%;\n" +
                $"Случайное прилагательное: {selectedAdjective.RandomAdjective};\n" +
                $"Количество потомков: {selectedEvent.NumberOfDescendants}";

            return exit;
        }

        private static void Confirm(string name, string? textToSearch)
        {
            var confirm = ConsoleKey.Z;
            while (confirm != ConsoleKey.Enter)
            {
                var exit = Prediction.Get(name, textToSearch);
                Console.WriteLine(exit);

                Console.WriteLine("\nОставить предсказание? Для подтверждения нажмите <Enter>\n");
                confirm = Console.ReadKey()
                    .Key;

                if (confirm == ConsoleKey.Enter)
                    File.AppendAllText(Directory + @"\exit.txt", exit);
            }
        }

        private static List<T> RandomPermutaion<T>(List<T> datas)
        {
            var random = new Random();
            for (int i = datas.Count - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                var temp = datas[j];
                datas[j] = datas[i];
                datas[i] = temp;
            }

            return datas;
        }
    }
}
