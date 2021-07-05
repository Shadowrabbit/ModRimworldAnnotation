using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000245 RID: 581
	public class XmlContainer
	{
		// Token: 0x060010DA RID: 4314 RVA: 0x0005FA38 File Offset: 0x0005DC38
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.node = xmlRoot;
		}

		// Token: 0x04000CD5 RID: 3285
		public XmlNode node;
	}
}
