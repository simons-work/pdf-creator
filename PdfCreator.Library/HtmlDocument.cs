using PdfCreator.Library.Interfaces;
using System.Xml;

namespace PdfCreator.Library
{
    public class HtmlDocument : IHtmlDocument
    {
        public void Initialise()
        {
            _document = new XmlDocument();
            _document.LoadXml("<body></body>");
            _currentContainerNode = _document.DocumentElement;
            CurrentIndentation = 0;
        }

        public XmlElement CreateDocumentNode(string name)
        {
            return _document.CreateElement(name);
        }

        public XmlText CreateContentNode(string value)
        {
            return _document.CreateTextNode(value);
        }

        public void AddDocumentChildNode(XmlNode newChild, bool isContainerNode)
        {
            _currentContainerNode.AppendChild(newChild);
            _currentContainerNode = isContainerNode ? _currentContainerNode.LastChild as XmlElement : _currentContainerNode;
        }

        public void CloseCurrentContainerNode()
        {
            // Updates the internal pointer to the current container node to be the parent, so in effect we're closing current node and all future additions will be at parent level
            _currentContainerNode = _currentContainerNode.ParentNode as XmlElement;
        }

        public string PopulateHtmlTemplate(string htmlFragment)
        {
            string htmlTemplate = _htmlTemplate;
            return htmlTemplate.Replace("<body></body>", htmlFragment);
        }

        public XmlElement CurrentContainer { get => _currentContainerNode; }

        public string OuterHtml { get => _document.OuterXml; }

        public int CurrentIndentation { get; set; }

        #region Private methods / members

        private XmlDocument _document;
        private XmlElement _currentContainerNode;
        private readonly IHtmlDocument _htmlDocument;
        private const string _htmlTemplate = "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset='utf-8' /><title></title><style>body { font-size:30px; font-family:'Segoe UI',Tahoma,Geneva,Verdana,sans-serif; }</style></head><body></body></html>";

        #endregion

    }
}
