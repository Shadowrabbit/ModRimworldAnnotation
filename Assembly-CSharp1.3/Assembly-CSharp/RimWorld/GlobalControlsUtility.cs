using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001332 RID: 4914
	public static class GlobalControlsUtility
	{
		// Token: 0x060076DD RID: 30429 RVA: 0x0029BBD0 File Offset: 0x00299DD0
		public static void DoPlaySettings(WidgetRow rowVisibility, bool worldView, ref float curBaseY)
		{
			float y = curBaseY - TimeControls.TimeButSize.y;
			rowVisibility.Init((float)UI.screenWidth, y, UIDirection.LeftThenUp, 141f, 4f);
			Find.PlaySettings.DoPlaySettingsGlobalControls(rowVisibility, worldView);
			curBaseY = rowVisibility.FinalY;
		}

		// Token: 0x060076DE RID: 30430 RVA: 0x0029BC18 File Offset: 0x00299E18
		public static void DoTimespeedControls(float leftX, float width, ref float curBaseY)
		{
			leftX += Mathf.Max(0f, width - 150f);
			width = Mathf.Min(width, 150f);
			float y = TimeControls.TimeButSize.y;
			Rect timerRect = new Rect(leftX + 16f, curBaseY - y, width, y);
			TimeControls.DoTimeControlsGUI(timerRect);
			curBaseY -= timerRect.height;
		}

		// Token: 0x060076DF RID: 30431 RVA: 0x0029BC78 File Offset: 0x00299E78
		public static void DoDate(float leftX, float width, ref float curBaseY)
		{
			Rect dateRect = new Rect(leftX, curBaseY - DateReadout.Height, width, DateReadout.Height);
			DateReadout.DateOnGUI(dateRect);
			curBaseY -= dateRect.height;
		}

		// Token: 0x060076E0 RID: 30432 RVA: 0x0029BCB0 File Offset: 0x00299EB0
		public static void DoRealtimeClock(float leftX, float width, ref float curBaseY)
		{
			Rect rect = new Rect(leftX - 20f, curBaseY - 26f, width + 20f - 7f, 26f);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect, DateTime.Now.ToString("HH:mm"));
			Text.Anchor = TextAnchor.UpperLeft;
			curBaseY -= 26f;
		}

		// Token: 0x0400420E RID: 16910
		private const int VisibilityControlsPerRow = 5;
	}
}
