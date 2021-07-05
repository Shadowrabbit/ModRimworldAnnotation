using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200036F RID: 879
	public class PatchOperationConditional : PatchOperationPathed
	{
		// Token: 0x06001636 RID: 5686 RVA: 0x000D5718 File Offset: 0x000D3918
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

		// Token: 0x04001106 RID: 4358
		private PatchOperation match;

		// Token: 0x04001107 RID: 4359
		private PatchOperation nomatch;
	}
}
