using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001864 RID: 6244
	public class CompProperties_SpawnSubplant : CompProperties
	{
		// Token: 0x06008A86 RID: 35462 RVA: 0x0005CE5E File Offset: 0x0005B05E
		public CompProperties_SpawnSubplant()
		{
			this.compClass = typeof(CompSpawnSubplant);
		}

		// Token: 0x040058E3 RID: 22755
		public ThingDef subplant;

		// Token: 0x040058E4 RID: 22756
		public SoundDef spawnSound;
	}
}
