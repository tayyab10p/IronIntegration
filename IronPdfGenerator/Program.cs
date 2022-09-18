using Models;
using IronPdf;
namespace IronPdfGenerator
{
    public class IronHtmlGenerator
    {
        public IronHtmlGenerator()
        {

        }
        public void GenerateIronPdf(List<string> fileNames, List<SectionsDetails> sectionsDetails,string frontPageHeader)
        {
            string output_path = @"D:\PDFtemplates\Integration\IntergationPDFTest\IronPdfSample\";
            var PDFs = new List<PdfDocument>();
            var renderer = new IronPdf.ChromePdfRenderer();
            bool isConvertedSuccessfull = false;

            foreach (var item in fileNames.Select((file, index) => (file, index)))
            {
                if (item.file.Contains("FrontPage"))
                {

                    HtmlHeaderFooter frontpageHeader = new HtmlHeaderFooter()
                    {
                        MaxHeight = 25,
                        HtmlFragment = frontPageHeader,
                        LoadStylesAndCSSFromMainHtmlDocument = false
                    };
                    renderer.RenderingOptions.MarginTop = 25;
                    renderer.RenderingOptions.MarginBottom = 10;
                    renderer.RenderingOptions.MarginLeft = 5;
                    renderer.RenderingOptions.MarginRight = 5;
                    renderer.RenderingOptions.PaperSize = IronPdf.Rendering.PdfPaperSize.A4;
                    //renderer.RenderingOptions.FitToPaperMode = IronPdf.Engines.Chrome.FitToPaperModes.Automatic;
                    //renderer.RenderHTMLFileAsPdf(item.file);
                    using var pdf = renderer.RenderHTMLFileAsPdf(item.file);
                    var allHeaderPages = Enumerable.Range(0, pdf.PageCount);
                    var skipFirstPageIndexes5 = allHeaderPages.Skip(1);
                    pdf.AddHtmlHeaders(frontpageHeader, 1, skipFirstPageIndexes5);

                    var savedFileName = output_path + $"{Guid.NewGuid()}-FrontPage.pdf";
                    pdf.SaveAs(savedFileName);
                    PDFs.Add(PdfDocument.FromFile(savedFileName));
                }
                else
                {
                    renderer.RenderingOptions.HtmlHeader = new HtmlHeaderFooter()
                    {
                        MaxHeight = 62,
                        HtmlFragment = sectionsDetails[item.index].Header,
                        LoadStylesAndCSSFromMainHtmlDocument = false
                    };
                    renderer.RenderingOptions.PaperOrientation = sectionsDetails[item.index].IsSectionLandscape ? IronPdf.Rendering.PdfPaperOrientation.Landscape : IronPdf.Rendering.PdfPaperOrientation.Portrait;
                    renderer.RenderingOptions.MarginTop = 62;
                    renderer.RenderingOptions.MarginBottom = 10;
                    renderer.RenderingOptions.MarginLeft = 5;
                    renderer.RenderingOptions.MarginRight = 5;
                    renderer.RenderingOptions.PaperSize = IronPdf.Rendering.PdfPaperSize.A4;
                    using var pdf = renderer.RenderHTMLFileAsPdf(item.file);
                    var savedFileName = output_path + $"{Guid.NewGuid()}-sample.pdf";
                    pdf.SaveAs(savedFileName);
                    PDFs.Add(PdfDocument.FromFile(savedFileName));
                }
            }
            PdfDocument doc = PdfDocument.Merge(PDFs);
            //doc.AddTextFooters = "{ page} of {total-pages}";
            //footer for first page
            var footer = new HtmlHeaderFooter();
            footer.MaxHeight = 10;
            footer.HtmlFragment = "<p style='text-align:center;'>Page {page} of {total-pages}</p>";
            var firstPageIndex = new List<int>() { 0 };
            doc.AddHtmlFooters(footer, 1, firstPageIndex);
            var header = new HtmlHeaderFooter();
            //header for all sections
            header.MaxHeight = 45;
            header.DrawDividerLine = false;
            header.HtmlFragment = "<p class='sectionHeader' style='text-align:right;margin-top:80px; margin-right:15mm;font-family:Arial;'>Page {page} of {total-pages}</p>";
            var allPageIndexes = Enumerable.Range(0, doc.PageCount);
            var skipFirstPage = allPageIndexes.Skip(1);
            doc.AddHtmlHeaders(header, 1, skipFirstPage);
            doc.SaveAs(output_path + "IronPdf_editor_withHeader.pdf");
            //doc.SaveAs(output_path + "IronPdf_execution.pdf");

        }
    }
   
}