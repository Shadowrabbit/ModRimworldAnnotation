using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace RimWorld.IO
{
	// Token: 0x02001825 RID: 6181
	public static class VirtualFileInfoExt
	{
		// Token: 0x06009123 RID: 37155 RVA: 0x003402C0 File Offset: 0x0033E4C0
		public static XDocument LoadAsXDocument(this VirtualFile file)
		{
			XDocument result;
			using (Stream stream = file.CreateReadStream())
			{
				result = XDocument.Load(XmlReader.Create(stream), LoadOptions.SetLineInfo);
			}
			return result;
		}
	}
}
