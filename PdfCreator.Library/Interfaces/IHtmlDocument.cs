using System.Xml;
using System.Xml.Linq;

namespace PdfCreator.Library.Interfaces
{
    public interface IHtmlDocument
    {
        void Initialise();
        void AddContentNode(string content);
        void OpenContainerNode(string nodeName, string attributeName = null, string attributeValue = null);
        XElement CreateDocumentNode(string name);
        XText CreateContentNode(string value);
        void AddDocumentChildNode(XNode newChild, bool isContainerNode);
        void CloseCurrentContainerNode();
        string PopulateHtmlTemplate(string htmlFragment);
        XElement CurrentContainer { get; }
        string OuterHtml { get; }
        int CurrentIndentation { get; set; }
    }
}