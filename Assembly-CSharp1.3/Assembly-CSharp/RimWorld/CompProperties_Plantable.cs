using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200116E RID: 4462
	public class CompProperties_Plantable : CompProperties
	{
		// Token: 0x06006B26 RID: 27430 RVA: 0x0023F729 File Offset: 0x0023D929
		public CompProperties_Plantable()
		{
			this.compClass = typeof(CompPlantable);
		}

		// Token: 0x04003B9B RID: 15259
		public ThingDef plantDefToSpawn;
	}
}
