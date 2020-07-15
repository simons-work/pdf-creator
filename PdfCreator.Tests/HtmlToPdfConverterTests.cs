using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfCreator.Library;
using System.IO;

namespace PdfCreator.Tests
{
    [TestClass]
    public class HtmlToPdfConverterTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void Should_Return_Non_Empty_ByteArray_When_Convert_Called()
        {
            var sut = new HtmlToPdfConverter();
            var tempOutputPathname = $"{System.IO.Path.GetTempPath()}\\PdfCreatorTempOutput.pdf";

            var pdfByteArray = sut.Convert("<body style='font-size: 30px'>Test content</body>");
            // File.WriteAllBytes(tempOutputPathname, pdfByteArray);
            // System.Diagnostics.Trace.WriteLine($"Pdf written to temp folder location: {tempOutputPathname}");

            Assert.IsNotNull(pdfByteArray);
            Assert.IsTrue(pdfByteArray.Length > 5000); // Even a PDF with very little content should more than a few KB in length
        }
    }
}
