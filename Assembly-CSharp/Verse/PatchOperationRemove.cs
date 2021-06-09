using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000366 RID: 870
	public class PatchOperationRemove : PatchOperationPathed
	{
		// Token: 0x06001622 RID: 5666 RVA: 0x000D52E0 File Offset: 0x000D34E0
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				xmlNode.ParentNode.RemoveChild(xmlNode);
			}
			return result;
		}
	}
}
