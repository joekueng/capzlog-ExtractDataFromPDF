

using UglyToad.PdfPig;

namespace capzlog_ExtractDataFromPDF;

public class Indexing
{
    private List<(int pageNumber, string firstLine)> _pageIndex;
    private readonly PdfDocument _pdfDoc;

    public  Indexing(PdfDocument pdfDoc)
    {
        _pdfDoc = pdfDoc;
        _pageIndex = GetFirstLines();
    }
    
    private List<(int pageNumber, string firstLine)> GetFirstLines()
    {
        List<(int pageNumber, string firstLine)> index = new List<(int pageNumber, string firstLine)>();
        ExtractText extractText = new ExtractText();
        var firstLines = extractText.ExtractFirstLines(_pdfDoc);
        for (int i = 1; i <= _pdfDoc.NumberOfPages; i++)
        {
            
            
            index.Add((i, firstLines[i]));
        }
        return index;
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