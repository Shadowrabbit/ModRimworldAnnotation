using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace RimWorld.IO
{
	// Token: 0x02002204 RID: 8708
	public static class VirtualFileInfoExt
	{
		// Token: 0x0600BAD4 RID: 47828 RVA: 0x0035A77C File Offset: 0x0035897C
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
