using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenHtmlToPdf;
using PdfCreator.Library;
using System;
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

            var pdfByteArray = sut.Convert("<body style='font-size: 30px'>Test content</body>");
            // var tempOutputPathname = $"{System.IO.Path.GetTempPath()}\\PdfCreatorTempOutput.pdf";
            // File.WriteAllBytes(tempOutputPathname, pdfByteArray);
            // System.Diagnostics.Trace.WriteLine($"Pdf written to temp folder location: {tempOutputPathname}");

            Assert.IsNotNull(pdfByteArray);
            Assert.IsTrue(pdfByteArray.Length > 5000); // Even a PDF with very little content should more than a few KB in length
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Should_Return_Non_Empty_ByteArray_When_Convert_Called_With_Single_Body_Tag()
        {
            var sut = new HtmlToPdfConverter();

            // I know this is not legal html5 but just wanted to see what the bare minimum this library copes with
            var pdfByteArray = sut.Convert("<body/>");

            Assert.IsNotNull(pdfByteArray);
            Assert.IsTrue(pdfByteArray.Length > 1000);
        }

        [TestMethod]
        [ExpectedException(typeof(PdfDocumentCreationFailedException))]
        [TestCategory("Integration")]
        public void Should_Return_Exception_When_Convert_Called_With_No_Data()
        {
            var sut = new HtmlToPdfConverter();

            var pdfByteArray = sut.Convert("");
        }

        [TestMethod]
        [ExpectedException(typeof(PdfDocumentCreationFailedException))]
        [TestCategory("Integration")]
        public void Should_Return_Exception_When_Convert_Called_With_Null_Data()
        {
            var sut = new HtmlToPdfConverter();

            var pdfByteArray = sut.Convert(null);
        }
    }
}
