
using System.Text.RegularExpressions;
using capzlog_ExtractDataFromPDF.models;
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
                
                Indexing indexing = new Indexing(pdfDoc);
                List<int> indexFlightPlans = indexing.GetIndexFlightPlans();
               
                Console.WriteLine("Found "+indexFlightPlans.Count+" flight plans");
                
                List<Flight> flights = new List<Flight>();

                Elaborate elaborate = new Elaborate(pdfDoc);
                
                for (int i = 0; i < indexFlightPlans.Count; i++)
                {
                    flights.Add(elaborate.GetAllFlightInfo(indexFlightPlans[i]));
                    
                }
                
                flights.ForEach(flight =>
                {
                    Console.WriteLine("Flight Number: " + flight.Info.FlightNumber);
                    Console.WriteLine("Aircraft Type: " + flight.Info.AircraftType);
                    Console.WriteLine("Departure: " + flight.Info.Departure);
                    Console.WriteLine("Arrival: " + flight.Info.Destination);
                    Console.WriteLine("Date: " + flight.Info.Date);
                    Console.WriteLine("Zero Fuel Mass: " + flight.MassLoad.ZeroFuelMass);
                    Console.WriteLine("Scheduled Departure Time: " + flight.Schedule.ScheduledDepartureTime);
                    flight.CrewBriefing.Crews.ForEach(crew =>
                    {
                        Console.WriteLine(crew.Function + ": " + crew.Name);
                    });
                    
                    Console.WriteLine();

                });

            

            }
            
        }
        
        
        
    }
}