using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F22 RID: 3874
	public class CompProperties_Usable : CompProperties
	{
		// Token: 0x0600558B RID: 21899 RVA: 0x0003B601 File Offset: 0x00039801
		public CompProperties_Usable()
		{
			this.compClass = typeof(CompUsable);
		}

		// Token: 0x040036B8 RID: 14008
		public JobDef useJob;

		// Token: 0x040036B9 RID: 14009
		[MustTranslate]
		public string useLabel;

		// Token: 0x040036BA RID: 14010
		public int useDuration = 100;
	}
}
