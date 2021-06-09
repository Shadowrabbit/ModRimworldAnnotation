using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200011E RID: 286
	public struct SkyColorSet
	{
		// Token: 0x060007D2 RID: 2002 RVA: 0x0000C37F File Offset: 0x0000A57F
		public SkyColorSet(Color sky, Color shadow, Color overlay, float saturation)
		{
			this.sky = sky;
			this.shadow = shadow;
			this.overlay = overlay;
			this.saturation = saturation;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00093EC8 File Offset: 0x000920C8
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

		// Token: 0x060007D4 RID: 2004 RVA: 0x00093F44 File Offset: 0x00092144
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

		// Token: 0x0400054F RID: 1359
		public Color sky;

		// Token: 0x04000550 RID: 1360
		public Color shadow;

		// Token: 0x04000551 RID: 1361
		public Color overlay;

		// Token: 0x04000552 RID: 1362
		public float saturation;
	}
}
