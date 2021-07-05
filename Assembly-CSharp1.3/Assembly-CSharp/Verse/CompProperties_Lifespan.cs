using System;

namespace Verse
{
	// Token: 0x0200009A RID: 154
	public class CompProperties_Lifespan : CompProperties
	{
		// Token: 0x0600052E RID: 1326 RVA: 0x0001A8DD File Offset: 0x00018ADD
		public CompProperties_Lifespan()
		{
			this.compClass = typeof(CompLifespan);
		}

		// Token: 0x04000263 RID: 611
		public int lifespanTicks = 100;

		// Token: 0x04000264 RID: 612
		public EffecterDef expireEffect;

		// Token: 0x04000265 RID: 613
		public ThingDef plantDefToSpawn;
	}
}
