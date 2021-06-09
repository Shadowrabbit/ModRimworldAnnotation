using System;

namespace Verse
{
	// Token: 0x020000FD RID: 253
	public class CompProperties_Lifespan : CompProperties
	{
		// Token: 0x06000733 RID: 1843 RVA: 0x0000BD72 File Offset: 0x00009F72
		public CompProperties_Lifespan()
		{
			this.compClass = typeof(CompLifespan);
		}

		// Token: 0x04000437 RID: 1079
		public int lifespanTicks = 100;

		// Token: 0x04000438 RID: 1080
		public EffecterDef expireEffect;
	}
}
