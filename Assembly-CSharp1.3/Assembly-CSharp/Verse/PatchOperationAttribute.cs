using System;

namespace Verse
{
	// Token: 0x0200024E RID: 590
	public abstract class PatchOperationAttribute : PatchOperationPathed
	{
		// Token: 0x060010EF RID: 4335 RVA: 0x00060043 File Offset: 0x0005E243
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.attribute);
		}

		// Token: 0x04000CE1 RID: 3297
		protected string attribute;
	}
}
