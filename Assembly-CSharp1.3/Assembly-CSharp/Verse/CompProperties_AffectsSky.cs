using System;

namespace Verse
{
	// Token: 0x02000098 RID: 152
	public class CompProperties_AffectsSky : CompProperties
	{
		// Token: 0x0600052C RID: 1324 RVA: 0x0001A876 File Offset: 0x00018A76
		public CompProperties_AffectsSky()
		{
			this.compClass = typeof(CompAffectsSky);
		}

		// Token: 0x0400025B RID: 603
		public float glow = 1f;

		// Token: 0x0400025C RID: 604
		public SkyColorSet skyColors;

		// Token: 0x0400025D RID: 605
		public float lightsourceShineSize = 1f;

		// Token: 0x0400025E RID: 606
		public float lightsourceShineIntensity = 1f;

		// Token: 0x0400025F RID: 607
		public bool lerpDarken;
	}
}
