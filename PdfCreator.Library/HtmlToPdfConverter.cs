using OpenHtmlToPdf;
using System;

namespace PdfCreator.Library
{
    public class HtmlToPdfConverter
    {
        public byte[] Convert(string html)
        {
            var pdfByteArray = Pdf
                .From(html)
                .Content();
            return pdfByteArray;
        }
    }
}
