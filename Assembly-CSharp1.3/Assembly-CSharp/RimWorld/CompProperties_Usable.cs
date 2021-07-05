using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A12 RID: 2578
	public class CompProperties_Usable : CompProperties
	{
		// Token: 0x06003F0B RID: 16139 RVA: 0x00157FAC File Offset: 0x001561AC
		public CompProperties_Usable()
		{
			this.compClass = typeof(CompUsable);
		}

		// Token: 0x04002217 RID: 8727
		public JobDef useJob;

		// Token: 0x04002218 RID: 8728
		[MustTranslate]
		public string useLabel;

		// Token: 0x04002219 RID: 8729
		public int useDuration = 100;
	}
}
