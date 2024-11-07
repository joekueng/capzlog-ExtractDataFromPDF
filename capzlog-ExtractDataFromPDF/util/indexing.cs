using System.Text.RegularExpressions;
using UglyToad.PdfPig;

namespace capzlog_ExtractDataFromPDF;

public class Indexing
{
    private List<(int pageNumber, string firstLine)> _pageIndex;
    private readonly PdfDocument _pdfDoc;

    public Indexing(PdfDocument pdfDoc)
    {
        _pdfDoc = pdfDoc;
        _pageIndex = GetFirstLines();
    }

    private List<(int pageNumber, string firstLine)> GetFirstLines()
    {
        List<(int pageNumber, string firstLine)> index = new List<(int pageNumber, string firstLine)>();
        ExtractText extractText = new ExtractText();
        var firstLines = extractText.ExtractFirstLines(_pdfDoc);
        for (int i = 0; i < _pdfDoc.NumberOfPages; i++)
        {
            index.Add((i, firstLines[i]));
        }

        return index;
    }


    public List<int> GetIndexFlightPlans()
    {
        List<int> flightPlans = new List<int>();

        string pattern = @"FMS\sIDENT=\S+\s+Log\sNr\.\:\s+\S+";

        foreach (var (pageNumber, line) in _pageIndex)
        {
            if (Regex.IsMatch(line, @"FMS\sIDENT=\S+\s+Log\sNr\.\:\s+\S+\s+Page\s1"))
            {
                flightPlans.Add(pageNumber+1);
            }
        }

        return flightPlans;
    }


    //TODO: check for FlrNr, first line, and page 1 of
    public int getIndexCrewBriefing(string flightNumber)
    {
        foreach (var (pageNumber, line) in _pageIndex)
        {
            if (line.Equals("Flight Assignment / Flight Crew Briefing"))
            {
                ExtractText extractText = new ExtractText();
                string content = extractText.GetTextFormSinglePage(_pdfDoc, pageNumber);
                if (content.Contains(flightNumber) && Regex.IsMatch(content, @"Page\s1\sof"))
                {
                    return pageNumber;
                }
            }
        }
        return -1;
    }

    public int GetPageNumber(string firstLine)
    {
        foreach (var (pageNumber, line) in _pageIndex)
        {
            if (line == firstLine)
            {
                return pageNumber;
            }
        }

        return -1;
    }
}