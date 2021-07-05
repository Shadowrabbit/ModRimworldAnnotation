using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003ED RID: 1005
	public static class BeautyDrawer
	{
		// Token: 0x06001E49 RID: 7753 RVA: 0x000BD6C4 File Offset: 0x000BB8C4
		public static void BeautyDrawerOnGUI()
		{
			if (Event.current.type != EventType.Repaint || !BeautyDrawer.ShouldShow())
			{
				return;
			}
			BeautyDrawer.DrawBeautyAroundMouse();
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000BD6E0 File Offset: 0x000BB8E0
		private static bool ShouldShow()
		{
			return Find.PlaySettings.showBeauty && !Mouse.IsInputBlockedNow && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap);
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x000BD720 File Offset: 0x000BB920
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

		// Token: 0x06001E4C RID: 7756 RVA: 0x000BD7B8 File Offset: 0x000BB9B8
		public static Color BeautyColor(float beauty, float scale)
		{
			float num = Mathf.InverseLerp(-scale, scale, beauty);
			num = Mathf.Clamp01(num);
			return Color.Lerp(Color.Lerp(BeautyDrawer.ColorUgly, BeautyDrawer.ColorBeautiful, num), Color.white, 0.5f);
		}

		// Token: 0x04001261 RID: 4705
		private static List<Thing> beautyCountedThings = new List<Thing>();

		// Token: 0x04001262 RID: 4706
		private static Color ColorUgly = Color.red;

		// Token: 0x04001263 RID: 4707
		private static Color ColorBeautiful = Color.green;
	}
}
