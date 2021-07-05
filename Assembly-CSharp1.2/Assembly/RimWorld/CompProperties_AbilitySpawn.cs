using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200138D RID: 5005
	public class CompProperties_AbilitySpawn : CompProperties_AbilityEffect
	{
		// Token: 0x06006CA2 RID: 27810 RVA: 0x00049E04 File Offset: 0x00048004
		public CompProperties_AbilitySpawn()
		{
			this.compClass = typeof(CompAbilityEffect_Spawn);
		}

		// Token: 0x04004803 RID: 18435
		public ThingDef thingDef;

		// Token: 0x04004804 RID: 18436
		public bool allowOnBuildings = true;
	}
}
