using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x0200024D RID: 589
	public class PatchOperationSetName : PatchOperationPathed
	{
		// Token: 0x060010ED RID: 4333 RVA: 0x0005FFCC File Offset: 0x0005E1CC
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				XmlNode xmlNode2 = xmlNode.OwnerDocument.CreateElement(this.name);
				xmlNode2.InnerXml = xmlNode.InnerXml;
				xmlNode.ParentNode.InsertBefore(xmlNode2, xmlNode);
				xmlNode.ParentNode.RemoveChild(xmlNode);
			}
			return result;
		}

		// Token: 0x04000CE0 RID: 3296
		protected string name;
	}
}
