using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017FA RID: 6138
	public class CompProperties_MoteEmitterProximityScan : CompProperties_MoteEmitter
	{
		// Token: 0x060087D8 RID: 34776 RVA: 0x0005B1F8 File Offset: 0x000593F8
		public CompProperties_MoteEmitterProximityScan()
		{
			this.compClass = typeof(CompMoteEmitterProximityScan);
		}

		// Token: 0x0400571C RID: 22300
		public float warmupPulseFadeInTime;

		// Token: 0x0400571D RID: 22301
		public float warmupPulseSolidTime;

		// Token: 0x0400571E RID: 22302
		public float warmupPulseFadeOutTime;

		// Token: 0x0400571F RID: 22303
		public SoundDef soundEmitting;
	}
}
