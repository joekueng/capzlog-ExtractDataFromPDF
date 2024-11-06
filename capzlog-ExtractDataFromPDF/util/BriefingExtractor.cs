using System.Text.RegularExpressions;
using capzlog_ExtractDataFromPDF.models;

namespace capzlog_ExtractDataFromPDF;

public class BriefingExtractor
{
    private string briefingText;

    public BriefingExtractor(string briefingText)
    {
        this.briefingText = briefingText;
    }

    public FlightAssigment ExtractFlightAssignment()
    {
        FlightAssigment flightAssignment = new FlightAssigment();

        var dowMatch = Regex.Match(briefingText, @"DOW:\s*(\d+(?:\.\d+)?)kg");
        if (dowMatch.Success)
        {
            flightAssignment.DOW = double.Parse(dowMatch.Groups[1].Value);
        }

        var doiMatch = Regex.Match(briefingText, @"DOI:\s*(\d+(?:\.\d+)?)");
        if (doiMatch.Success)
        {
            flightAssignment.DOI = double.Parse(doiMatch.Groups[1].Value);
        }

        return flightAssignment;
    }

    public Passegers ExtractPassengers()
    {
        Passegers passengers = new Passegers();

        var paxMatch = Regex.Match(briefingText, @"\d+\/(\d+)");
        if (paxMatch.Success)
        {
            passengers.Business = int.Parse(paxMatch.Groups[0].Value.Split("/")[0]);;
            passengers.Economy = int.Parse(paxMatch.Groups[1].Value);
        }

        return passengers;
    }

    //TODO: check for multiple crew members in the same function
    public List<Crew> ExtractCrew()
    {
        List<Crew> crewList = new List<Crew>();

        var lines = briefingText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        List<string> combinedLines = new List<string>();

        string currentLine = "";
        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            if (Regex.IsMatch(trimmedLine, @"^(CMD|COP|CAB|SEN)\s[A-Z]{3}"))
            {
                if (!string.IsNullOrEmpty(currentLine))
                {
                    combinedLines.Add(currentLine.Trim());
                }

                currentLine = trimmedLine;
            }
            else
            {
                currentLine += " " + trimmedLine;
            }
        }


        if (!string.IsNullOrEmpty(currentLine))
        {
            combinedLines.Add(currentLine.Trim());
        }

        combinedLines.RemoveAt(0);

        int index = combinedLines[combinedLines.Count - 1].IndexOf("X:");

        if (index >= 0)
        {
            combinedLines[combinedLines.Count - 1] = combinedLines[combinedLines.Count - 1].Substring(0, index);
        }

        foreach (var line in combinedLines)
        {
            var match = Regex.Match(line,
                @"(CMD|COP|CAB|SEN)\s+(\w+)\s+([A-Za-zÀ-ÿ\s\-]+)");
            if (match.Success)
            {
                Crew crewMember = new Crew
                {
                    Function = match.Groups[1].Value,
                    Lc = match.Groups[2].Value,
                    Name = match.Groups[3].Value,
                };
                crewList.Add(crewMember);
            }
        }

        return crewList;
    }
}