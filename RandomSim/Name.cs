
namespace RandomSim
{
    public static class Name
    {
        public static string Get()
        {
            Console.WriteLine("\nВведите имя персонажа:\n");
            Console.WriteLine("Имя: ");
            var name = UpperFirstChar(Console.ReadLine().Split(' ')
                .First()
                .ToString()
                .Trim()
                .ToLower());

            Console.WriteLine("Фамилия: ");
            var surname = UpperFirstChar(Console.ReadLine().Split(' ')
                .First()
                .ToString()
                .Trim()
                .ToLower());

            return $"{name} {surname}";
        }

        private static string UpperFirstChar(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
