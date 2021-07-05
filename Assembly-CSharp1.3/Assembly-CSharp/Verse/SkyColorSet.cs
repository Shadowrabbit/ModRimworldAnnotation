using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000B6 RID: 182
	public struct SkyColorSet
	{
		// Token: 0x0600059D RID: 1437 RVA: 0x0001CE59 File Offset: 0x0001B059
		public SkyColorSet(Color sky, Color shadow, Color overlay, float saturation)
		{
			this.sky = sky;
			this.shadow = shadow;
			this.overlay = overlay;
			this.saturation = saturation;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001CE78 File Offset: 0x0001B078
		public static SkyColorSet Lerp(SkyColorSet A, SkyColorSet B, float t)
		{
			return new SkyColorSet
			{
				sky = Color.Lerp(A.sky, B.sky, t),
				shadow = Color.Lerp(A.shadow, B.shadow, t),
				overlay = Color.Lerp(A.overlay, B.overlay, t),
				saturation = Mathf.Lerp(A.saturation, B.saturation, t)
			};
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001CEF4 File Offset: 0x0001B0F4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(sky=",
				this.sky,
				", shadow=",
				this.shadow,
				", overlay=",
				this.overlay,
				", sat=",
				this.saturation,
				")"
			});
		}

		// Token: 0x04000367 RID: 871
		public Color sky;

		// Token: 0x04000368 RID: 872
		public Color shadow;

		// Token: 0x04000369 RID: 873
		public Color overlay;

		// Token: 0x0400036A RID: 874
		public float saturation;
	}
}
