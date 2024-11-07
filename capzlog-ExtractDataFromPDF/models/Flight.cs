namespace capzlog_ExtractDataFromPDF.models
{
    public class Flight
    {
        public string Identifier { get; private set; } = "";
        public FlightInfo Info { get; set; } = new FlightInfo();
        public Times Schedule { get; set; } = new Times();
        public LoadMass MassLoad { get; set; } = new LoadMass();
        public Fuel FuelData { get; set; } = new Fuel();
        public Corrections Correction { get; set; } = new Corrections();

        public CrewBriefing CrewBriefing { get; set; } = new CrewBriefing();
    }

    public class FlightInfo
    {
        public string Date { get; set; } = "";
        public string Registration { get; set; } = "";
        public string AircraftType { get; set; } = "";
        public string Departure { get; set; } = "";
        public string Destination { get; set; } = "";
        public string Alternate1 { get; set; } = "";
        public string FlightNumber { get; set; } = "";
        public string ATCCode { get; set; } = "";
    }

    public class Times
    {
        public string ScheduledDepartureTime { get; set; } = "";
        public string ScheduledArrivalTime { get; set; } = "";
    }

    public class LoadMass
    {
        public string ZeroFuelMass { get; set; } = "";
    }

    public class Fuel
    {
        public string Limc { get; set; } = "";
        public string Liml { get; set; } = "";
        public string MinimumRequired { get; set; } = "";
    }

    public class Corrections
    {
        public double GainOrLoss { get; set; } = 0.0;
    }
}