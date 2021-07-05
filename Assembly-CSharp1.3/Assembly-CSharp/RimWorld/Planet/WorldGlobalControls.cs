using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001817 RID: 6167
	public class WorldGlobalControls
	{
		// Token: 0x06009076 RID: 36982 RVA: 0x0033D604 File Offset: 0x0033B804
		public void WorldGlobalControlsOnGUI()
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			float leftX = (float)UI.screenWidth - 200f;
			float num = (float)UI.screenHeight - 4f;
			if (Current.ProgramState == ProgramState.Playing)
			{
				num -= 35f;
			}
			GlobalControlsUtility.DoPlaySettings(this.rowVisibility, true, ref num);
			if (Current.ProgramState == ProgramState.Playing)
			{
				num -= 4f;
				GlobalControlsUtility.DoTimespeedControls(leftX, 200f, ref num);
				if (Find.CurrentMap != null || Find.WorldSelector.AnyObjectOrTileSelected)
				{
					num -= 4f;
					GlobalControlsUtility.DoDate(leftX, 200f, ref num);
				}
				float num2 = 154f;
				float num3 = Find.World.gameConditionManager.TotalHeightAt(num2);
				Rect rect = new Rect((float)UI.screenWidth - num2, num - num3, num2, num3);
				Find.World.gameConditionManager.DoConditionsUI(rect);
				num -= rect.height;
			}
			if (Prefs.ShowRealtimeClock)
			{
				GlobalControlsUtility.DoRealtimeClock(leftX, 200f, ref num);
			}
			if (!Find.WorldTargeter.IsTargeting)
			{
				Find.WorldRoutePlanner.DoRoutePlannerButton(ref num);
			}
			if (!Find.PlaySettings.lockNorthUp)
			{
				CompassWidget.CompassOnGUI(ref num);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				num -= 10f;
				Find.LetterStack.LettersOnGUI(num);
			}
		}

		// Token: 0x04005AE2 RID: 23266
		public const float Width = 200f;

		// Token: 0x04005AE3 RID: 23267
		private const int VisibilityControlsPerRow = 5;

		// Token: 0x04005AE4 RID: 23268
		private WidgetRow rowVisibility = new WidgetRow();
	}
}
