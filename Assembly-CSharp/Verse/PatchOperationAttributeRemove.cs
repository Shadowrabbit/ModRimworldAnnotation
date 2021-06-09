using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200036B RID: 875
	public class PatchOperationAttributeRemove : PatchOperationAttribute
	{
		// Token: 0x0600162C RID: 5676 RVA: 0x000D5504 File Offset: 0x000D3704
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
