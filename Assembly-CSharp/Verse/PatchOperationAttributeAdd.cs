using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200036A RID: 874
	public class PatchOperationAttributeAdd : PatchOperationAttribute
	{
		// Token: 0x0600162A RID: 5674 RVA: 0x000D5464 File Offset: 0x000D3664
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

		// Token: 0x04001102 RID: 4354
		protected string value;
	}
}
