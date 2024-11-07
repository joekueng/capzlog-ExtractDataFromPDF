using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

namespace capzlog_ExtractDataFromPDF;

public class ExtractText
{
    public string GetTextFormSinglePage(PdfDocument document, int pageNumber)
    {
        if (pageNumber < 1 || pageNumber > document.NumberOfPages)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number is out of range.");
        }
        
        var page = document.GetPage(pageNumber);
        
        var words = page.GetWords(NearestNeighbourWordExtractor.Instance);
        
        var blocks = DefaultPageSegmenter.Instance.GetBlocks(words);
        
        return blocks[0].Text;
    }

    public List<string> ExtractFirstLines(PdfDocument document)
    {
        List<string> firstLines = new List<string>();
        for (int i = 1; i <= document.NumberOfPages; i++)
        {
            var page = document.GetPage(i);
            var words = page.GetWords(NearestNeighbourWordExtractor.Instance);
            var blocks = DefaultPageSegmenter.Instance.GetBlocks(words);
            firstLines.Add(blocks[0].Text.Split("\n")[0]);;
        }
        return firstLines;
    }
}