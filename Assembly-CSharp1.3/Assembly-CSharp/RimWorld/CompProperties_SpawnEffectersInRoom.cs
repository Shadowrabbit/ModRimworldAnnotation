using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001578 RID: 5496
	public class CompProperties_SpawnEffectersInRoom : CompProperties
	{
		// Token: 0x060081F0 RID: 33264 RVA: 0x002DEBCC File Offset: 0x002DCDCC
		public CompProperties_SpawnEffectersInRoom()
		{
			this.compClass = typeof(CompSpawnEffectersInRoom);
		}

		// Token: 0x040050D8 RID: 20696
		public EffecterDef effecter;

		// Token: 0x040050D9 RID: 20697
		public float radius = 10f;
	}
}
