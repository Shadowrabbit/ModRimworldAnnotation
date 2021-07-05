using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000361 RID: 865
	public class PatchOperationAdd : PatchOperationPathed
	{
		// Token: 0x0600161C RID: 5660 RVA: 0x000D4F84 File Offset: 0x000D3184
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

		// Token: 0x040010F4 RID: 4340
		private XmlContainer value;

		// Token: 0x040010F5 RID: 4341
		private PatchOperationAdd.Order order;

		// Token: 0x02000362 RID: 866
		private enum Order
		{
			// Token: 0x040010F7 RID: 4343
			Append,
			// Token: 0x040010F8 RID: 4344
			Prepend
		}
	}
}
