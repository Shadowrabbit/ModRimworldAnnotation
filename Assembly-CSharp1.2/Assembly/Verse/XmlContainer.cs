using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200035D RID: 861
	public class XmlContainer
	{
		// Token: 0x06001614 RID: 5652 RVA: 0x00015B57 File Offset: 0x00013D57
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.node = xmlRoot;
		}

		// Token: 0x040010EA RID: 4330
		public XmlNode node;
	}
}
