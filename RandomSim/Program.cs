using RandomSim;

Console.WriteLine("\nВведите имя персонажа:\n");
Console.WriteLine("Имя: ");
var name = UpperFirstChar(Console.ReadLine()
    .Split(' ')
    .First()
    .ToString()
    .Trim()
    .ToLower());

Console.WriteLine("Фамилия: ");
var surname = UpperFirstChar(Console.ReadLine()
    .Split(' ')
    .First()
    .ToString()
    .Trim()
    .ToLower());

var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory())
    .Parent
    .Parent
    .Parent
    .Parent;

if (currentDirectory != null)
{
    var eventsPath = currentDirectory.FullName + @"\events.csv";
    var adjectivesPath = currentDirectory.FullName + @"\adjectives.csv";

    if (File.Exists(eventsPath) && File.Exists(adjectivesPath))
        GetPrediction(eventsPath, adjectivesPath, $"{name} {surname}");
    else
        Console.WriteLine($"Error: Программный файл не найден: {eventsPath}.");
}
else
{
    Console.WriteLine($"Error: Объект текущей директории не существует.");
}

void GetPrediction(string eventsPath, string adjectivesPath, string fullName)
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
    selectedEvent.NumberOfDescendants = random.Next(1,3);

    string exit = $"\nПолучено предсказание для персонажа: {fullName}\n\n" +
        $"Событие: {selectedEvent.Name}, степень влияния: {selectedEvent.DegreeOfInfluence}%;\n" +
        $"Случайное прилагательное: {selectedAdjective.RandomAdjective};\n" +
        $"Количество потомков: {selectedEvent.NumberOfDescendants}";

    Console.WriteLine(exit);
    File.AppendAllText(currentDirectory + @"/exit.txt", exit);
}

List<T> RandomPermutaion<T>(List<T> datas)
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

string UpperFirstChar(string input)
{
    if (string.IsNullOrEmpty(input))
    {
        return null;
    }

    return char.ToUpper(input[0]) + input.Substring(1);
}