using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x02000312 RID: 786
	public static class DirectXmlLoaderSimple
	{
		// Token: 0x0600169A RID: 5786 RVA: 0x00083C31 File Offset: 0x00081E31
		public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(VirtualFile file)
		{
			XDocument xdocument = file.LoadAsXDocument();
			foreach (XElement xelement in xdocument.Root.Elements())
			{
				string key = xelement.Name.ToString();
				string text = xelement.Value;
				text = text.Replace("\\n", "\n");
				yield return new DirectXmlLoaderSimple.XmlKeyValuePair
				{
					key = key,
					value = text,
					lineNumber = ((IXmlLineInfo)xelement).LineNumber
				};
			}
			IEnumerator<XElement> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x02001A4C RID: 6732
		public struct XmlKeyValuePair
		{
			// Token: 0x040064AF RID: 25775
			public string key;

			// Token: 0x040064B0 RID: 25776
			public string value;

			// Token: 0x040064B1 RID: 25777
			public int lineNumber;
		}
	}
}
