using System;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x02000314 RID: 788
	public static class DirectXmlSaveLoadUtility
	{
		// Token: 0x0600169E RID: 5790 RVA: 0x00083DF4 File Offset: 0x00081FF4
		public static string GetXPath(this XmlNode node)
		{
			string text = "";
			while (node != null)
			{
				XmlNodeType nodeType = node.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType != XmlNodeType.Attribute)
					{
						if (nodeType == XmlNodeType.Document)
						{
							return text;
						}
						text = "/?" + text;
						node = node.ParentNode;
					}
					else
					{
						text = "/@" + node.Name + text;
						node = ((XmlAttribute)node).OwnerElement;
					}
				}
				else
				{
					bool flag;
					int elementIndexForXPath = DirectXmlSaveLoadUtility.GetElementIndexForXPath((XmlElement)node, out flag);
					if (flag || node.Name == "li")
					{
						text = string.Concat(new object[]
						{
							"/",
							node.Name,
							"[",
							elementIndexForXPath,
							"]",
							text
						});
					}
					else
					{
						text = "/" + node.Name + text;
					}
					node = node.ParentNode;
				}
			}
			return text;
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x00083EE4 File Offset: 0x000820E4
		public static string GetInnerXml(this XElement element)
		{
			if (element == null)
			{
				return "";
			}
			string result;
			using (XmlReader xmlReader = element.CreateReader())
			{
				xmlReader.MoveToContent();
				result = xmlReader.ReadInnerXml();
			}
			return result;
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x00083F2C File Offset: 0x0008212C
		private static int GetElementIndexForXPath(XmlElement element, out bool multiple)
		{
			multiple = false;
			XmlNode parentNode = element.ParentNode;
			if (parentNode is XmlDocument)
			{
				return 1;
			}
			if (!(parentNode is XmlElement))
			{
				return 1;
			}
			foreach (object obj in parentNode.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode is XmlElement && xmlNode.Name == element.Name && xmlNode != element)
				{
					multiple = true;
					break;
				}
			}
			int num = 1;
			foreach (object obj2 in parentNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj2;
				if (xmlNode2 is XmlElement && xmlNode2.Name == element.Name)
				{
					if (xmlNode2 == element)
					{
						return num;
					}
					num++;
				}
			}
			return 1;
		}

		// Token: 0x04000FB8 RID: 4024
		public const BindingFlags FieldGetFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
	}
}
