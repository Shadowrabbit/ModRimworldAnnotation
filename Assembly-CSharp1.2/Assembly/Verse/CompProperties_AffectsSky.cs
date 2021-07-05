using System;

namespace Verse
{
	// Token: 0x020000FB RID: 251
	public class CompProperties_AffectsSky : CompProperties
	{
		// Token: 0x06000731 RID: 1841 RVA: 0x0000BD0B File Offset: 0x00009F0B
		public CompProperties_AffectsSky()
		{
			this.compClass = typeof(CompAffectsSky);
		}

		// Token: 0x0400042F RID: 1071
		public float glow = 1f;

		// Token: 0x04000430 RID: 1072
		public SkyColorSet skyColors;

		// Token: 0x04000431 RID: 1073
		public float lightsourceShineSize = 1f;

		// Token: 0x04000432 RID: 1074
		public float lightsourceShineIntensity = 1f;

		// Token: 0x04000433 RID: 1075
		public bool lerpDarken;
	}
}
