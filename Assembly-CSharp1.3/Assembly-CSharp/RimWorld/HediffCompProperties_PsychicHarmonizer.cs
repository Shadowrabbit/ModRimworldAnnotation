using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200156F RID: 5487
	public class HediffCompProperties_PsychicHarmonizer : HediffCompProperties
	{
		// Token: 0x060081D1 RID: 33233 RVA: 0x002DE2B2 File Offset: 0x002DC4B2
		public HediffCompProperties_PsychicHarmonizer()
		{
			this.compClass = typeof(HediffComp_PsychicHarmonizer);
		}

		// Token: 0x040050CB RID: 20683
		public float range;

		// Token: 0x040050CC RID: 20684
		public ThoughtDef thought;
	}
}
