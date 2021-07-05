using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000013 RID: 19
	public static class ColorIntUtility
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00005C1A File Offset: 0x00003E1A
		public static ColorInt AsColorInt(this Color32 col)
		{
			return new ColorInt(col);
		}
	}
}
