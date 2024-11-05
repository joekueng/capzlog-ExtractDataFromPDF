using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis.ReadingOrderDetector;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

namespace capzlog_ExtractDataFromPDF;

public class ExtractText
{
    public IEnumerable<TextBlock> ExtractTextBlocks(PdfDocument document, int pageNumber)
    {
        if (pageNumber < 1 || pageNumber > document.NumberOfPages)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number is out of range.");
        }
        
        var page = document.GetPage(pageNumber);

        var letters = page.Letters;
        var wordExtractor = NearestNeighbourWordExtractor.Instance;

        var words = wordExtractor.GetWords(letters);
                
        var pageSegmenter = DocstrumBoundingBoxes.Instance;

        var textBlocks = pageSegmenter.GetBlocks(words);
                
        var readingOrder = UnsupervisedReadingOrderDetector.Instance;
        return readingOrder.Get(textBlocks);
    }
}