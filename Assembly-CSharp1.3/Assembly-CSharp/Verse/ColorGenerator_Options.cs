using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000095 RID: 149
	public class ColorGenerator_Options : ColorGenerator
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x0001A5C0 File Offset: 0x000187C0
		public override Color ExemplaryColor
		{
			get
			{
				ColorOption colorOption = null;
				for (int i = 0; i < this.options.Count; i++)
				{
					if (colorOption == null || this.options[i].weight > colorOption.weight)
					{
						colorOption = this.options[i];
					}
				}
				if (colorOption == null)
				{
					return Color.white;
				}
				if (colorOption.only.a >= 0f)
				{
					return colorOption.only;
				}
				return new Color((colorOption.min.r + colorOption.max.r) / 2f, (colorOption.min.g + colorOption.max.g) / 2f, (colorOption.min.b + colorOption.max.b) / 2f, (colorOption.min.a + colorOption.max.a) / 2f);
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001A6A9 File Offset: 0x000188A9
		public override Color NewRandomizedColor()
		{
			return this.options.RandomElementByWeight((ColorOption pi) => pi.weight).RandomizedColor();
		}

		// Token: 0x04000255 RID: 597
		public List<ColorOption> options = new List<ColorOption>();
	}
}
