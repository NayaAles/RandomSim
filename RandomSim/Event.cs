
namespace RandomSim
{
    public class Event
    {
        public string Name { get; set; } = null!;
        public double DegreeOfInfluence { get; set; }
        public int NumberOfDescendants { get; set; }
    }

    public class Adjective
    {
        public string RandomAdjective { get; set; } = null!;
    }
}
