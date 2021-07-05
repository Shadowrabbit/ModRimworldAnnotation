using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A1A RID: 2586
	public class CompProperties_Schedule : CompProperties
	{
		// Token: 0x06003F13 RID: 16147 RVA: 0x001580B2 File Offset: 0x001562B2
		public CompProperties_Schedule()
		{
			this.compClass = typeof(CompSchedule);
		}

		// Token: 0x04002224 RID: 8740
		public float startTime;

		// Token: 0x04002225 RID: 8741
		public float endTime = 1f;

		// Token: 0x04002226 RID: 8742
		[MustTranslate]
		public string offMessage;
	}
}
