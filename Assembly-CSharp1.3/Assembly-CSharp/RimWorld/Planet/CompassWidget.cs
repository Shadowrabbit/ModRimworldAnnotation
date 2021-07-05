using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001806 RID: 6150
	[StaticConstructorOnStartup]
	public static class CompassWidget
	{
		// Token: 0x17001790 RID: 6032
		// (get) Token: 0x06008FCD RID: 36813 RVA: 0x00337C18 File Offset: 0x00335E18
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

		// Token: 0x06008FCE RID: 36814 RVA: 0x00337C6C File Offset: 0x00335E6C
		public static void CompassOnGUI(ref float curBaseY)
		{
			CompassWidget.CompassOnGUI(new Vector2((float)UI.screenWidth - 10f - 32f, curBaseY - 10f - 32f));
			curBaseY -= 84f;
		}

		// Token: 0x06008FCF RID: 36815 RVA: 0x00337CA4 File Offset: 0x00335EA4
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

		// Token: 0x04005A55 RID: 23125
		private const float Padding = 10f;

		// Token: 0x04005A56 RID: 23126
		private const float Size = 64f;

		// Token: 0x04005A57 RID: 23127
		private static readonly Texture2D CompassTex = ContentFinder<Texture2D>.Get("UI/Misc/Compass", true);
	}
}
