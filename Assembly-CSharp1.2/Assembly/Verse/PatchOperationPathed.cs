using System;

namespace Verse
{
	// Token: 0x02000360 RID: 864
	public abstract class PatchOperationPathed : PatchOperation
	{
		// Token: 0x0600161A RID: 5658 RVA: 0x00015B7D File Offset: 0x00013D7D
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.xpath);
		}

		// Token: 0x040010F3 RID: 4339
		protected string xpath;
	}
}
