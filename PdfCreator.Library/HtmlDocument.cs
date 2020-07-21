using PdfCreator.Library.Interfaces;
using System.Xml.Linq;

namespace PdfCreator.Library
{
    public class HtmlDocument : IHtmlDocument
    {
        public void Initialise()
        {
            _document = XDocument.Parse("<body></body>");
            _currentContainerNode = _document.Root;
            CurrentIndentation = 0;
        }

        public void OpenContainerNode(string nodeName, string attributeName, string attributeValue)
        {
            XElement element = CreateDocumentNode(nodeName);

            if (attributeName != null) element.SetAttributeValue(attributeName, attributeValue);

            AddDocumentChildNode(element, true);
        }

        public XElement CreateDocumentNode(string name)
        {
            return new XElement(name);
        }

        public XText CreateContentNode(string value)
        {
            return new XText(value);
        }

        public void AddDocumentChildNode(XNode newChild, bool isContainerNode)
        {
            _currentContainerNode.Add(newChild);
            _currentContainerNode = isContainerNode ? _currentContainerNode.LastNode as XElement : _currentContainerNode;
        }

        public void CloseCurrentContainerNode()
        {
            // Updates the internal pointer to the current container node to be the parent, so in effect we're closing current node and all future additions will be at parent level
            _currentContainerNode = _currentContainerNode.Parent as XElement;
        }

        public string PopulateHtmlTemplate(string htmlFragment)
        {
            string htmlTemplate = _htmlTemplate;
            return htmlTemplate.Replace("<body></body>", htmlFragment);
        }

        public XElement CurrentContainer { get => _currentContainerNode; }

        public string OuterHtml { get => _document.ToString(SaveOptions.DisableFormatting); }

        public int CurrentIndentation { get; set; }

        #region Private methods / members

        private XDocument _document;
        private XElement _currentContainerNode;
        private const string _htmlTemplate = "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset='utf-8' /><title></title><style>body { font-size:30px; font-family:'Segoe UI',Tahoma,Geneva,Verdana,sans-serif; }</style></head><body></body></html>";

        #endregion

    }
}
