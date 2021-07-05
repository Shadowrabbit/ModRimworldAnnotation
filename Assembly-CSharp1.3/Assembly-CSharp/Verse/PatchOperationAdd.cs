using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000248 RID: 584
	public class PatchOperationAdd : PatchOperationPathed
	{
		// Token: 0x060010E3 RID: 4323 RVA: 0x0005FB4C File Offset: 0x0005DD4C
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				result = true;
				XmlNode xmlNode = obj as XmlNode;
				if (this.order == PatchOperationAdd.Order.Append)
				{
					using (IEnumerator enumerator2 = node.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode node2 = (XmlNode)obj2;
							xmlNode.AppendChild(xmlNode.OwnerDocument.ImportNode(node2, true));
						}
						continue;
					}
				}
				if (this.order == PatchOperationAdd.Order.Prepend)
				{
					for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
					{
						xmlNode.PrependChild(xmlNode.OwnerDocument.ImportNode(node.ChildNodes[i], true));
					}
				}
			}
			return result;
		}

		// Token: 0x04000CDA RID: 3290
		private XmlContainer value;

		// Token: 0x04000CDB RID: 3291
		private PatchOperationAdd.Order order;

		// Token: 0x020019CA RID: 6602
		private enum Order
		{
			// Token: 0x040062F7 RID: 25335
			Append,
			// Token: 0x040062F8 RID: 25336
			Prepend
		}
	}
}
