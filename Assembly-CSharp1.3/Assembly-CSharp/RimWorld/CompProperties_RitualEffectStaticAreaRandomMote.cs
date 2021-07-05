using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBD RID: 4029
	public class CompProperties_RitualEffectStaticAreaRandomMote : CompProperties_RitualEffectIntervalSpawnArea
	{
		// Token: 0x06005F02 RID: 24322 RVA: 0x00208278 File Offset: 0x00206478
		public CompProperties_RitualEffectStaticAreaRandomMote()
		{
			this.compClass = typeof(CompRitualEffect_StaticAreaRandomMote);
		}

		// Token: 0x040036C1 RID: 14017
		public float minDist = 1.5f;

		// Token: 0x040036C2 RID: 14018
		public List<ThingDef> moteDefs;
	}
}
