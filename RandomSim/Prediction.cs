
namespace RandomSim
{
    public static  class Prediction
    {
        public static string Get(string eventsPath, string adjectivesPath, string fullName)
        {
            var random = new Random();
            var events = RandomPermutaion(ReaderWriterCsv.ReadFromCsv<Event>(eventsPath, ';'))
                .Where(z => !String.IsNullOrEmpty(z.Name))
                .GroupBy(x => x.Name)
                .Select(y => y.FirstOrDefault())
                .ToList();

            var adjectives = RandomPermutaion(ReaderWriterCsv.ReadFromCsv<Adjective>(adjectivesPath, ';'))
            .Where(z => !String.IsNullOrEmpty(z.RandomAdjective))
            .GroupBy(x => x.RandomAdjective)
            .Select(y => y.FirstOrDefault())
            .ToList();

            var selectedEvent = events[random.Next(0, events.Count - 1)];
            var selectedAdjective = adjectives[random.Next(0, adjectives.Count - 1)];

            selectedEvent.DegreeOfInfluence = Math.Round(random.NextDouble() * 100.0, 2);
            selectedEvent.NumberOfDescendants = random.Next(1, 5);

            var exit = $"\n\nПолучено предсказание для персонажа: {fullName}\n\n" +
                $"Событие: {selectedEvent.Name}, степень влияния: {selectedEvent.DegreeOfInfluence}%;\n" +
                $"Случайное прилагательное: {selectedAdjective.RandomAdjective};\n" +
                $"Количество потомков: {selectedEvent.NumberOfDescendants}";

            return exit;
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
