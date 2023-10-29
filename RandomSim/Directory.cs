
namespace RandomSim
{
    public static class CurrentDirectory
    {
        public static string Get(int numberOfDepth)
        {
            var startDirectory = new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent;

            var i = 0;
            string currentDirectory = "";
            do
            {
                if (startDirectory != null)
                {
                    i++;
                    currentDirectory = startDirectory.FullName;
                    startDirectory = startDirectory.Parent;
                }
            }
            while (i < numberOfDepth);

            return currentDirectory;
        }
    }
}
