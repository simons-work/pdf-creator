using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfCreator.Library.Configuration;
using PdfCreator.Library.Interfaces;
using System;

namespace PdfCreator.Tests
{
    /// <summary>
    /// Integration tests for CommandManager
    /// </summary>
    /// <remarks>
    /// Note: sut stands for Subject Under Test
    /// 
    /// One thing about most of these tests is that wherever there is content e.g. inside the span or para tags, it always includes a trailing space
    /// This was to cope with the example where in middle of sentence a word is italicised and from looking at the sample input file, it wasn't clear if space characters exist on the ends
    /// of the lines. It would be frustrating for the user to have to remember to leave a trailing space on a word they italicised for example so i choose to do that for them.
    /// However this creates a problem where a bolded word is followed by a comma probably without intention of a space so maybe the system shouldn't automatically add the space
    /// </remarks>

    [TestClass]
    public class CommandManagerTests
    {
        [TestMethod]
        public void Should_Return_Span_Tag_When_Bold_Command_Used()
        {
            var commands = new[] { ".bold", "line", ".regular" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><span style=""font-weight:bold"">line </span></body>", result);
        }

        public void Should_Return_Span_Tag_And_Should_Be_Closed_Correctly_When_Bold_Command_Used_And_No_Regular_Command()
        {
            var commands = new[] { ".bold", "line" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><span style=""font-weight:bold"">line </span></body>", result);
        }

        [TestMethod]
        public void Should_Return_Span_Tag_And_Should_Be_Closed_Correctly_When_Bold_Command_Used_And_No_Text_Content_Or_Regular_Command()
        {
            var commands = new[] { ".bold" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><span style=""font-weight:bold"" /></body>", result);
        }

        [TestMethod]
        public void Should_Return_Empty_Span_Tag_When_Bold_Command_Used_With_No_Text_Content_Before_Regular_Command()
        {
            var commands = new[] { ".bold", ".regular" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><span style=""font-weight:bold"" /></body>", result);
        }

        [TestMethod]
        public void Should_Return_Span_Tag_When_Italic_Command_Used()
        {
            var commands = new[] { ".italics", "line", ".regular" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><span style=""font-style:italic"">line </span></body>", result);
        }

        [TestMethod]
        public void Should_Return_Header_Tag_When_Large_Command_Used()
        {
            var commands = new[] { ".large", "heading", ".normal" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><h1>heading </h1></body>", result);
        }

        [TestMethod]
        public void Should_Return_Para_Tag_When_Para_Command_Used()
        {
            var commands = new[] { ".paragraph", "some text" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p>some text </p></body>", result);
        }


        [TestMethod]
        public void Should_Return_Tag_Tag_And_Should_Be_Closed_Correctly_When_Para_Command_Used_And_No_Text_Content()
        {
            var commands = new[] { ".paragraph" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p /></body>", result);
        }

        [TestMethod]
        public void Should_Return_Para_With_Justify_Style_When_Para_And_Fill_Command_Used()
        {
            var commands = new[] { ".paragraph", ".fill", "some text" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p style=""text-align: justify;"">some text </p></body>", result);
        }

        // The examples show fill command always follows paragraph command so what happens if omit the paragraph, perhaps the code should throw error
        // I've made it more forgiving where it will just apply the justification style to whatever the parent is, in this case the entire document body
        [TestMethod]
        public void Should_Return_Body_With_Justify_Style_When_Para_Command_Forgotten_And_Fill_Command_Used()
        {
            var commands = new[] { ".fill", "some text" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body style=""text-align: justify;"">some text </body>", result);
        }

        [TestMethod]
        public void Should_Return_Para_With_Justify_Style_When_Para_And_Fill_Command_Used_And_No_Text_Content()
        {
            var commands = new[] { ".paragraph", ".fill" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p style=""text-align: justify;"" /></body>", result);
        }

        [TestMethod]
        public void Should_Return_Second_Para_With_No_Justify_Style_When_Para_With_Content_And_NoFill_Command_Used()
        {
            // The thinking behind the nofill command is that it must have to close an existing container first
            // as I think it needs to start a new paragraph because if you were in middle of paragraph which was by virtue of fill command set to be justify, you'd need to start
            // a new line / new para at the very least to be able to see the difference in justification style
            var commands = new[] { ".paragraph", "initial text", ".nofill", "some text" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p>initial text </p><p>some text </p></body>", result);
        }

        // Again the examples only show the nofill command being used after a fill comand as it's pointless operation because default for a pargraph is no justification 
        // so this test only exists to make sure something vaguely sensible is returned
        [TestMethod]
        public void Should_Return_Second_Para_With_No_Justify_Style_When_Para_With_Empty_Content_And_NoFill_Command_Used()
        {
            var commands = new[] { ".paragraph", ".nofill", "some text" };

            var result = _sut.Execute(commands);

            // PDF generator is happy with XHTML rather than HTML so going for <p/> rather than <p></p>
            // but if this bothered you, you could add "element.Add(new XText(""));" into HtmlDocument.OpenContainerNode method which forces non self closing tag style
            Assert.AreEqual(@"<body><p /><p>some text </p></body>", result);
        }

        [TestMethod]
        public void Should_Return_Second_Para_With_Justify_Style_When_Para_With_Content_And_Fill_Command_Used()
        {
            var commands = new[] { ".paragraph", "initial text", ".fill", "some text" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p>initial text </p><p style=""text-align: justify;"">some text </p></body>", result);
        }

        [TestMethod]
        public void Should_Return_Second_Para_With_Indented_Style_When_Para_With_Content_And_Indent_Command_Used()
        {
            var commands = new[] { ".paragraph", "initial text", ".indent +1", "some text" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p>initial text </p><p style=""margin-left:4em;"">some text </p></body>", result);
        }

        [TestMethod]
        public void Should_Return_Third_Para_With_UnIndented_Style_When_Para_With_Content_And_Indent_Then_Unindent_Command_Used()
        {
            var commands = new[] { ".paragraph", "initial text", ".indent +1", "some text", ".indent -1", "more text" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p>initial text </p><p style=""margin-left:4em;"">some text </p><p style=""margin-left:0em;"">more text </p></body>", result);
        }

        [TestMethod]
        public void Should_Return_Para_Containing_Span_Tag_When_Para_And_Italic_Command_Used()
        {
            var commands = new[] { ".paragraph", "some paragraph ...", ".italics", "content", ".regular", "and some further text" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p>some paragraph ... <span style=""font-style:italic"">content </span>and some further text </p></body>", result);
        }

        [TestMethod]
        public void Should_Return_Empty_Document_When_Command_List_Is_Empty()
        {
            var commands = new string[0];

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body></body>", result);
        }

        [TestMethod]
        public void Should_Return_Whitespace_Document_When_Command_List_Has_One_Command_With_EmptyString()
        {
            var commands = new[] { "" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body> </body>", result);
        }

        [TestMethod]
        public void Should_Return_Whitespace_Document_When_Command_List_Has_One_Command_With_Whitespace()
        {
            var commands = new[] { " " };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body>  </body>", result);
        }

        [TestMethod]
        public void Should_Return_Minimal_Document_When_Command_List_Is_Just_Data()
        {
            var commands = new[] { "test" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body>test </body>", result);
        }

        [TestMethod]
        public void Should_Return_Minimal_Document_When_Command_List_Has_Invalid_Command_And_Treated_As_Data()
        {
            var commands = new[] { ".nonesense" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body>.nonesense </body>", result);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_Return_Exception_When_Command_List_Is_Null()
        {
            string[] commands = null;

            _sut.Execute(commands);
        }

        [TestMethod]
        public void Should_Return_Correctly_Encoded_Characters()
        {
            var commands = new[] { "Unicode quote chars “Lorem ipsum” & an ampersand character" };

            var result = _sut.Execute(commands);

            // Note I have put this string into ...\PdfCreator.Console\OtherInputs\Random.txt to check that the html encoded entities for example &amp; to eventually get converted back to original & in the pdf output
            Assert.AreEqual(@"<body>Unicode quote chars “Lorem ipsum” &amp; an ampersand character </body>", result);
        }


        [TestInitialize]
        public void TestInitialize()
        {
            _sut = AppInitialisation.GetRequiredService<ICommandManager>();
        }

        private ICommandManager _sut;
    }
}
