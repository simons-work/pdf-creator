using OpenHtmlToPdf;
using PdfCreator.Library.Interfaces;
using System;

namespace PdfCreator.Library
{
    public class HtmlToPdfConverter : IHtmlToPdfConverter
    {
        public byte[] Convert(string html)
        {
            byte[] pdfContent = Pdf
                .From(html)
                .OfSize(PaperSize.A4)
                .WithTitle("Title")
                .WithoutOutline()
                .WithMargins(2.0.Centimeters())
                .Portrait()
                .Comressed()
                .Content();

            return pdfContent;
        }
    }
}
