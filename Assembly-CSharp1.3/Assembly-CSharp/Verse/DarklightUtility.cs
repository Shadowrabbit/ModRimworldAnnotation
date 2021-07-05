using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000187 RID: 391
	public static class DarklightUtility
	{
		// Token: 0x06000B1D RID: 2845 RVA: 0x0003C7C4 File Offset: 0x0003A9C4
		private static bool IsDarklight(Color32 color)
		{
			float f;
			float num;
			float num2;
			Color.RGBToHSV(color, out f, out num, out num2);
			return DarklightUtility.DarklightHueRange.Includes(f);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0003C7ED File Offset: 0x0003A9ED
		public static bool IsDarklightAt(IntVec3 position, Map map)
		{
			return position.InBounds(map) && position.Roofed(map) && DarklightUtility.IsDarklight(map.glowGrid.VisualGlowAt(position));
		}

		// Token: 0x0400093F RID: 2367
		private static FloatRange DarklightHueRange = new FloatRange(0.49f, 0.51f);
	}
}
