using System.Xml;

namespace PdfCreator.Library.Interfaces
{
    public interface IHtmlDocument
    {
        void Initialise();
        XmlElement CreateDocumentNode(string name);
        XmlText CreateContentNode(string value);
        void AddDocumentChildNode(XmlNode newChild, bool isContainerNode);
        void CloseCurrentContainerNode();
        string PopulateHtmlTemplate(string htmlFragment);
        XmlElement CurrentContainer { get; }
        string OuterHtml { get; }
        int CurrentIndentation { get; set; }
    }
}