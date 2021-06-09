using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000317 RID: 791
	public struct SkyTarget
	{
		// Token: 0x06001426 RID: 5158 RVA: 0x000147FF File Offset: 0x000129FF
		public SkyTarget(float glow, SkyColorSet colorSet, float lightsourceShineSize, float lightsourceShineIntensity)
		{
			this.glow = glow;
			this.lightsourceShineSize = lightsourceShineSize;
			this.lightsourceShineIntensity = lightsourceShineIntensity;
			this.colors = colorSet;
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x000CDBE0 File Offset: 0x000CBDE0
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

		// Token: 0x06001428 RID: 5160 RVA: 0x000CDC5C File Offset: 0x000CBE5C
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

		// Token: 0x06001429 RID: 5161 RVA: 0x000CDCF8 File Offset: 0x000CBEF8
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

		// Token: 0x04000FD7 RID: 4055
		public float glow;

		// Token: 0x04000FD8 RID: 4056
		public SkyColorSet colors;

		// Token: 0x04000FD9 RID: 4057
		public float lightsourceShineSize;

		// Token: 0x04000FDA RID: 4058
		public float lightsourceShineIntensity;
	}
}
