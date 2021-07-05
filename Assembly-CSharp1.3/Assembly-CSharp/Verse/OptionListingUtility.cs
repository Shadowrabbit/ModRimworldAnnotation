using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000435 RID: 1077
	public static class OptionListingUtility
	{
		// Token: 0x0600206C RID: 8300 RVA: 0x000C8EEC File Offset: 0x000C70EC
		public static float DrawOptionListing(Rect rect, List<ListableOption> optList)
		{
			float num = 0f;
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			foreach (ListableOption listableOption in optList)
			{
				num += listableOption.DrawOption(new Vector2(0f, num), rect.width) + 7f;
			}
			GUI.EndGroup();
			return num;
		}

		// Token: 0x040013AC RID: 5036
		private const float OptionSpacing = 7f;
	}
}
