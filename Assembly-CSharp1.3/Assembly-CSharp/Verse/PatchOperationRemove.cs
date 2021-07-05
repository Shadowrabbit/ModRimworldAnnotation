using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x0200024B RID: 587
	public class PatchOperationRemove : PatchOperationPathed
	{
		// Token: 0x060010E9 RID: 4329 RVA: 0x0005FEC0 File Offset: 0x0005E0C0
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
