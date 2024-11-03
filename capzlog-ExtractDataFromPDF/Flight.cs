namespace DefaultNamespace
{
    public class Flight
    {
        private string ident = "";
        private FlightInfo flightInfo;
    }

    public class FlightInfo
    {
        public string Date { get; set; } = "";
        public string Reg { get; set; } = "";
        public string Type { get; set; } = "";
        public string From { get; set; } = "";
        public string To { get; set; } = "";
        public string Altn1 { get; set; } = "";
        public string FltNr { get; set; } = "";
        public string ATC { get; set; } = "";
    }

    public class Times
    {
        public string STD { get; set; } = "";
        public string STA { get; set; } = "";
    }

    public class Loadmass
    {
        public string ZFM { get; set; } = "";
    }

    public class Fuel
    {
        public string LIMC { get; set; } = "";
        public string LIML { get; set; } = "";
        public string MIN { get; set; } = "";

    }

    public class Corrections
    {
        public double gain-loss { get; set; } = 0;
    }


}
