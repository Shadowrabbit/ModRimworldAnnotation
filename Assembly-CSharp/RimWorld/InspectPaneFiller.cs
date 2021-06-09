using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AEE RID: 6894
	[StaticConstructorOnStartup]
	public class InspectPaneFiller
	{
		// Token: 0x060097E4 RID: 38884 RVA: 0x002C901C File Offset: 0x002C721C
		public static void DoPaneContentsFor(ISelectable sel, Rect rect)
		{
			try
			{
				GUI.BeginGroup(rect);
				float num = 0f;
				Thing thing = sel as Thing;
				Pawn pawn = sel as Pawn;
				if (thing != null)
				{
					num += 3f;
					WidgetRow row = new WidgetRow(0f, num, UIDirection.RightThenUp, 99999f, 4f);
					InspectPaneFiller.DrawHealth(row, thing);
					if (pawn != null)
					{
						InspectPaneFiller.DrawMood(row, pawn);
						if (pawn.timetable != null)
						{
							InspectPaneFiller.DrawTimetableSetting(row, pawn);
						}
						InspectPaneFiller.DrawAreaAllowed(row, pawn);
					}
					num += 18f;
				}
				Rect rect2 = rect.AtZero();
				rect2.yMin = num;
				InspectPaneFiller.DrawInspectStringFor(sel, rect2);
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Error in DoPaneContentsFor ",
					Find.Selector.FirstSelectedObject,
					": ",
					ex.ToString()
				}), 754672, false);
			}
			finally
			{
				GUI.EndGroup();
			}
		}

		// Token: 0x060097E5 RID: 38885 RVA: 0x002C9114 File Offset: 0x002C7314
		public static void DrawHealth(WidgetRow row, Thing t)
		{
			Pawn pawn = t as Pawn;
			float fillPct;
			string label;
			if (pawn == null)
			{
				if (!t.def.useHitPoints)
				{
					return;
				}
				if (t.HitPoints >= t.MaxHitPoints)
				{
					GUI.color = Color.white;
				}
				else if ((float)t.HitPoints > (float)t.MaxHitPoints * 0.5f)
				{
					GUI.color = Color.yellow;
				}
				else if (t.HitPoints > 0)
				{
					GUI.color = Color.red;
				}
				else
				{
					GUI.color = Color.grey;
				}
				fillPct = (float)t.HitPoints / (float)t.MaxHitPoints;
				label = t.HitPoints.ToStringCached() + " / " + t.MaxHitPoints.ToStringCached();
			}
			else
			{
				GUI.color = Color.white;
				fillPct = pawn.health.summaryHealth.SummaryHealthPercent;
				label = HealthUtility.GetGeneralConditionLabel(pawn, true);
			}
			row.FillableBar(93f, 16f, fillPct, label, InspectPaneFiller.HealthTex, InspectPaneFiller.BarBGTex);
			GUI.color = Color.white;
		}

		// Token: 0x060097E6 RID: 38886 RVA: 0x002C9214 File Offset: 0x002C7414
		private static void DrawMood(WidgetRow row, Pawn pawn)
		{
			if (pawn.needs == null || pawn.needs.mood == null)
			{
				return;
			}
			row.Gap(6f);
			row.FillableBar(93f, 16f, pawn.needs.mood.CurLevelPercentage, pawn.needs.mood.MoodString.CapitalizeFirst(), InspectPaneFiller.MoodTex, InspectPaneFiller.BarBGTex);
		}

		// Token: 0x060097E7 RID: 38887 RVA: 0x002C9284 File Offset: 0x002C7484
		private static void DrawTimetableSetting(WidgetRow row, Pawn pawn)
		{
			row.Gap(6f);
			row.FillableBar(93f, 16f, 1f, pawn.timetable.CurrentAssignment.LabelCap, pawn.timetable.CurrentAssignment.ColorTexture, null);
		}

		// Token: 0x060097E8 RID: 38888 RVA: 0x002C92D8 File Offset: 0x002C74D8
		private static void DrawAreaAllowed(WidgetRow row, Pawn pawn)
		{
			if (pawn.playerSettings == null || !pawn.playerSettings.RespectsAllowedArea)
			{
				return;
			}
			row.Gap(6f);
			bool flag = pawn.playerSettings != null && pawn.playerSettings.EffectiveAreaRestriction != null;
			Texture2D fillTex;
			if (flag)
			{
				fillTex = pawn.playerSettings.EffectiveAreaRestriction.ColorTexture;
			}
			else
			{
				fillTex = BaseContent.GreyTex;
			}
			Rect rect = row.FillableBar(93f, 16f, 1f, AreaUtility.AreaAllowedLabel(pawn), fillTex, null);
			if (Mouse.IsOver(rect))
			{
				if (flag)
				{
					pawn.playerSettings.EffectiveAreaRestriction.MarkForDraw();
				}
				Widgets.DrawBox(rect.ContractedBy(-1f), 1);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				AreaUtility.MakeAllowedAreaListFloatMenu(delegate(Area a)
				{
					pawn.playerSettings.AreaRestriction = a;
				}, true, true, pawn.Map);
			}
		}

		// Token: 0x060097E9 RID: 38889 RVA: 0x002C93E0 File Offset: 0x002C75E0
		public static void DrawInspectStringFor(ISelectable sel, Rect rect)
		{
			string text;
			try
			{
				text = sel.GetInspectString();
				Thing thing = sel as Thing;
				if (thing != null)
				{
					string inspectStringLowPriority = thing.GetInspectStringLowPriority();
					if (!inspectStringLowPriority.NullOrEmpty())
					{
						if (!text.NullOrEmpty())
						{
							text = text.TrimEndNewlines() + "\n";
						}
						text += inspectStringLowPriority;
					}
				}
			}
			catch (Exception ex)
			{
				text = "GetInspectString exception on " + sel.ToString() + ":\n" + ex.ToString();
				if (!InspectPaneFiller.debug_inspectStringExceptionErrored)
				{
					Log.Error(text, false);
					InspectPaneFiller.debug_inspectStringExceptionErrored = true;
				}
			}
			if (!text.NullOrEmpty() && GenText.ContainsEmptyLines(text))
			{
				Log.ErrorOnce(string.Format("Inspect string for {0} contains empty lines.\n\nSTART\n{1}\nEND", sel, text), 837163521, false);
			}
			InspectPaneFiller.DrawInspectString(text, rect);
		}

		// Token: 0x060097EA RID: 38890 RVA: 0x00065251 File Offset: 0x00063451
		public static void DrawInspectString(string str, Rect rect)
		{
			Text.Font = GameFont.Small;
			Widgets.LabelScrollable(rect, str, ref InspectPaneFiller.inspectStringScrollPos, true, true, false);
		}

		// Token: 0x040060FF RID: 24831
		private const float BarHeight = 16f;

		// Token: 0x04006100 RID: 24832
		private static readonly Texture2D MoodTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(26, 52, 52).ToColor);

		// Token: 0x04006101 RID: 24833
		private static readonly Texture2D BarBGTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(10, 10, 10).ToColor);

		// Token: 0x04006102 RID: 24834
		private static readonly Texture2D HealthTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(35, 35, 35).ToColor);

		// Token: 0x04006103 RID: 24835
		private const float BarWidth = 93f;

		// Token: 0x04006104 RID: 24836
		private const float BarSpacing = 6f;

		// Token: 0x04006105 RID: 24837
		private static bool debug_inspectStringExceptionErrored = false;

		// Token: 0x04006106 RID: 24838
		private static Vector2 inspectStringScrollPos;
	}
}
