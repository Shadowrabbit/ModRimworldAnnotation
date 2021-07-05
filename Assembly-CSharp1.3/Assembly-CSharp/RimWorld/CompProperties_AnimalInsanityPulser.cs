using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F3 RID: 2547
	public class CompProperties_AnimalInsanityPulser : CompProperties
	{
		// Token: 0x06003EC7 RID: 16071 RVA: 0x001573F4 File Offset: 0x001555F4
		public CompProperties_AnimalInsanityPulser()
		{
			this.compClass = typeof(CompAnimalInsanityPulser);
		}

		// Token: 0x04002191 RID: 8593
		public IntRange pulseInterval = new IntRange(60000, 150000);

		// Token: 0x04002192 RID: 8594
		public int radius = 25;
	}
}
