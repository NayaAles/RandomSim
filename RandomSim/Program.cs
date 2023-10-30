using RandomSim;

var name = Name.Get();

if (!String.IsNullOrEmpty(Prediction.Directory))
{
    if (File.Exists(Prediction.EventsPath) && File.Exists(Prediction.AdjectivesPath))
        Prediction.Menu(name);
    else
        Console.WriteLine($"Error: Один из программных файлов не найден");  
}
else
{
    Console.WriteLine($"Error: Объект текущей директории не существует");
}