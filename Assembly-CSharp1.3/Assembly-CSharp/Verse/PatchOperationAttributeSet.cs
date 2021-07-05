using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000251 RID: 593
	public class PatchOperationAttributeSet : PatchOperationAttribute
	{
		// Token: 0x060010F5 RID: 4341 RVA: 0x00060194 File Offset: 0x0005E394
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

		// Token: 0x04000CE3 RID: 3299
		protected string value;
	}
}
