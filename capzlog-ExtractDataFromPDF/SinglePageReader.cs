using capzlog_ExtractDataFromPDF.models;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis;


namespace capzlog_ExtractDataFromPDF;

//GET flightAssigment and flight crew
public class SinglePageReader
{
    // Method to read a specific page
    // public void ReadPage(string pdfPath, int pageNumber)
    // {
    //     using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(pdfPath)))
    //     {
    //         if (pageNumber < 1 || pageNumber > pdfDoc.GetNumberOfPages())
    //         {
    //             Console.WriteLine($"Page {pageNumber} does not exist in the document.");
    //             return;
    //         }
    //
    //         // Create a text extraction renderer
    //         LocationTextExtractionStrategy strategy = new LocationTextExtractionStrategy();
    //
    //         // Process the specified page content
    //         PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);
    //         parser.ProcessPageContent(pdfDoc.GetPage(pageNumber));
    //
    //         // Extracted text from the specified page
    //         string pageText = strategy.GetResultantText();
    //
    //         // Log the extracted text to the console (for now, we don't return anything)
    //         Console.WriteLine($"Text from Page {pageNumber}:");
    //         Console.WriteLine(pageText);
    //     }
    // }


    public void GetCrewAndFlightAssignment(PdfDocument pdfDocument, int pageNumber)
    {
        ExtractText extractText = new ExtractText();
        var textBlocks = extractText.ExtractTextBlocks(pdfDocument, pageNumber);
        
        


        
            Console.WriteLine(textBlocks);
      
        
        
        
    }
    
    private List<Crew> GetCrewData(IEnumerable<TextBlock> textBlocks)
    {
        List<Crew> crewList = new List<Crew>();
        var data = textBlocks.ElementAt(0).Text.Split('\n').ToList();
        
        var nameraw = textBlocks.ElementAt(3).Text.Split('\n').ToList();

        if (data.Count>0)
        {
            for (int i = 2; i < data.Count; i++)
            {
                var crew = new Crew();
                crew.Function = data[i];
                
                string[] parts = nameraw[i-1].Split(new[] { ' ' }, 2); // Divide al primo spazio

                string part1 = parts[0]; // "VEN"
                string part2 = parts.Length > 1 ? parts[1] : ""; // "Nico Verhelst"

                crew.Name = part2;
                crew.Lc = part1;
               
                crewList.Add(crew);

            }

        }
        
        return crewList;
    }

}