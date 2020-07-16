namespace PdfCreator.Library.Interfaces
{
    public interface IHtmlToPdfConverter
    {
        byte[] Convert(string html);
    }
}