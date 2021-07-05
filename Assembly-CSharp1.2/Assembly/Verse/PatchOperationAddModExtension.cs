using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000363 RID: 867
	public class PatchOperationAddModExtension : PatchOperationPathed
	{
		// Token: 0x0600161E RID: 5662 RVA: 0x000D50A4 File Offset: 0x000D32A4
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				XmlNode xmlNode2 = xmlNode["modExtensions"];
				if (xmlNode2 == null)
				{
					xmlNode2 = xmlNode.OwnerDocument.CreateElement("modExtensions");
					xmlNode.AppendChild(xmlNode2);
				}
				foreach (object obj2 in node.ChildNodes)
				{
					XmlNode node2 = (XmlNode)obj2;
					xmlNode2.AppendChild(xmlNode.OwnerDocument.ImportNode(node2, true));
				}
				result = true;
			}
			return result;
		}

		// Token: 0x040010F9 RID: 4345
		private XmlContainer value;
	}
}
