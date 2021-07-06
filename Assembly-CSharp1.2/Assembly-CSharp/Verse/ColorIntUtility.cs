using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000017 RID: 23
	public static class ColorIntUtility
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x0000752D File Offset: 0x0000572D
		public static ColorInt AsColorInt(this Color32 col)
		{
			return new ColorInt(col);
		}
	}
}
