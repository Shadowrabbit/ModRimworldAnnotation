using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000367 RID: 871
	public class PatchOperationReplace : PatchOperationPathed
	{
		// Token: 0x06001624 RID: 5668 RVA: 0x000D5328 File Offset: 0x000D3528
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

		// Token: 0x040010FF RID: 4351
		private XmlContainer value;
	}
}
