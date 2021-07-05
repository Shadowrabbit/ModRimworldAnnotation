using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011FE RID: 4606
	public class CompProperties_RitualTargetMoteSpawner : CompProperties
	{
		// Token: 0x06006EC5 RID: 28357 RVA: 0x0025100A File Offset: 0x0024F20A
		public CompProperties_RitualTargetMoteSpawner()
		{
			this.compClass = typeof(CompRitualTargetMoteSpawner);
		}

		// Token: 0x04003D54 RID: 15700
		public ThingDef mote;
	}
}
