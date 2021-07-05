using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001356 RID: 4950
	[StaticConstructorOnStartup]
	public class InspectPaneFiller
	{
		// Token: 0x060077DF RID: 30687 RVA: 0x002A36B4 File Offset: 0x002A18B4
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
				}), 754672);
			}
			finally
			{
				GUI.EndGroup();
			}
		}

		// Token: 0x060077E0 RID: 30688 RVA: 0x002A37AC File Offset: 0x002A19AC
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

		// Token: 0x060077E1 RID: 30689 RVA: 0x002A38AC File Offset: 0x002A1AAC
		private static void DrawMood(WidgetRow row, Pawn pawn)
		{
			if (pawn.needs == null || pawn.needs.mood == null)
			{
				return;
			}
			row.Gap(6f);
			row.FillableBar(93f, 16f, pawn.needs.mood.CurLevelPercentage, pawn.needs.mood.MoodString.CapitalizeFirst(), InspectPaneFiller.MoodTex, InspectPaneFiller.BarBGTex);
		}

		// Token: 0x060077E2 RID: 30690 RVA: 0x002A391C File Offset: 0x002A1B1C
		private static void DrawTimetableSetting(WidgetRow row, Pawn pawn)
		{
			row.Gap(6f);
			row.FillableBar(93f, 16f, 1f, pawn.timetable.CurrentAssignment.LabelCap, pawn.timetable.CurrentAssignment.ColorTexture, null);
		}

		// Token: 0x060077E3 RID: 30691 RVA: 0x002A3970 File Offset: 0x002A1B70
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
				Widgets.DrawBox(rect.ContractedBy(-1f), 1, null);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				AreaUtility.MakeAllowedAreaListFloatMenu(delegate(Area a)
				{
					pawn.playerSettings.AreaRestriction = a;
				}, true, true, pawn.Map);
			}
		}

		// Token: 0x060077E4 RID: 30692 RVA: 0x002A3A78 File Offset: 0x002A1C78
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
					Log.Error(text);
					InspectPaneFiller.debug_inspectStringExceptionErrored = true;
				}
			}
			if (!text.NullOrEmpty() && GenText.ContainsEmptyLines(text))
			{
				Log.ErrorOnce(string.Format("Inspect string for {0} contains empty lines.\n\nSTART\n{1}\nEND", sel, text), 837163521);
			}
			InspectPaneFiller.DrawInspectString(text, rect);
		}

		// Token: 0x060077E5 RID: 30693 RVA: 0x002A3B38 File Offset: 0x002A1D38
		public static void DrawInspectString(string str, Rect rect)
		{
			Text.Font = GameFont.Small;
			Widgets.LabelScrollable(rect, str, ref InspectPaneFiller.inspectStringScrollPos, true, true, false);
		}

		// Token: 0x0400429F RID: 17055
		private const float BarHeight = 16f;

		// Token: 0x040042A0 RID: 17056
		private static readonly Texture2D MoodTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(26, 52, 52).ToColor);

		// Token: 0x040042A1 RID: 17057
		private static readonly Texture2D BarBGTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(10, 10, 10).ToColor);

		// Token: 0x040042A2 RID: 17058
		private static readonly Texture2D HealthTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(35, 35, 35).ToColor);

		// Token: 0x040042A3 RID: 17059
		private const float BarWidth = 93f;

		// Token: 0x040042A4 RID: 17060
		private const float BarSpacing = 6f;

		// Token: 0x040042A5 RID: 17061
		private static bool debug_inspectStringExceptionErrored = false;

		// Token: 0x040042A6 RID: 17062
		private static Vector2 inspectStringScrollPos;
	}
}
