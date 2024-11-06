using capzlog_ExtractDataFromPDF.models;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis;


namespace capzlog_ExtractDataFromPDF;

//GET flightAssigment and flight crew
public class SinglePageReader
{
  


    public string GetCrewAndFlightAssignment(PdfDocument pdfDocument, int pageNumber)
    {
        ExtractText extractText = new ExtractText();
        var textBlocks = extractText.ExtractTextBlocks(pdfDocument, pageNumber);
        
        return textBlocks;

    }
    

}