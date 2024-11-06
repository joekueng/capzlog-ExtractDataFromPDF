
using System.Text.RegularExpressions;

using UglyToad.PdfPig;


namespace capzlog_ExtractDataFromPDF
{
    public class ParseCzech
    {
        public static readonly String DEST = "C:\\Users\\joeku\\RiderProjects\\capzlog-ExtractDataFromPDF\\text.txt";
        public static readonly String SRC = "C:\\Users\\joeku\\RiderProjects\\capzlog-ExtractDataFromPDF\\Task 1 - Extract Data from a PDF File - Sample File.pdf";
        public static void Main(String[] args)
        {
            using (var pdfDoc = PdfDocument.Open(SRC))
            {
                //Prova prova = new Prova();
                //prova.ProvaRead(SRC);
                
                
               
               // var testoEstratto = EstraiTestoDaPosizione(SRC, pagina: 85, xMin: 400, yMin: 400, xMax: 800, yMax: 800);

                //Console.WriteLine("Testo Estratto:");
               // Console.WriteLine(testoEstratto);
                
                //EstraiCrewConRegex(SRC, 85);
                
                SinglePageReader reader = new SinglePageReader();
                reader.GetCrewAndFlightAssignment(pdfDoc, 89);
            }
            
            static string EstraiTestoDaPosizione(string filePath, int pagina, double xMin, double yMin, double xMax, double yMax)
            {
                using (PdfDocument pdf = PdfDocument.Open(filePath))
                {
                    var paginaPdf = pdf.GetPage(pagina);
                    List<string> testoNellaZona = new List<string>();

                    foreach (var parola in paginaPdf.GetWords())
                    {
                        var posizione = parola.BoundingBox;

                        if (posizione.Left >= xMin && posizione.Right <= xMax &&
                            posizione.Bottom >= yMin && posizione.Top <= yMax)
                        {
                            testoNellaZona.Add(parola.Text);
                        }
                    }

                    return string.Join(" ", testoNellaZona);
                }
            }

            static void EstraiCrewConRegex(string filePath, int pagina)
            {
                List<string> crew = new List<string>();
                using (PdfDocument pdf = PdfDocument.Open(filePath))
                {
                    var paginaPdf = pdf.GetPage(pagina);
                    string testo = paginaPdf.Text;

                    // Regex per catturare ruolo, codice, nome e posizione in maniera compatta
                    var regex = new Regex(@"\b(CMD|COP|CAB|SEN)\s(\w+)\s([A-Za-z]+\s[A-Za-z]+)\s(.+?)(?=\bCMD|\bCOP|\bCAB|\bSEN|Observer|Contacts|Crew|\Z)");

                    foreach (Match match in regex.Matches(testo))
                    {
                        string ruolo = match.Groups[1].Value;
                        string codice = match.Groups[2].Value;
                        string nome = match.Groups[3].Value;
                        string posizione = match.Groups[4].Value;

                        crew.Add($"{ruolo} {codice} {nome} - {posizione}");
                    }
                }
            }


                        

            //
            
            // reader.ReadPage(SRC, 85); // Read page 2 for example
            //
            //FileInfo file = new FileInfo(DEST);
            //file.Directory.Create();

            //new ParseCzech().ManipulatePdf(DEST);
        }
        
        
        
    }
}