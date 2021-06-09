using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA4 RID: 7076
	[StaticConstructorOnStartup]
	public static class SkillUI
	{
		// Token: 0x06009BEC RID: 39916 RVA: 0x00067B25 File Offset: 0x00065D25
		public static void Reset()
		{
			SkillUI.skillDefsInListOrderCached = (from sd in DefDatabase<SkillDef>.AllDefs
			orderby sd.listOrder descending
			select sd).ToList<SkillDef>();
		}

		// Token: 0x06009BED RID: 39917 RVA: 0x002DB32C File Offset: 0x002D952C
		public static void DrawSkillsOf(Pawn p, Vector2 offset, SkillUI.SkillDrawMode mode)
		{
			Text.Font = GameFont.Small;
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				float x = Text.CalcSize(allDefsListForReading[i].skillLabel.CapitalizeFirst()).x;
				if (x > SkillUI.levelLabelWidth)
				{
					SkillUI.levelLabelWidth = x;
				}
			}
			for (int j = 0; j < SkillUI.skillDefsInListOrderCached.Count; j++)
			{
				SkillDef skillDef = SkillUI.skillDefsInListOrderCached[j];
				float y = (float)j * 27f + offset.y;
				SkillUI.DrawSkill(p.skills.GetSkill(skillDef), new Vector2(offset.x, y), mode, "");
			}
		}

		// Token: 0x06009BEE RID: 39918 RVA: 0x00067B5A File Offset: 0x00065D5A
		public static void DrawSkill(SkillRecord skill, Vector2 topLeft, SkillUI.SkillDrawMode mode, string tooltipPrefix = "")
		{
			SkillUI.DrawSkill(skill, new Rect(topLeft.x, topLeft.y, 230f, 24f), mode, "");
		}

		// Token: 0x06009BEF RID: 39919 RVA: 0x002DB3D8 File Offset: 0x002D95D8
		public static void DrawSkill(SkillRecord skill, Rect holdingRect, SkillUI.SkillDrawMode mode, string tooltipPrefix = "")
		{
			if (Mouse.IsOver(holdingRect))
			{
				GUI.DrawTexture(holdingRect, TexUI.HighlightTex);
			}
			GUI.BeginGroup(holdingRect);
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect = new Rect(6f, 0f, SkillUI.levelLabelWidth + 6f, holdingRect.height);
			Widgets.Label(rect, skill.def.skillLabel.CapitalizeFirst());
			Rect position = new Rect(rect.xMax, 0f, 24f, 24f);
			if (!skill.TotallyDisabled)
			{
				if (skill.passion > Passion.None)
				{
					Texture2D image = (skill.passion == Passion.Major) ? SkillUI.PassionMajorIcon : SkillUI.PassionMinorIcon;
					GUI.DrawTexture(position, image);
				}
				Rect rect2 = new Rect(position.xMax, 0f, holdingRect.width - position.xMax, holdingRect.height);
				float fillPercent = Mathf.Max(0.01f, (float)skill.Level / 20f);
				Widgets.FillableBar(rect2, fillPercent, SkillUI.SkillBarFillTex, null, false);
			}
			Rect rect3 = new Rect(position.xMax + 4f, 0f, 999f, holdingRect.height);
			rect3.yMin += 3f;
			string label;
			if (skill.TotallyDisabled)
			{
				GUI.color = SkillUI.DisabledSkillColor;
				label = "-";
			}
			else
			{
				label = skill.Level.ToStringCached();
			}
			GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
			Widgets.Label(rect3, label);
			GenUI.ResetLabelAlign();
			GUI.color = Color.white;
			GUI.EndGroup();
			if (Mouse.IsOver(holdingRect))
			{
				string text = SkillUI.GetSkillDescription(skill);
				if (tooltipPrefix != "")
				{
					text = tooltipPrefix + "\n\n" + text;
				}
				TooltipHandler.TipRegion(holdingRect, new TipSignal(text, skill.def.GetHashCode() * 397945));
			}
		}

		// Token: 0x06009BF0 RID: 39920 RVA: 0x002DB5A4 File Offset: 0x002D97A4
		private static string GetSkillDescription(SkillRecord sk)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (sk.TotallyDisabled)
			{
				stringBuilder.Append("DisabledLower".Translate().CapitalizeFirst());
			}
			else
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"Level".Translate() + " ",
					sk.Level,
					": ",
					sk.LevelDescriptor
				}));
				if (Current.ProgramState == ProgramState.Playing)
				{
					string text = (sk.Level == 20) ? "Experience".Translate() : "ProgressToNextLevel".Translate();
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						text,
						": ",
						sk.xpSinceLastLevel.ToString("F0"),
						" / ",
						sk.XpRequiredForLevelUp
					}));
				}
				stringBuilder.Append("Passion".Translate() + ": ");
				switch (sk.passion)
				{
				case Passion.None:
					stringBuilder.Append("PassionNone".Translate(0.35f.ToStringPercent("F0")));
					break;
				case Passion.Minor:
					stringBuilder.Append("PassionMinor".Translate(1f.ToStringPercent("F0")));
					break;
				case Passion.Major:
					stringBuilder.Append("PassionMajor".Translate(1.5f.ToStringPercent("F0")));
					break;
				}
				if (sk.LearningSaturatedToday)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append("LearnedMaxToday".Translate(sk.xpSinceMidnight.ToString("F0"), 4000, 0.2f.ToStringPercent("F0")));
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append(sk.def.description);
			return stringBuilder.ToString();
		}

		// Token: 0x0400633C RID: 25404
		private static float levelLabelWidth = -1f;

		// Token: 0x0400633D RID: 25405
		private static List<SkillDef> skillDefsInListOrderCached;

		// Token: 0x0400633E RID: 25406
		private const float SkillWidth = 230f;

		// Token: 0x0400633F RID: 25407
		public const float SkillHeight = 24f;

		// Token: 0x04006340 RID: 25408
		public const float SkillYSpacing = 3f;

		// Token: 0x04006341 RID: 25409
		private const float LeftEdgeMargin = 6f;

		// Token: 0x04006342 RID: 25410
		private const float IncButX = 205f;

		// Token: 0x04006343 RID: 25411
		private const float IncButSpacing = 10f;

		// Token: 0x04006344 RID: 25412
		private static readonly Color DisabledSkillColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x04006345 RID: 25413
		private static Texture2D PassionMinorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMinor", true);

		// Token: 0x04006346 RID: 25414
		private static Texture2D PassionMajorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMajor", true);

		// Token: 0x04006347 RID: 25415
		private static Texture2D SkillBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

		// Token: 0x02001BA5 RID: 7077
		public enum SkillDrawMode : byte
		{
			// Token: 0x04006349 RID: 25417
			Gameplay,
			// Token: 0x0400634A RID: 25418
			Menu
		}
	}
}
