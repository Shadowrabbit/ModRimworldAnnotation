using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001576 RID: 5494
	public class CompProperties_RitualHediffGiverInRoom : CompProperties
	{
		// Token: 0x060081EC RID: 33260 RVA: 0x002DEA25 File Offset: 0x002DCC25
		public CompProperties_RitualHediffGiverInRoom()
		{
			this.compClass = typeof(CompRitualHediffGiverInRoom);
		}

		// Token: 0x040050D3 RID: 20691
		public HediffDef hediff;

		// Token: 0x040050D4 RID: 20692
		public float minRadius = 999f;

		// Token: 0x040050D5 RID: 20693
		public float severity = -1f;

		// Token: 0x040050D6 RID: 20694
		public bool resetLastRecreationalDrugTick;
	}
}
