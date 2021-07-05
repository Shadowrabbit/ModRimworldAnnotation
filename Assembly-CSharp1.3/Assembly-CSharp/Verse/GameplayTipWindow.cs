using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000039 RID: 57
	public class GameplayTipWindow
	{
		// Token: 0x0600032F RID: 815 RVA: 0x00011548 File Offset: 0x0000F748
		public static void DrawWindow(Vector2 offset, bool useWindowStack)
		{
			if (GameplayTipWindow.allTipsCached == null)
			{
				GameplayTipWindow.allTipsCached = DefDatabase<TipSetDef>.AllDefsListForReading.SelectMany((TipSetDef set) => set.tips).InRandomOrder(null).ToList<string>();
			}
			Rect rect = new Rect(offset.x, offset.y, GameplayTipWindow.WindowSize.x, GameplayTipWindow.WindowSize.y);
			if (useWindowStack)
			{
				Find.WindowStack.ImmediateWindow(62893997, rect, WindowLayer.Super, delegate
				{
					GameplayTipWindow.DrawContents(rect.AtZero());
				}, true, false, 1f, null);
				return;
			}
			Widgets.DrawShadowAround(rect);
			Widgets.DrawWindowBackground(rect);
			GameplayTipWindow.DrawContents(rect);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00011618 File Offset: 0x0000F818
		private static void DrawContents(Rect rect)
		{
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			if (Time.realtimeSinceStartup - GameplayTipWindow.lastTimeUpdatedTooltip > 17.5f || GameplayTipWindow.lastTimeUpdatedTooltip < 0f)
			{
				GameplayTipWindow.currentTipIndex = (GameplayTipWindow.currentTipIndex + 1) % GameplayTipWindow.allTipsCached.Count;
				GameplayTipWindow.lastTimeUpdatedTooltip = Time.realtimeSinceStartup;
			}
			Rect rect2 = rect;
			rect2.x += GameplayTipWindow.TextMargin.x;
			rect2.width -= GameplayTipWindow.TextMargin.x * 2f;
			rect2.y += GameplayTipWindow.TextMargin.y;
			rect2.height -= GameplayTipWindow.TextMargin.y * 2f;
			Widgets.Label(rect2, GameplayTipWindow.allTipsCached[GameplayTipWindow.currentTipIndex]);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000116F9 File Offset: 0x0000F8F9
		public static void ResetTipTimer()
		{
			GameplayTipWindow.lastTimeUpdatedTooltip = -1f;
		}

		// Token: 0x040000AD RID: 173
		private static List<string> allTipsCached;

		// Token: 0x040000AE RID: 174
		private static float lastTimeUpdatedTooltip = -1f;

		// Token: 0x040000AF RID: 175
		private static int currentTipIndex = 0;

		// Token: 0x040000B0 RID: 176
		public const float tipUpdateInterval = 17.5f;

		// Token: 0x040000B1 RID: 177
		public static readonly Vector2 WindowSize = new Vector2(776f, 60f);

		// Token: 0x040000B2 RID: 178
		private static readonly Vector2 TextMargin = new Vector2(15f, 8f);
	}
}
