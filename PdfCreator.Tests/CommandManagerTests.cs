using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfCreator.Library;
using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Configuration;
using PdfCreator.Library.Extensions;
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
            var commands = new string[] { ".bold", "line", ".regular" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><span style=""font-weight:bold"">line </span></body>", result);
        }

        [TestMethod]
        public void Should_Return_Span_Tag_When_Italic_Command_Used()
        {
            var commands = new string[] { ".italics", "line", ".regular" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><span style=""font-style:italic"">line </span></body>", result);
        }

        [TestMethod]
        public void Should_Return_Para_Tag_When_Para_Command_Used()
        {
            var commands = new string[] { ".paragraph", "some text" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p>some text </p></body>", result);
        }

        // public void Should_Return_Para_With_Margin_When_Para_And_Ident_Command_Used()

        [TestMethod]
        public void Should_Return_Para_With_Justify_Style_When_Para_And_Fill_Command_Used()
        {
            var commands = new string[] { ".paragraph", ".fill", "some text" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p style=""text-align: justify;"">some text </p></body>", result);
        }

        [TestMethod]
        public void Should_Return_Second_Para_With_No_Justify_Style_When_Para_With_Content_And_NoFill_Command_Used()
        {
            // The thinking behind the nofill command is that it must have to close an existing container first
            // as I think it needs to start a new paragraph because if you were in middle of paragraph which was by virtue of fill command set to be justify, you'd need to start
            // a new line / new para at the very least to be able to see the difference in justification style
            var commands = new string[] { ".paragraph", "initial text", ".nofill", "some text" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p>initial text </p><p>some text </p></body>", result);
        }

        [TestMethod]
        public void Should_Return_Second_Para_With_No_Justify_Style_When_Para_With_Empty_Content_And_NoFill_Command_Used()
        {
            var commands = new string[] { ".paragraph", ".nofill", "some text" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p></p><p>some text </p></body>", result);
        }

        [TestMethod]
        public void Should_Return_Second_Para_With_Justify_Style_When_Para_With_Content_And_Fill_Command_Used()
        {
            var commands = new string[] { ".paragraph", "initial text", ".fill", "some text" };

            var result = _sut.Execute(commands);

            Assert.AreEqual(@"<body><p>initial text </p><p style=""text-align: justify;"">some text </p></body>", result);
        }


        [TestMethod]
        public void Should_Return_Para_Containing_Span_Tag_When_Para_And_Italic_Command_Used()
        {
            var commands = new string[] { ".paragraph", "some paragraph ...", ".italics", "content", ".regular", "and some further text" };

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
            var commands = new string[] { "" };

            var result = _sut.Execute(commands);

            // Not show that <body> is allowd to be self closing tag but have tested that the HtmlToPdf library is happy with it and this is probably due to using XmlDocument to generate the Html
            Assert.AreEqual(@"<body> </body>", result);
        }

        [TestMethod]
        public void Should_Return_Whitespace_Document_When_Command_List_Has_One_Command_With_Whitespace()
        {
            var commands = new string[] { " " };

            var result = _sut.Execute(commands);

            // Not show that <body> is allowd to be self closing tag but have tested that the HtmlToPdf library is happy with it and this is probably due to using XmlDocument to generate the Html
            Assert.AreEqual(@"<body>  </body>", result);
        }

        [TestMethod]
        public void Should_Return_Minimal_Document_When_Command_List_Is_Just_Data()
        {
            var commands = new string[] { "test" };

            var result = _sut.Execute(commands);

            // Not show that <body> is allowd to be self closing tag but have tested that the HtmlToPdf library is happy with it and this is probably due to using XmlDocument to generate the Html
            Assert.AreEqual(@"<body>test </body>", result);
        }

        [TestMethod]
        public void Should_Return_Minimal_Document_When_Command_List_Has_Invalid_Command_And_Treated_As_Data()
        {
            var commands = new string[] { ".nonesense" };

            var result = _sut.Execute(commands);

            // Not show that <body> is allowd to be self closing tag but have tested that the HtmlToPdf library is happy with it and this is probably due to using XmlDocument to generate the Html
            Assert.AreEqual(@"<body>.nonesense </body>", result);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_Return_Exception_When_Command_List_Is_Null()
        {
            string[] commands = null;

            _sut.Execute(commands);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var appConfiguration = serviceProvider.GetRequiredService<IAppConfiguration>();

            LoadConfiguration(new string[0], appConfiguration);
            
            _sut = serviceProvider.GetRequiredService<ICommandManager>();
        }

        static private void LoadConfiguration(string[] args, IAppConfiguration appConfiguration)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true)
                .Build();

            configuration.Bind("config", appConfiguration);
        }


        static private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAppConfiguration, AppConfiguration>();
            serviceCollection.AddSingleton<IPdfCreator, PdfCreator.Library.PdfCreator>();
            serviceCollection.AddSingleton<ICommandManager, CommandManager>();
            serviceCollection.AddSingleton<IHtmlDocument, HtmlDocument>();
            serviceCollection.AddSingleton<IHtmlToPdfConverter, HtmlToPdfConverter>();
            serviceCollection.RegisterAllTypes<ICommand>(new[] { typeof(CommandManager).Assembly });
        }

        private ICommandManager _sut;
    }
}
