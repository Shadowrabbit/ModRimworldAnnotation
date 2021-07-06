using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200036E RID: 878
	public class PatchOperationTest : PatchOperationPathed
	{
		// Token: 0x06001634 RID: 5684 RVA: 0x00015BE4 File Offset: 0x00013DE4
		protected override bool ApplyWorker(XmlDocument xml)
		{
			return xml.SelectSingleNode(this.xpath) != null;
		}
	}
}
