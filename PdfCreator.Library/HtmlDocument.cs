﻿using PdfCreator.Library.Interfaces;
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

        public void AddNewContentNode(string content)
        {
            XNode element = CreateContentNode(content);
            AddDocumentChildNode(element, false);
        }

        public void OpenNewContainerNode(string nodeName, string attributeName = null, string attributeValue = null)
        {
            XElement element = CreateDocumentNode(nodeName);

            if (attributeName != null) element.SetAttributeValue(attributeName, attributeValue);

            AddDocumentChildNode(element, true);
        }

        public void UpdateCurrentContainerNodeAttributes(string attributeName, string attributeValue)
        {
            string existingAttribute = _currentContainerNode.Attribute(attributeName)?.Value;
            _currentContainerNode.SetAttributeValue(attributeName, string.Join(";", attributeValue, existingAttribute));
        }

        public void CloseCurrentContainerNode()
        {
            // Updates the internal pointer to the current container node to be the parent, so in effect we're closing current node and all future additions will be at parent level
            _currentContainerNode = _currentContainerNode.Parent as XElement;
        }

        public bool IsCurrentContainerNodeEmpty 
        { 
            get => string.IsNullOrEmpty(CurrentContainer?.Value); 
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
