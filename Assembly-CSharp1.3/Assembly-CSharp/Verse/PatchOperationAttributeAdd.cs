using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200024F RID: 591
	public class PatchOperationAttributeAdd : PatchOperationAttribute
	{
		// Token: 0x060010F1 RID: 4337 RVA: 0x0006005C File Offset: 0x0005E25C
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] == null)
				{
					XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(this.attribute);
					xmlAttribute.Value = this.value;
					xmlNode.Attributes.Append(xmlAttribute);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x04000CE2 RID: 3298
		protected string value;
	}
}
