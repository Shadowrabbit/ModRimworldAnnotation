using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000777 RID: 1911
	public static class OptionListingUtility
	{
		// Token: 0x06003017 RID: 12311 RVA: 0x0013D8F4 File Offset: 0x0013BAF4
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

		// Token: 0x0400209B RID: 8347
		private const float OptionSpacing = 7f;
	}
}
