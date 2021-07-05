using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000096 RID: 150
	public class ColorOption
	{
		// Token: 0x06000520 RID: 1312 RVA: 0x0001A6F0 File Offset: 0x000188F0
		public Color RandomizedColor()
		{
			if (this.only.a >= 0f)
			{
				return this.only;
			}
			return new Color(Rand.Range(this.min.r, this.max.r), Rand.Range(this.min.g, this.max.g), Rand.Range(this.min.b, this.max.b), Rand.Range(this.min.a, this.max.a));
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001A787 File Offset: 0x00018987
		public void SetSingle(Color color)
		{
			this.only = color;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001A790 File Offset: 0x00018990
		public void SetMin(Color color)
		{
			this.min = color;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001A799 File Offset: 0x00018999
		public void SetMax(Color color)
		{
			this.max = color;
		}

		// Token: 0x04000256 RID: 598
		public float weight = 1f;

		// Token: 0x04000257 RID: 599
		public Color min = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x04000258 RID: 600
		public Color max = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x04000259 RID: 601
		public Color only = new Color(-1f, -1f, -1f, -1f);
	}
}
