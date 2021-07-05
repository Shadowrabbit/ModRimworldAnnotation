using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000257 RID: 599
	[StaticConstructorOnStartup]
	public static class GenMapUI
	{
		// Token: 0x0600110E RID: 4366 RVA: 0x00060A04 File Offset: 0x0005EC04
		public static Vector2 LabelDrawPosFor(Thing thing, float worldOffsetZ)
		{
			Vector3 drawPos = thing.DrawPos;
			drawPos.z += worldOffsetZ;
			Vector2 vector = Find.Camera.WorldToScreenPoint(drawPos) / Prefs.UIScale;
			vector.y = (float)UI.screenHeight - vector.y;
			return vector;
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00060A54 File Offset: 0x0005EC54
		public static Vector2 LabelDrawPosFor(IntVec3 center)
		{
			Vector3 position = center.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Vector2 vector = Find.Camera.WorldToScreenPoint(position) / Prefs.UIScale;
			vector.y = (float)UI.screenHeight - vector.y;
			vector.y -= 1f;
			return vector;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00060AAB File Offset: 0x0005ECAB
		public static void DrawThingLabel(Thing thing, string text)
		{
			GenMapUI.DrawThingLabel(thing, text, GenMapUI.DefaultThingLabelColor);
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00060AB9 File Offset: 0x0005ECB9
		public static void DrawThingLabel(Thing thing, string text, Color textColor)
		{
			GenMapUI.DrawThingLabel(GenMapUI.LabelDrawPosFor(thing, -0.4f), text, textColor);
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00060AD0 File Offset: 0x0005ECD0
		public static void DrawThingLabel(Vector2 screenPos, string text, Color textColor)
		{
			Text.Font = GameFont.Tiny;
			float x = Text.CalcSize(text).x;
			GUI.DrawTexture(new Rect(screenPos.x - x / 2f - 4f, screenPos.y, x + 8f, 12f), TexUI.GrayTextBG);
			GUI.color = textColor;
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(new Rect(screenPos.x - x / 2f, screenPos.y - 3f, x, 999f), text);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00060B74 File Offset: 0x0005ED74
		public static void DrawPawnLabel(Pawn pawn, Vector2 pos, float alpha = 1f, float truncateToWidth = 9999f, Dictionary<string, string> truncatedLabelsCache = null, GameFont font = GameFont.Tiny, bool alwaysDrawBg = true, bool alignCenter = true)
		{
			float pawnLabelNameWidth = GenMapUI.GetPawnLabelNameWidth(pawn, truncateToWidth, truncatedLabelsCache, font);
			Rect bgRect = new Rect(pos.x - pawnLabelNameWidth / 2f - 4f, pos.y, pawnLabelNameWidth + 8f, 12f);
			if (!pawn.RaceProps.Humanlike)
			{
				bgRect.y -= 4f;
			}
			GenMapUI.DrawPawnLabel(pawn, bgRect, alpha, truncateToWidth, truncatedLabelsCache, font, alwaysDrawBg, alignCenter);
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00060BEC File Offset: 0x0005EDEC
		public static void DrawPawnLabel(Pawn pawn, Rect bgRect, float alpha = 1f, float truncateToWidth = 9999f, Dictionary<string, string> truncatedLabelsCache = null, GameFont font = GameFont.Tiny, bool alwaysDrawBg = true, bool alignCenter = true)
		{
			GUI.color = new Color(1f, 1f, 1f, alpha);
			Text.Font = font;
			string pawnLabel = GenMapUI.GetPawnLabel(pawn, truncateToWidth, truncatedLabelsCache, font);
			float pawnLabelNameWidth = GenMapUI.GetPawnLabelNameWidth(pawn, truncateToWidth, truncatedLabelsCache, font);
			float summaryHealthPercent = pawn.health.summaryHealth.SummaryHealthPercent;
			if (alwaysDrawBg || summaryHealthPercent < 0.999f)
			{
				GUI.DrawTexture(bgRect, TexUI.GrayTextBG);
			}
			if (summaryHealthPercent < 0.999f)
			{
				Widgets.FillableBar(bgRect.ContractedBy(1f), summaryHealthPercent, GenMapUI.OverlayHealthTex, BaseContent.ClearTex, false);
			}
			Color color = PawnNameColorUtility.PawnNameColorOf(pawn);
			color.a = alpha;
			GUI.color = color;
			Rect rect;
			if (alignCenter)
			{
				Text.Anchor = TextAnchor.UpperCenter;
				rect = new Rect(bgRect.center.x - pawnLabelNameWidth / 2f, bgRect.y - 2f, pawnLabelNameWidth, 100f);
			}
			else
			{
				Text.Anchor = TextAnchor.UpperLeft;
				rect = new Rect(bgRect.x + 2f, bgRect.center.y - Text.CalcSize(pawnLabel).y / 2f, pawnLabelNameWidth, 100f);
			}
			Widgets.Label(rect, pawnLabel);
			if (pawn.Drafted)
			{
				Widgets.DrawLineHorizontal(bgRect.center.x - pawnLabelNameWidth / 2f, bgRect.y + 11f, pawnLabelNameWidth);
			}
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00060D54 File Offset: 0x0005EF54
		public static void DrawText(Vector2 worldPos, string text, Color textColor)
		{
			Vector3 position = new Vector3(worldPos.x, 0f, worldPos.y);
			Vector2 vector = Find.Camera.WorldToScreenPoint(position) / Prefs.UIScale;
			vector.y = (float)UI.screenHeight - vector.y;
			Text.Font = GameFont.Tiny;
			GUI.color = textColor;
			Text.Anchor = TextAnchor.UpperCenter;
			float x = Text.CalcSize(text).x;
			Widgets.Label(new Rect(vector.x - x / 2f, vector.y - 2f, x, 999f), text);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00060E04 File Offset: 0x0005F004
		private static float GetPawnLabelNameWidth(Pawn pawn, float truncateToWidth, Dictionary<string, string> truncatedLabelsCache, GameFont font)
		{
			GameFont font2 = Text.Font;
			Text.Font = font;
			string pawnLabel = GenMapUI.GetPawnLabel(pawn, truncateToWidth, truncatedLabelsCache, font);
			float num;
			if (font == GameFont.Tiny)
			{
				num = pawnLabel.GetWidthCached();
			}
			else
			{
				num = Text.CalcSize(pawnLabel).x;
			}
			if (Math.Abs(Math.Round((double)Prefs.UIScale) - (double)Prefs.UIScale) > 1.401298464324817E-45)
			{
				num += 0.5f;
			}
			if (num < 20f)
			{
				num = 20f;
			}
			Text.Font = font2;
			return num;
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00060E7C File Offset: 0x0005F07C
		private static string GetPawnLabel(Pawn pawn, float truncateToWidth, Dictionary<string, string> truncatedLabelsCache, GameFont font)
		{
			GameFont font2 = Text.Font;
			Text.Font = font;
			string result = pawn.LabelShortCap.Truncate(truncateToWidth, truncatedLabelsCache);
			Text.Font = font2;
			return result;
		}

		// Token: 0x04000CF1 RID: 3313
		public static readonly Texture2D OverlayHealthTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0f, 0f, 0.25f));

		// Token: 0x04000CF2 RID: 3314
		public static readonly Texture2D OverlayEntropyTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.55f, 0.84f, 0.5f));

		// Token: 0x04000CF3 RID: 3315
		public const float NameBGHeight_Tiny = 12f;

		// Token: 0x04000CF4 RID: 3316
		public const float NameBGExtraWidth_Tiny = 4f;

		// Token: 0x04000CF5 RID: 3317
		public const float NameBGHeight_Small = 16f;

		// Token: 0x04000CF6 RID: 3318
		public const float NameBGExtraWidth_Small = 6f;

		// Token: 0x04000CF7 RID: 3319
		public const float LabelOffsetYStandard = -0.4f;

		// Token: 0x04000CF8 RID: 3320
		public const float PsychicEntropyBarHeight = 4f;

		// Token: 0x04000CF9 RID: 3321
		public static readonly Color DefaultThingLabelColor = new Color(1f, 1f, 1f, 0.75f);
	}
}
