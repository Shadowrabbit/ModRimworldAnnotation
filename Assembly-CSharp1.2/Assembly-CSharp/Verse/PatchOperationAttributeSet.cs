using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200036C RID: 876
	public class PatchOperationAttributeSet : PatchOperationAttribute
	{
		// Token: 0x0600162E RID: 5678 RVA: 0x000D5594 File Offset: 0x000D3794
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] != null)
				{
					xmlNode.Attributes[this.attribute].Value = this.value;
				}
				else
				{
					XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(this.attribute);
					xmlAttribute.Value = this.value;
					xmlNode.Attributes.Append(xmlAttribute);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04001103 RID: 4355
		protected string value;
	}
}
