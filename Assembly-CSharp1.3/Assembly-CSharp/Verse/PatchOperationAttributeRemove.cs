using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000250 RID: 592
	public class PatchOperationAttributeRemove : PatchOperationAttribute
	{
		// Token: 0x060010F3 RID: 4339 RVA: 0x00060104 File Offset: 0x0005E304
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] != null)
				{
					xmlNode.Attributes.Remove(xmlNode.Attributes[this.attribute]);
					result = true;
				}
			}
			return result;
		}
	}
}
