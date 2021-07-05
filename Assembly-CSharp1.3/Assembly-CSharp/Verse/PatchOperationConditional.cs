using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000254 RID: 596
	public class PatchOperationConditional : PatchOperationPathed
	{
		// Token: 0x060010FD RID: 4349 RVA: 0x00060338 File Offset: 0x0005E538
		protected override bool ApplyWorker(XmlDocument xml)
		{
			if (xml.SelectSingleNode(this.xpath) != null)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			else if (this.nomatch != null)
			{
				return this.nomatch.Apply(xml);
			}
			return this.match != null || this.nomatch != null;
		}

		// Token: 0x04000CE6 RID: 3302
		private PatchOperation match;

		// Token: 0x04000CE7 RID: 3303
		private PatchOperation nomatch;
	}
}
