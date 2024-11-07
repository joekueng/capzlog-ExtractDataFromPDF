namespace capzlog_ExtractDataFromPDF.models
{
    public class CrewBriefing
    {
        public List<Crew> Crews { get; set; }= new List<Crew>();
        public FlightAssigment FlightAssignment { get; set; } = new FlightAssigment();
        public Passegers Passengers { get; set; } = new Passegers();
    }
    
    public class Crew
    {
        public string Function { get; set; } = "";
        public string Lc { get; set; } = "";
        public string Name { get; set; } = "";
    
    }
    
    public class FlightAssigment
    {
        public double DOW { get; set; } = 0;
        public double DOI { get; set; } = 0;
    }
    public class Passegers
    {
        public int Business { get; set; } = 0;
        public int Economy { get; set; } = 0;
    }
}

