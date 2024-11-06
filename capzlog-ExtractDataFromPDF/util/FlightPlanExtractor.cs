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
            // Converti la data nel formato desiderato
            string originalDate = dateMatch.Groups[1].Value; // e.g., "19MAR24"
            DateTime parsedDate = DateTime.ParseExact(originalDate, "ddMMMyy", CultureInfo.InvariantCulture);
            flightInfo.Date = parsedDate.ToString("dd.MM.yyyy"); // e.g., "19.03.2024"
        }

        flightInfo.Registration = Regex.Match(OperationText, registrationPattern).Groups[1].Value;
        flightInfo.AircraftType = Regex.Match(OperationText, aircraftTypePattern).Groups[1].Value;
        flightInfo.Departure = Regex.Match(OperationText, departurePattern).Groups[1].Value;
        flightInfo.Destination = Regex.Match(OperationText, destinationPattern).Groups[1].Value;
        flightInfo.Alternate1 = Regex.Match(OperationText, alternate1Pattern).Groups[1].Value;
        flightInfo.FlightNumber = Regex.Match(OperationText, flightNumberPattern).Groups[1].Value;
        flightInfo.ATCCode = Regex.Match(OperationText, atcCodePattern).Groups[1].Value;
        return flightInfo;
    }

    private Times ExtractTimes()
    {
        // Pattern per catturare i tempi STD e STA
        var timesPattern = @"STD:\s(\d{2}:\d{2})\sSTA:\s(\d{2}:\d{2})";

        var times = new Times();

        // Esegui il match per estrarre i tempi
        var match = Regex.Match(OperationText, timesPattern);
        if (match.Success)
        {
            times.ScheduledDepartureTime = match.Groups[1].Value; // Estrae STD
            times.ScheduledArrivalTime = match.Groups[2].Value; // Estrae STA
        }

        return times;
    }

    private LoadMass ExtractLoadMass()
    {
        var zeroFuelMassPattern = @"ZFM:\s(\d+)";

        LoadMass loadMass = new LoadMass();

        var limcMatch = Regex.Match(OperationText, zeroFuelMassPattern);
        if (limcMatch.Success)
        {
            loadMass.ZeroFuelMass = limcMatch.Groups[1].Value; // Fuel quantity for LIMC
        }

        return loadMass;
    }

    private Fuel ExtractFuel()
    {
        // Regular expressions to capture the values for LIMC, LIML, and MIN
        var limcPattern = @"LIMC:\s([^\s]+ [^\s])";
        var limlPattern = @"LIML:\s([^\s]+ [^\s])";
        var minPattern = @"MIN:\s([^\s]+ [^\s])";

        var fuelData = new Fuel();

        // Match for LIMC
        var limcMatch = Regex.Match(OperationText, limcPattern);
        if (limcMatch.Success)
        {
            fuelData.Limc = limcMatch.Groups[1].Value; // Fuel quantity for LIMC
        }

        // Match for LIML
        var limlMatch = Regex.Match(OperationText, limlPattern);
        if (limlMatch.Success)
        {
            fuelData.Liml = limlMatch.Groups[1].Value; // Fuel quantity for LIML
        }

        // Match for MIN
        var minMatch = Regex.Match(OperationText, minPattern);
        if (minMatch.Success)
        {
            fuelData.MinimumRequired = minMatch.Groups[1].Value; // Fuel quantity for MIN
        }

        return fuelData;
    }

    private Corrections ExtractCorrections()
    {
        var gainLossPattern = @"Gain\s*/\s*Loss:\s*(GAIN|LOSS)\s*(\d+)\$/TON";

        var corrections = new Corrections();

        // Esegui il match per estrarre il tipo di guadagno/perdita e l'importo
        var match = Regex.Match(OperationText, gainLossPattern);
        if (match.Success)
        {
            string type = match.Groups[1].Value; // "GAIN" o "LOSS"
            double amount = double.Parse(match.Groups[2].Value); // Importo numerico

            // Imposta il valore in positivo per GAIN e in negativo per LOSS
            corrections.GainOrLoss = type == "GAIN" ? amount : -amount;
        }

        return corrections;
    }
}