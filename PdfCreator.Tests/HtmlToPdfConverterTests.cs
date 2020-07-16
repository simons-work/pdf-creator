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
        /// <summary>
        /// Integration tests for HtmlToPdfConverter
        /// </summary>
        /// <remarks>Note: sut stands for Subject Under Test</remarks>
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
        public void Should_Return_Non_Empty_ByteArray_When_Convert_Called_With_Empty_Body_Tag()
        {
            var sut = new HtmlToPdfConverter();

            var pdfByteArray = sut.Convert("<body></body>");

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
