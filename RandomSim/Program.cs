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
    var events = ReaderWriterCsv.ReadFromCsv<Event>(path, ';');

    var randomNumber = random.Next(0, events.Count - 1);

    var selectedEvent = events[randomNumber];
    selectedEvent.DegreeOfInfluence = Math.Round(random.NextDouble() * 100.0, 2)
        .ToString();

    Console.WriteLine($"Событие: {selectedEvent.Name}, степень влияния: {selectedEvent.DegreeOfInfluence}%");
}