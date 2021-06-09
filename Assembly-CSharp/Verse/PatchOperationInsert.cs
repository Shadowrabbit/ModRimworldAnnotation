using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000364 RID: 868
	public class PatchOperationInsert : PatchOperationPathed
	{
		// Token: 0x06001620 RID: 5664 RVA: 0x000D51A4 File Offset: 0x000D33A4
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				result = true;
				XmlNode xmlNode = obj as XmlNode;
				XmlNode parentNode = xmlNode.ParentNode;
				if (this.order == PatchOperationInsert.Order.Append)
				{
					using (IEnumerator enumerator2 = node.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode node2 = (XmlNode)obj2;
							parentNode.InsertAfter(parentNode.OwnerDocument.ImportNode(node2, true), xmlNode);
						}
						goto IL_E0;
					}
					goto IL_98;
				}
				goto IL_98;
				IL_E0:
				if (xmlNode.NodeType == XmlNodeType.Text)
				{
					parentNode.Normalize();
					continue;
				}
				continue;
				IL_98:
				if (this.order == PatchOperationInsert.Order.Prepend)
				{
					for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
					{
						parentNode.InsertBefore(parentNode.OwnerDocument.ImportNode(node.ChildNodes[i], true), xmlNode);
					}
					goto IL_E0;
				}
				goto IL_E0;
			}
			return result;
		}

		// Token: 0x040010FA RID: 4346
		private XmlContainer value;

		// Token: 0x040010FB RID: 4347
		private PatchOperationInsert.Order order = PatchOperationInsert.Order.Prepend;

		// Token: 0x02000365 RID: 869
		private enum Order
		{
			// Token: 0x040010FD RID: 4349
			Append,
			// Token: 0x040010FE RID: 4350
			Prepend
		}
	}
}
