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
    
    public CrewBriefing ExtractCrewBriefing()
    {
        CrewBriefing crewBriefing = new CrewBriefing();
        crewBriefing.Crews = ExtractCrew();
        crewBriefing.FlightAssignment = ExtractFlightAssignment();
        crewBriefing.Passengers = ExtractPassengers();
        return crewBriefing;
    }

    private FlightAssigment ExtractFlightAssignment()
    {
        FlightAssigment flightAssignment = new FlightAssigment();

        var dowMatch = Regex.Match(briefingText, @"DOW:\s*(\d+(?:\.\d+)?)kg");
        flightAssignment.DOW = dowMatch.Success ? double.Parse(dowMatch.Groups[1].Value) : 0.0;

        var doiMatch = Regex.Match(briefingText, @"DOI:\s*(\d+(?:\.\d+)?)");
        flightAssignment.DOI = doiMatch.Success ? double.Parse(doiMatch.Groups[1].Value) : 0.0;

        return flightAssignment;
    }

    private Passegers ExtractPassengers()
    {
        Passegers passengers = new Passegers();

        var paxMatch = Regex.Match(briefingText, @"\d+\/(\d+)");
        if (paxMatch.Success)
        {
            passengers.Business = int.Parse(paxMatch.Groups[0].Value.Split("/")[0]);
            passengers.Economy = int.Parse(paxMatch.Groups[1].Value);
        }
        else
        {
            passengers.Business = 0;
            passengers.Economy = 0;
        }

        return passengers;
    }

    // TODO: check for multiple crew members in the same function
    private List<Crew> ExtractCrew()
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

        if (combinedLines.Count > 0)
        {
            combinedLines.RemoveAt(0);
        }

        int index = combinedLines.Count > 0 ? combinedLines[combinedLines.Count - 1].IndexOf("X:") : -1;

        if (index >= 0)
        {
            combinedLines[combinedLines.Count - 1] = combinedLines[combinedLines.Count - 1].Substring(0, index);
        }

        foreach (var line in combinedLines)
        {
            var match = Regex.Match(line, @"(CMD|COP|CAB|SEN)\s+(\w+)\s+([A-Za-zÀ-ÿ\s\-]+)");
            if (match.Success)
            {
                Crew crewMember = new Crew
                {
                    Function = match.Groups[1].Success ? match.Groups[1].Value : "N/A",
                    Lc = match.Groups[2].Success ? match.Groups[2].Value : "N/A",
                    Name = match.Groups[3].Success ? match.Groups[3].Value : "N/A",
                };
                crewList.Add(crewMember);
            }
            else
            {
                crewList.Add(new Crew { Function = "N/A", Lc = "N/A", Name = "N/A" });
            }
        }

        return crewList;
    }
}
