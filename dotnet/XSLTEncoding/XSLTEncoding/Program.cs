using System.Xml;
using System.Xml.Xsl;

namespace XSLTEncoding
{
    /// <summary>
    /// Application for testing of XSL transformation when target encoding is defined in the XSLT
    /// </summary>
    class Program
    {
        static void Main()
        {
            using (var xslt = XmlReader.Create(@".\TestData\xsltOSP.xsl"))
            {
                var transform = new XslCompiledTransform();
                transform.Load(xslt);
                transform.Transform(@".\TestData\xml84.xml", @".\TestData\xml84.txt");
                xslt.Close();
            }
        }
    }
}
