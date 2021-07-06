using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2A RID: 3882
	public class CompProperties_Schedule : CompProperties
	{
		// Token: 0x06005593 RID: 21907 RVA: 0x0003B6F5 File Offset: 0x000398F5
		public CompProperties_Schedule()
		{
			this.compClass = typeof(CompSchedule);
		}

		// Token: 0x040036C1 RID: 14017
		public float startTime;

		// Token: 0x040036C2 RID: 14018
		public float endTime = 1f;

		// Token: 0x040036C3 RID: 14019
		[MustTranslate]
		public string offMessage;
	}
}
