using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000226 RID: 550
	public struct SkyTarget
	{
		// Token: 0x06000FA0 RID: 4000 RVA: 0x0005902C File Offset: 0x0005722C
		public SkyTarget(float glow, SkyColorSet colorSet, float lightsourceShineSize, float lightsourceShineIntensity)
		{
			this.glow = glow;
			this.lightsourceShineSize = lightsourceShineSize;
			this.lightsourceShineIntensity = lightsourceShineIntensity;
			this.colors = colorSet;
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0005904C File Offset: 0x0005724C
		public static SkyTarget Lerp(SkyTarget A, SkyTarget B, float t)
		{
			return new SkyTarget
			{
				colors = SkyColorSet.Lerp(A.colors, B.colors, t),
				glow = Mathf.Lerp(A.glow, B.glow, t),
				lightsourceShineSize = Mathf.Lerp(A.lightsourceShineSize, B.lightsourceShineSize, t),
				lightsourceShineIntensity = Mathf.Lerp(A.lightsourceShineIntensity, B.lightsourceShineIntensity, t)
			};
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x000590C8 File Offset: 0x000572C8
		public static SkyTarget LerpDarken(SkyTarget A, SkyTarget B, float t)
		{
			return new SkyTarget
			{
				colors = SkyColorSet.Lerp(A.colors, B.colors, t),
				glow = Mathf.Lerp(A.glow, Mathf.Min(A.glow, B.glow), t),
				lightsourceShineSize = Mathf.Lerp(A.lightsourceShineSize, Mathf.Min(A.lightsourceShineSize, B.lightsourceShineSize), t),
				lightsourceShineIntensity = Mathf.Lerp(A.lightsourceShineIntensity, Mathf.Min(A.lightsourceShineIntensity, B.lightsourceShineIntensity), t)
			};
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00059164 File Offset: 0x00057364
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(glow=",
				this.glow.ToString("F2"),
				", colors=",
				this.colors.ToString(),
				", lightsourceShineSize=",
				this.lightsourceShineSize.ToString(),
				", lightsourceShineIntensity=",
				this.lightsourceShineIntensity.ToString(),
				")"
			});
		}

		// Token: 0x04000C4C RID: 3148
		public float glow;

		// Token: 0x04000C4D RID: 3149
		public SkyColorSet colors;

		// Token: 0x04000C4E RID: 3150
		public float lightsourceShineSize;

		// Token: 0x04000C4F RID: 3151
		public float lightsourceShineIntensity;
	}
}
