using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021BF RID: 8639
	[StaticConstructorOnStartup]
	public static class CompassWidget
	{
		// Token: 0x17001B70 RID: 7024
		// (get) Token: 0x0600B8E9 RID: 47337 RVA: 0x00352724 File Offset: 0x00350924
		private static float Angle
		{
			get
			{
				Vector2 vector = GenWorldUI.WorldToUIPosition(Find.WorldGrid.NorthPolePos);
				Vector2 a = new Vector2((float)UI.screenWidth / 2f, (float)UI.screenHeight / 2f);
				vector.y = (float)UI.screenHeight - vector.y;
				return a.AngleTo(vector);
			}
		}

		// Token: 0x0600B8EA RID: 47338 RVA: 0x00077D27 File Offset: 0x00075F27
		public static void CompassOnGUI(ref float curBaseY)
		{
			CompassWidget.CompassOnGUI(new Vector2((float)UI.screenWidth - 10f - 32f, curBaseY - 10f - 32f));
			curBaseY -= 84f;
		}

		// Token: 0x0600B8EB RID: 47339 RVA: 0x00352778 File Offset: 0x00350978
		private static void CompassOnGUI(Vector2 center)
		{
			Widgets.DrawTextureRotated(center, CompassWidget.CompassTex, CompassWidget.Angle, 1f);
			Rect rect = new Rect(center.x - 32f, center.y - 32f, 64f, 64f);
			TooltipHandler.TipRegionByKey(rect, "CompassTip");
			if (Widgets.ButtonInvisible(rect, true))
			{
				Find.WorldCameraDriver.RotateSoNorthIsUp(true);
			}
		}

		// Token: 0x04007E3A RID: 32314
		private const float Padding = 10f;

		// Token: 0x04007E3B RID: 32315
		private const float Size = 64f;

		// Token: 0x04007E3C RID: 32316
		private static readonly Texture2D CompassTex = ContentFinder<Texture2D>.Get("UI/Misc/Compass", true);
	}
}
