using RandomSim;

var name = Name.Get();

var currentDirectory = CurrentDirectory.Get();

if (!String.IsNullOrEmpty(currentDirectory))
{
    var eventsPath = currentDirectory + @"\events.csv";
    var adjectivesPath = currentDirectory + @"\adjectives.csv";

    if (File.Exists(eventsPath) && File.Exists(adjectivesPath))
        Prediction.Get(currentDirectory, eventsPath, adjectivesPath, name);
    else
        Console.WriteLine($"Error: Программный файл не найден: {eventsPath}.");
}
else
{
    Console.WriteLine($"Error: Объект текущей директории не существует.");
}