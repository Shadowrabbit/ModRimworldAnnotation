using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000368 RID: 872
	public class PatchOperationSetName : PatchOperationPathed
	{
		// Token: 0x06001626 RID: 5670 RVA: 0x000D53EC File Offset: 0x000D35EC
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

		// Token: 0x04001100 RID: 4352
		protected string name;
	}
}
