using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EFE RID: 3838
	public class CompProperties_AnimalInsanityPulser : CompProperties
	{
		// Token: 0x06005505 RID: 21765 RVA: 0x0003AEE8 File Offset: 0x000390E8
		public CompProperties_AnimalInsanityPulser()
		{
			this.compClass = typeof(CompAnimalInsanityPulser);
		}

		// Token: 0x04003604 RID: 13828
		public IntRange pulseInterval = new IntRange(60000, 150000);

		// Token: 0x04003605 RID: 13829
		public int radius = 25;
	}
}
