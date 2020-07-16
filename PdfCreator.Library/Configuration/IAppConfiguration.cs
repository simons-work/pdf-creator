namespace PdfCreator.Library.Configuration
{
    public interface IAppConfiguration
    {
        string InputFilename { get; set; }
        string OutputFilename { get; set; }
        int IndentMultiplierForMarginSize { get; set; }
    }
}