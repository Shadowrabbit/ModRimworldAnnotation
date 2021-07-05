using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x0200024C RID: 588
	public class PatchOperationReplace : PatchOperationPathed
	{
		// Token: 0x060010EB RID: 4331 RVA: 0x0005FF08 File Offset: 0x0005E108
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				XmlNode parentNode = xmlNode.ParentNode;
				foreach (object obj in node.ChildNodes)
				{
					XmlNode node2 = (XmlNode)obj;
					parentNode.InsertBefore(parentNode.OwnerDocument.ImportNode(node2, true), xmlNode);
				}
				parentNode.RemoveChild(xmlNode);
			}
			return result;
		}

		// Token: 0x04000CDF RID: 3295
		private XmlContainer value;
	}
}
