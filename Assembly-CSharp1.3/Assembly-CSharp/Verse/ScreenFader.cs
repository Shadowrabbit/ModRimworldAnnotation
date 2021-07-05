using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E4 RID: 1252
	[StaticConstructorOnStartup]
	public static class ScreenFader
	{
		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060025D2 RID: 9682 RVA: 0x000EA71C File Offset: 0x000E891C
		private static float CurTime
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x000EA724 File Offset: 0x000E8924
		static ScreenFader()
		{
			ScreenFader.fadeTexture = new Texture2D(1, 1);
			ScreenFader.fadeTexture.name = "ScreenFader";
			ScreenFader.backgroundStyle.normal.background = ScreenFader.fadeTexture;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x000EA7C8 File Offset: 0x000E89C8
		public static void OverlayOnGUI(Vector2 windowSize)
		{
			Color color = ScreenFader.CurrentInstantColor();
			if (color.a > 0f)
			{
				if (ScreenFader.fadeTextureDirty)
				{
					ScreenFader.fadeTexture.SetPixel(0, 0, color);
					ScreenFader.fadeTexture.Apply();
				}
				GUI.Label(new Rect(-10f, -10f, windowSize.x + 10f, windowSize.y + 10f), ScreenFader.fadeTexture, ScreenFader.backgroundStyle);
			}
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x000EA83C File Offset: 0x000E8A3C
		private static Color CurrentInstantColor()
		{
			if (ScreenFader.CurTime > ScreenFader.targetTime || ScreenFader.targetTime == ScreenFader.sourceTime)
			{
				return ScreenFader.targetColor;
			}
			return Color.Lerp(ScreenFader.sourceColor, ScreenFader.targetColor, (ScreenFader.CurTime - ScreenFader.sourceTime) / (ScreenFader.targetTime - ScreenFader.sourceTime));
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x000EA88D File Offset: 0x000E8A8D
		public static void SetColor(Color newColor)
		{
			ScreenFader.sourceColor = newColor;
			ScreenFader.targetColor = newColor;
			ScreenFader.targetTime = 0f;
			ScreenFader.sourceTime = 0f;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x000EA8B5 File Offset: 0x000E8AB5
		public static void StartFade(Color finalColor, float duration)
		{
			if (duration <= 0f)
			{
				ScreenFader.SetColor(finalColor);
				return;
			}
			ScreenFader.sourceColor = ScreenFader.CurrentInstantColor();
			ScreenFader.targetColor = finalColor;
			ScreenFader.sourceTime = ScreenFader.CurTime;
			ScreenFader.targetTime = ScreenFader.CurTime + duration;
		}

		// Token: 0x040017A9 RID: 6057
		private static GUIStyle backgroundStyle = new GUIStyle();

		// Token: 0x040017AA RID: 6058
		private static Texture2D fadeTexture;

		// Token: 0x040017AB RID: 6059
		private static Color sourceColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x040017AC RID: 6060
		private static Color targetColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x040017AD RID: 6061
		private static float sourceTime = 0f;

		// Token: 0x040017AE RID: 6062
		private static float targetTime = 0f;

		// Token: 0x040017AF RID: 6063
		private static bool fadeTextureDirty = true;
	}
}
