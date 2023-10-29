using RandomSim;

var name = Name.Get();
var currentDirectory = CurrentDirectory.Get(4);
var confirm = ConsoleKey.Z;

if (!String.IsNullOrEmpty(currentDirectory))
{
    var eventsPath = currentDirectory + @"\events.csv";
    var adjectivesPath = currentDirectory + @"\adjectives.csv";

    if (File.Exists(eventsPath) && File.Exists(adjectivesPath))
    {
        while (confirm != ConsoleKey.Enter)
        {
            var exit = Prediction.Get(eventsPath, adjectivesPath, name);
            Console.WriteLine(exit);

            Console.WriteLine("\nОставить предсказание? Для подтверждения нажмите <Enter>\n");
            confirm = Console.ReadKey()
                .Key;

            if (confirm == ConsoleKey.Enter)
                File.AppendAllText(currentDirectory + @"\exit.txt", exit);
        }
    }
    else
    {
        Console.WriteLine($"Error: Программный файл не найден: {eventsPath}.");
    }
    
    
}
else
{
    Console.WriteLine($"Error: Объект текущей директории не существует.");
}