using System.Globalization;
using System.Text.RegularExpressions;
using capzlog_ExtractDataFromPDF.models;

namespace capzlog_ExtractDataFromPDF;

public class FlightPlanExtractor
{
    private string OperationText;

    public FlightPlanExtractor(string operationText)
    {
        OperationText = operationText;
    }
    

    public Flight ExtractFlightPlan()
    {
        Flight flight = new Flight();
        flight.Info = ExtractFlightInfo();
        flight.Schedule = ExtractTimes();
        flight.MassLoad = ExtractLoadMass();
        flight.FuelData = ExtractFuel();
        flight.Correction = ExtractCorrections();
        return flight;
    }

    private FlightInfo ExtractFlightInfo()
    {
        var datePattern = @"Date:\s(\d{2}[A-Z]{3}\d{2})";
        var registrationPattern = @"Reg\.\s*:\s*([A-Z0-9]+)";
        var aircraftTypePattern = @"Type:\s([A-Z0-9]+)";
        var departurePattern = @"From:\s([A-Z]{4})";
        var destinationPattern = @"To:\s([A-Z]{4})";
        var alternate1Pattern = @"ALTN1:\s([A-Z]{4})";
        var flightNumberPattern = @"FltNr:\s([A-Z0-9]+)";
        var atcCodePattern = @"ATC:\s([A-Z0-9]+)";

        FlightInfo flightInfo = new FlightInfo();
        
        var dateMatch = Regex.Match(OperationText, datePattern);
        if (dateMatch.Success)
        {
            string originalDate = dateMatch.Groups[1].Value; 
            DateTime parsedDate = DateTime.ParseExact(originalDate, "ddMMMyy", CultureInfo.InvariantCulture);
            flightInfo.Date = parsedDate.ToString("dd.MM.yyyy");
        }
        else
        {
            flightInfo.Date = "N/A";
        }

        flightInfo.Registration = Regex.Match(OperationText, registrationPattern).Groups[1].Success ? 
            Regex.Match(OperationText, registrationPattern).Groups[1].Value : "N/A";
        
        flightInfo.AircraftType = Regex.Match(OperationText, aircraftTypePattern).Groups[1].Success ? 
            Regex.Match(OperationText, aircraftTypePattern).Groups[1].Value : "N/A";
        
        flightInfo.Departure = Regex.Match(OperationText, departurePattern).Groups[1].Success ? 
            Regex.Match(OperationText, departurePattern).Groups[1].Value : "N/A";
        
        flightInfo.Destination = Regex.Match(OperationText, destinationPattern).Groups[1].Success ? 
            Regex.Match(OperationText, destinationPattern).Groups[1].Value : "N/A";
        
        flightInfo.Alternate1 = Regex.Match(OperationText, alternate1Pattern).Groups[1].Success ? 
            Regex.Match(OperationText, alternate1Pattern).Groups[1].Value : "N/A";
        
        flightInfo.FlightNumber = Regex.Match(OperationText, flightNumberPattern).Groups[1].Success ? 
            Regex.Match(OperationText, flightNumberPattern).Groups[1].Value : "N/A";
        
        flightInfo.ATCCode = Regex.Match(OperationText, atcCodePattern).Groups[1].Success ? 
            Regex.Match(OperationText, atcCodePattern).Groups[1].Value : "N/A";
        
        return flightInfo;
    }

    private Times ExtractTimes()
    {
        var timesPattern = @"STD:\s(\d{2}:\d{2})\sSTA:\s(\d{2}:\d{2})";
        var times = new Times();
        var match = Regex.Match(OperationText, timesPattern);

        if (match.Success)
        {
            times.ScheduledDepartureTime = match.Groups[1].Value;
            times.ScheduledArrivalTime = match.Groups[2].Value;
        }
        else
        {
            times.ScheduledDepartureTime = "N/A";
            times.ScheduledArrivalTime = "N/A";
        }

        return times;
    }

    private LoadMass ExtractLoadMass()
    {
        var zeroFuelMassPattern = @"ZFM:\s(\d+)";
        LoadMass loadMass = new LoadMass();
        
        var limcMatch = Regex.Match(OperationText, zeroFuelMassPattern);
        loadMass.ZeroFuelMass = limcMatch.Success ? limcMatch.Groups[1].Value : "N/A";

        return loadMass;
    }

    private Fuel ExtractFuel()
    {
        var limcPattern = @"LIMC:\s([^\s]+ [^\s])";
        var limlPattern = @"LIML:\s([^\s]+ [^\s])";
        var minPattern = @"MIN:\s([^\s]+ [^\s])";
        var fuelData = new Fuel();

        var limcMatch = Regex.Match(OperationText, limcPattern);
        fuelData.Limc = limcMatch.Success ? limcMatch.Groups[1].Value : "N/A";

        var limlMatch = Regex.Match(OperationText, limlPattern);
        fuelData.Liml = limlMatch.Success ? limlMatch.Groups[1].Value : "N/A";

        var minMatch = Regex.Match(OperationText, minPattern);
        fuelData.MinimumRequired = minMatch.Success ? minMatch.Groups[1].Value : "N/A";

        return fuelData;
    }

    private Corrections ExtractCorrections()
    {
        var gainLossPattern = @"Gain\s*/\s*Loss:\s*(GAIN|LOSS)\s*(\d+)\$/TON";
        var corrections = new Corrections();
        var match = Regex.Match(OperationText, gainLossPattern);

        if (match.Success)
        {
            string type = match.Groups[1].Value;
            double amount = double.Parse(match.Groups[2].Value);
            corrections.GainOrLoss = type == "GAIN" ? amount : -amount;
        }
        else
        {
            corrections.GainOrLoss = 0; // Assuming no gain or loss found
        }

        return corrections;
    }
}
