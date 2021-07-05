using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000F7 RID: 247
	public class ColorOption
	{
		// Token: 0x06000715 RID: 1813 RVA: 0x00090CEC File Offset: 0x0008EEEC
		public Color RandomizedColor()
		{
			if (this.only.a >= 0f)
			{
				return this.only;
			}
			return new Color(Rand.Range(this.min.r, this.max.r), Rand.Range(this.min.g, this.max.g), Rand.Range(this.min.b, this.max.b), Rand.Range(this.min.a, this.max.a));
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0000BC45 File Offset: 0x00009E45
		public void SetSingle(Color color)
		{
			this.only = color;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0000BC4E File Offset: 0x00009E4E
		public void SetMin(Color color)
		{
			this.min = color;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0000BC57 File Offset: 0x00009E57
		public void SetMax(Color color)
		{
			this.max = color;
		}

		// Token: 0x04000421 RID: 1057
		public float weight = 1f;

		// Token: 0x04000422 RID: 1058
		public Color min = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x04000423 RID: 1059
		public Color max = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x04000424 RID: 1060
		public Color only = new Color(-1f, -1f, -1f, -1f);
	}
}
