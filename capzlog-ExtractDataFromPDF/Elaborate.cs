using capzlog_ExtractDataFromPDF.models;
using UglyToad.PdfPig;

namespace capzlog_ExtractDataFromPDF;

public class Elaborate
{
    private PdfDocument _pdfDocument;
    public Elaborate(PdfDocument pdfDocument)
    {
        _pdfDocument = pdfDocument;
        
    }
    
    public Flight GetAllFlightInfo(int pageOfFlightPlan)
    {
        ExtractText reader = new ExtractText();
        string content = reader.GetTextFormSinglePage(_pdfDocument, pageOfFlightPlan);

        FlightPlanExtractor flightPlanExtractor = new FlightPlanExtractor(content);
        Flight flight = flightPlanExtractor.ExtractFlightPlan();


        Indexing indexing = new Indexing(_pdfDocument);

        int indexCrewBriefing = indexing.getIndexCrewBriefing(flight.Info.FlightNumber);
        if (indexCrewBriefing < 0)
        {
            return flight;
        }

        string crewContent = reader.GetTextFormSinglePage(_pdfDocument, indexCrewBriefing);

        BriefingExtractor briefingExtractor = new BriefingExtractor(crewContent);
        CrewBriefing crewBriefing = briefingExtractor.ExtractCrewBriefing();

        flight.CrewBriefing = crewBriefing;

        return flight;
    }
}