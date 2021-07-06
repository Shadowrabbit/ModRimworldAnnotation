using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F09 RID: 3849
	public class CompProperties_EggLayer : CompProperties
	{
		// Token: 0x06005530 RID: 21808 RVA: 0x001C7B88 File Offset: 0x001C5D88
		public CompProperties_EggLayer()
		{
			this.compClass = typeof(CompEggLayer);
		}

		// Token: 0x0400362B RID: 13867
		public float eggLayIntervalDays = 1f;

		// Token: 0x0400362C RID: 13868
		public IntRange eggCountRange = IntRange.one;

		// Token: 0x0400362D RID: 13869
		public ThingDef eggUnfertilizedDef;

		// Token: 0x0400362E RID: 13870
		public ThingDef eggFertilizedDef;

		// Token: 0x0400362F RID: 13871
		public int eggFertilizationCountMax = 1;

		// Token: 0x04003630 RID: 13872
		public bool eggLayFemaleOnly = true;

		// Token: 0x04003631 RID: 13873
		public float eggProgressUnfertilizedMax = 1f;
	}
}
