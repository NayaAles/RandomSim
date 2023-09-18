using RandomSim;

var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory())
    .Parent
    .Parent
    .Parent
    .Parent;

if (currentDirectory != null && currentDirectory.Exists)
{
    var eventsPath = currentDirectory.FullName + @"\events.csv";
    GetEvent(eventsPath);
}
else
{
    Console.WriteLine("Нет файла");
}

void GetEvent(string path)
{
    var random = new Random();
    var events = RandomPermutaion(ReaderWriterCsv.ReadFromCsv<Event>(path, ';'))
        .Where(z => !String.IsNullOrEmpty(z.Name))
        .GroupBy(x => x.Name)
        .Select(y => y.FirstOrDefault())
        .ToList();

    var randomNumber = random.Next(0, events.Count - 1);

    var selectedEvent = events[randomNumber];
    selectedEvent.DegreeOfInfluence = Math.Round(random.NextDouble() * 100.0, 2);

    Console.WriteLine($"Событие: {selectedEvent.Name}, степень влияния: {selectedEvent.DegreeOfInfluence}%");
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