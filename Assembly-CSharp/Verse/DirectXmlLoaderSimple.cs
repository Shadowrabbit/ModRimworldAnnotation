using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200048A RID: 1162
	public static class DirectXmlLoaderSimple
	{
		// Token: 0x06001D44 RID: 7492 RVA: 0x0001A50F File Offset: 0x0001870F
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

		// Token: 0x0200048B RID: 1163
		public struct XmlKeyValuePair
		{
			// Token: 0x040014E8 RID: 5352
			public string key;

			// Token: 0x040014E9 RID: 5353
			public string value;

			// Token: 0x040014EA RID: 5354
			public int lineNumber;
		}
	}
}
