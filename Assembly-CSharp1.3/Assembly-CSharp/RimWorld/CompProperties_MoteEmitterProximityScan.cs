using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001163 RID: 4451
	public class CompProperties_MoteEmitterProximityScan : CompProperties_MoteEmitter
	{
		// Token: 0x06006AF0 RID: 27376 RVA: 0x0023E6AB File Offset: 0x0023C8AB
		public CompProperties_MoteEmitterProximityScan()
		{
			this.compClass = typeof(CompMoteEmitterProximityScan);
		}

		// Token: 0x04003B72 RID: 15218
		public float warmupPulseFadeInTime;

		// Token: 0x04003B73 RID: 15219
		public float warmupPulseSolidTime;

		// Token: 0x04003B74 RID: 15220
		public float warmupPulseFadeOutTime;

		// Token: 0x04003B75 RID: 15221
		public SoundDef soundEmitting;
	}
}
