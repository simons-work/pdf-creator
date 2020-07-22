using System.Xml;
using System.Xml.Linq;

namespace PdfCreator.Library.Interfaces
{
    public interface IHtmlDocument
    {
        void Initialise();
        void AddNewContentNode(string content);
        void OpenNewContainerNode(string nodeName, string attributeName = null, string attributeValue = null);
        void UpdateCurrentContainerNodeAttributes(string attributeName, string attributeValue);
        XElement CreateDocumentNode(string name);
        XText CreateContentNode(string value);
        void AddDocumentChildNode(XNode newChild, bool isContainerNode);
        void CloseCurrentContainerNode();
        bool IsCurrentContainerNodeEmpty { get; }
        string PopulateHtmlTemplate(string htmlFragment);
        XElement CurrentContainer { get; }
        string OuterHtml { get; }
        int CurrentIndentation { get; set; }
    }
}