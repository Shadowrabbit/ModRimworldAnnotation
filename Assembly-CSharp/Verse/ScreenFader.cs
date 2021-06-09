using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000890 RID: 2192
	[StaticConstructorOnStartup]
	public static class ScreenFader
	{
		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06003662 RID: 13922 RVA: 0x0002A3A1 File Offset: 0x000285A1
		private static float CurTime
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x0015BBE8 File Offset: 0x00159DE8
		static ScreenFader()
		{
			ScreenFader.fadeTexture = new Texture2D(1, 1);
			ScreenFader.fadeTexture.name = "ScreenFader";
			ScreenFader.backgroundStyle.normal.background = ScreenFader.fadeTexture;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x0015BC8C File Offset: 0x00159E8C
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

		// Token: 0x06003665 RID: 13925 RVA: 0x0015BD00 File Offset: 0x00159F00
		private static Color CurrentInstantColor()
		{
			if (ScreenFader.CurTime > ScreenFader.targetTime || ScreenFader.targetTime == ScreenFader.sourceTime)
			{
				return ScreenFader.targetColor;
			}
			return Color.Lerp(ScreenFader.sourceColor, ScreenFader.targetColor, (ScreenFader.CurTime - ScreenFader.sourceTime) / (ScreenFader.targetTime - ScreenFader.sourceTime));
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x0002A3A8 File Offset: 0x000285A8
		public static void SetColor(Color newColor)
		{
			ScreenFader.sourceColor = newColor;
			ScreenFader.targetColor = newColor;
			ScreenFader.targetTime = 0f;
			ScreenFader.sourceTime = 0f;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x0002A3D0 File Offset: 0x000285D0
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

		// Token: 0x040025E5 RID: 9701
		private static GUIStyle backgroundStyle = new GUIStyle();

		// Token: 0x040025E6 RID: 9702
		private static Texture2D fadeTexture;

		// Token: 0x040025E7 RID: 9703
		private static Color sourceColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x040025E8 RID: 9704
		private static Color targetColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x040025E9 RID: 9705
		private static float sourceTime = 0f;

		// Token: 0x040025EA RID: 9706
		private static float targetTime = 0f;

		// Token: 0x040025EB RID: 9707
		private static bool fadeTextureDirty = true;
	}
}
