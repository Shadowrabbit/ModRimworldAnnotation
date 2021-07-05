using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001200 RID: 4608
	public class CompProperties_RitualTargetEffecterSpawner : CompProperties
	{
		// Token: 0x06006ECA RID: 28362 RVA: 0x002510CE File Offset: 0x0024F2CE
		public CompProperties_RitualTargetEffecterSpawner()
		{
			this.compClass = typeof(CompRitualTargetEffecterSpawner);
		}

		// Token: 0x04003D56 RID: 15702
		public EffecterDef effecter;

		// Token: 0x04003D57 RID: 15703
		public float minRitualProgress;
	}
}
