
using System.Text.RegularExpressions;

using UglyToad.PdfPig;


namespace capzlog_ExtractDataFromPDF
{
    public class ParseCzech
    {
        public static readonly String SRC = "C:\\Users\\joeku\\RiderProjects\\capzlog-ExtractDataFromPDF\\Task 1 - Extract Data from a PDF File - Sample File.pdf";
        public static void Main(String[] args)
        {
            using (var pdfDoc = PdfDocument.Open(SRC))
            {
               
                
                SinglePageReader reader = new SinglePageReader();
                string content = reader.GetCrewAndFlightAssignment(pdfDoc, 89);
                BriefingExtractor briefingExtractor = new BriefingExtractor(content);

                Console.WriteLine("Crew Function: " + briefingExtractor.ExtractCrew()[0].Function);
                Console.WriteLine("Business Passengers: " + briefingExtractor.ExtractPassengers().Business);
                Console.WriteLine("Flight Assignment DOI: " + briefingExtractor.ExtractFlightAssignment().DOI);

                string content2 = reader.GetCrewAndFlightAssignment(pdfDoc, 12);
                FlightPlanExtractor flightPlanExtractor = new FlightPlanExtractor(content2);

                Console.WriteLine("Fuel Data Limc: " + flightPlanExtractor.ExtractFlightPlan().FuelData.Limc);
                Console.WriteLine("Zero Fuel Mass: " + flightPlanExtractor.ExtractFlightPlan().MassLoad.ZeroFuelMass);
                Console.WriteLine("Scheduled Arrival Time: " + flightPlanExtractor.ExtractFlightPlan().Schedule.ScheduledArrivalTime);
                Console.WriteLine("GainLoss: " + flightPlanExtractor.ExtractFlightPlan().Correction.GainOrLoss);
                Console.WriteLine("Aircraft Type: " + flightPlanExtractor.ExtractFlightPlan().Info.AircraftType);
                Console.WriteLine("Flight Date: " + flightPlanExtractor.ExtractFlightPlan().Info.Date);
                Console.WriteLine("Departure: " + flightPlanExtractor.ExtractFlightPlan().Info.Departure);


            }
            
        }
        
        
        
    }
}