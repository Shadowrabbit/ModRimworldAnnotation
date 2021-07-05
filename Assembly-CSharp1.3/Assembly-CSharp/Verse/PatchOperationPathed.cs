using System;

namespace Verse
{
	// Token: 0x02000247 RID: 583
	public abstract class PatchOperationPathed : PatchOperation
	{
		// Token: 0x060010E1 RID: 4321 RVA: 0x0005FB2B File Offset: 0x0005DD2B
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.xpath);
		}

		// Token: 0x04000CD9 RID: 3289
		protected string xpath;
	}
}
