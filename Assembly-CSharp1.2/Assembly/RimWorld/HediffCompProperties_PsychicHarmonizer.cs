using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DF8 RID: 7672
	public class HediffCompProperties_PsychicHarmonizer : HediffCompProperties
	{
		// Token: 0x0600A639 RID: 42553 RVA: 0x0006DF92 File Offset: 0x0006C192
		public HediffCompProperties_PsychicHarmonizer()
		{
			this.compClass = typeof(HediffComp_PsychicHarmonizer);
		}

		// Token: 0x040070B1 RID: 28849
		public float range;

		// Token: 0x040070B2 RID: 28850
		public ThoughtDef thought;
	}
}
