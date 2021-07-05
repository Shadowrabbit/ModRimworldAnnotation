using System;

namespace Verse
{
	// Token: 0x02000369 RID: 873
	public abstract class PatchOperationAttribute : PatchOperationPathed
	{
		// Token: 0x06001628 RID: 5672 RVA: 0x00015BB4 File Offset: 0x00013DB4
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.attribute);
		}

		// Token: 0x04001101 RID: 4353
		protected string attribute;
	}
}
