using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000253 RID: 595
	public class PatchOperationTest : PatchOperationPathed
	{
		// Token: 0x060010FB RID: 4347 RVA: 0x00060326 File Offset: 0x0005E526
		protected override bool ApplyWorker(XmlDocument xml)
		{
			return xml.SelectSingleNode(this.xpath) != null;
		}
	}
}
