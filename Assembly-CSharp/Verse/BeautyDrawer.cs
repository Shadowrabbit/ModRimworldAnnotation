using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000706 RID: 1798
	public static class BeautyDrawer
	{
		// Token: 0x06002D8F RID: 11663 RVA: 0x00023EB3 File Offset: 0x000220B3
		public static void BeautyDrawerOnGUI()
		{
			if (Event.current.type != EventType.Repaint || !BeautyDrawer.ShouldShow())
			{
				return;
			}
			BeautyDrawer.DrawBeautyAroundMouse();
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x00023ECF File Offset: 0x000220CF
		private static bool ShouldShow()
		{
			return Find.PlaySettings.showBeauty && !Mouse.IsInputBlockedNow && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap);
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x001341C8 File Offset: 0x001323C8
		private static void DrawBeautyAroundMouse()
		{
			if (!Find.PlaySettings.showBeauty)
			{
				return;
			}
			BeautyUtility.FillBeautyRelevantCells(UI.MouseCell(), Find.CurrentMap);
			for (int i = 0; i < BeautyUtility.beautyRelevantCells.Count; i++)
			{
				IntVec3 intVec = BeautyUtility.beautyRelevantCells[i];
				float num = BeautyUtility.CellBeauty(intVec, Find.CurrentMap, BeautyDrawer.beautyCountedThings);
				if (num != 0f)
				{
					GenMapUI.DrawThingLabel(GenMapUI.LabelDrawPosFor(intVec), Mathf.RoundToInt(num).ToStringCached(), BeautyDrawer.BeautyColor(num, 8f));
				}
			}
			BeautyDrawer.beautyCountedThings.Clear();
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x00134260 File Offset: 0x00132460
		public static Color BeautyColor(float beauty, float scale)
		{
			float num = Mathf.InverseLerp(-scale, scale, beauty);
			num = Mathf.Clamp01(num);
			return Color.Lerp(Color.Lerp(BeautyDrawer.ColorUgly, BeautyDrawer.ColorBeautiful, num), Color.white, 0.5f);
		}

		// Token: 0x04001F03 RID: 7939
		private static List<Thing> beautyCountedThings = new List<Thing>();

		// Token: 0x04001F04 RID: 7940
		private static Color ColorUgly = Color.red;

		// Token: 0x04001F05 RID: 7941
		private static Color ColorBeautiful = Color.green;
	}
}
