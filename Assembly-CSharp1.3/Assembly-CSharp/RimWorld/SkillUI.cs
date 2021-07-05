using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013AA RID: 5034
	[StaticConstructorOnStartup]
	public static class SkillUI
	{
		// Token: 0x06007A8A RID: 31370 RVA: 0x002B3C8C File Offset: 0x002B1E8C
		public static void Reset()
		{
			SkillUI.skillDefsInListOrderCached = (from sd in DefDatabase<SkillDef>.AllDefs
			orderby sd.listOrder descending
			select sd).ToList<SkillDef>();
		}

		// Token: 0x06007A8B RID: 31371 RVA: 0x002B3CC4 File Offset: 0x002B1EC4
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

		// Token: 0x06007A8C RID: 31372 RVA: 0x002B3D70 File Offset: 0x002B1F70
		public static void DrawSkill(SkillRecord skill, Vector2 topLeft, SkillUI.SkillDrawMode mode, string tooltipPrefix = "")
		{
			SkillUI.DrawSkill(skill, new Rect(topLeft.x, topLeft.y, 230f, 24f), mode, "");
		}

		// Token: 0x06007A8D RID: 31373 RVA: 0x002B3D9C File Offset: 0x002B1F9C
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

		// Token: 0x06007A8E RID: 31374 RVA: 0x002B3F68 File Offset: 0x002B2168
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
					"Level".Translate().CapitalizeFirst() + " ",
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

		// Token: 0x040043C6 RID: 17350
		private static float levelLabelWidth = -1f;

		// Token: 0x040043C7 RID: 17351
		private static List<SkillDef> skillDefsInListOrderCached;

		// Token: 0x040043C8 RID: 17352
		private const float SkillWidth = 230f;

		// Token: 0x040043C9 RID: 17353
		public const float SkillHeight = 24f;

		// Token: 0x040043CA RID: 17354
		public const float SkillYSpacing = 3f;

		// Token: 0x040043CB RID: 17355
		private const float LeftEdgeMargin = 6f;

		// Token: 0x040043CC RID: 17356
		private const float IncButX = 205f;

		// Token: 0x040043CD RID: 17357
		private const float IncButSpacing = 10f;

		// Token: 0x040043CE RID: 17358
		private static readonly Color DisabledSkillColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x040043CF RID: 17359
		private static Texture2D PassionMinorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMinor", true);

		// Token: 0x040043D0 RID: 17360
		private static Texture2D PassionMajorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMajor", true);

		// Token: 0x040043D1 RID: 17361
		private static Texture2D SkillBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

		// Token: 0x020027B0 RID: 10160
		public enum SkillDrawMode : byte
		{
			// Token: 0x0400961B RID: 38427
			Gameplay,
			// Token: 0x0400961C RID: 38428
			Menu
		}
	}
}
