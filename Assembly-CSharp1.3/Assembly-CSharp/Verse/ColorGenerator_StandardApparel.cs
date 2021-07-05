using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000094 RID: 148
	public class ColorGenerator_StandardApparel : ColorGenerator
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0001A4DE File Offset: 0x000186DE
		public override Color ExemplaryColor
		{
			get
			{
				return new Color(0.7f, 0.7f, 0.7f);
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001A4F4 File Offset: 0x000186F4
		public override Color NewRandomizedColor()
		{
			if (Rand.Value < 0.1f)
			{
				return Color.white;
			}
			if (Rand.Value < 0.1f)
			{
				return new Color(0.4f, 0.4f, 0.4f);
			}
			Color white = Color.white;
			float num = Rand.Range(0f, 0.6f);
			white.r -= num * Rand.Value;
			white.g -= num * Rand.Value;
			white.b -= num * Rand.Value;
			if (Rand.Value < 0.2f)
			{
				white.r *= 0.4f;
				white.g *= 0.4f;
				white.b *= 0.4f;
			}
			return white;
		}

		// Token: 0x04000254 RID: 596
		private const float DarkAmp = 0.4f;
	}
}
