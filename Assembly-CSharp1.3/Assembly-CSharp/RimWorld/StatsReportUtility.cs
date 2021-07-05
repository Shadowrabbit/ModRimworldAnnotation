using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001311 RID: 4881
	public static class StatsReportUtility
	{
		// Token: 0x1700148A RID: 5258
		// (get) Token: 0x0600757A RID: 30074 RVA: 0x00287B1B File Offset: 0x00285D1B
		public static int SelectedStatIndex
		{
			get
			{
				if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>() || StatsReportUtility.selectedEntry == null)
				{
					return -1;
				}
				return StatsReportUtility.cachedDrawEntries.IndexOf(StatsReportUtility.selectedEntry);
			}
		}

		// Token: 0x1700148B RID: 5259
		// (get) Token: 0x0600757B RID: 30075 RVA: 0x00287B41 File Offset: 0x00285D41
		public static QuickSearchWidget QuickSearchWidget
		{
			get
			{
				return StatsReportUtility.quickSearchWidget;
			}
		}

		// Token: 0x0600757C RID: 30076 RVA: 0x00287B48 File Offset: 0x00285D48
		public static void Reset()
		{
			StatsReportUtility.scrollPosition = default(Vector2);
			StatsReportUtility.scrollPositionRightPanel = default(Vector2);
			StatsReportUtility.selectedEntry = null;
			StatsReportUtility.scrollPositioner.Arm(false);
			StatsReportUtility.mousedOverEntry = null;
			StatsReportUtility.cachedDrawEntries.Clear();
			StatsReportUtility.quickSearchWidget.Reset();
			PermitsCardUtility.selectedPermit = null;
			PermitsCardUtility.selectedFaction = (ModLister.RoyaltyInstalled ? Faction.OfEmpire : null);
		}

		// Token: 0x0600757D RID: 30077 RVA: 0x00287BB0 File Offset: 0x00285DB0
		public static void DrawStatsReport(Rect rect, Def def, ThingDef stuff)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				BuildableDef buildableDef = def as BuildableDef;
				StatRequest req = (buildableDef != null) ? StatRequest.For(buildableDef, stuff, QualityCategory.Normal) : StatRequest.ForEmpty();
				StatsReportUtility.cachedDrawEntries.AddRange(def.SpecialDisplayStats(req));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(def, stuff)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x0600757E RID: 30078 RVA: 0x00287C3C File Offset: 0x00285E3C
		public static void DrawStatsReport(Rect rect, AbilityDef def)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatRequest req = StatRequest.ForEmpty();
				StatsReportUtility.cachedDrawEntries.AddRange(def.SpecialDisplayStats(req));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(def)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x0600757F RID: 30079 RVA: 0x00287CB4 File Offset: 0x00285EB4
		public static void DrawStatsReport(Rect rect, Thing thing)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(thing.def.SpecialDisplayStats(StatRequest.For(thing)));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(thing)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.cachedDrawEntries.RemoveAll((StatDrawEntry de) => de.stat != null && !de.stat.showNonAbstract);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, thing, null);
		}

		// Token: 0x06007580 RID: 30080 RVA: 0x00287D5C File Offset: 0x00285F5C
		public static void DrawStatsReport(Rect rect, WorldObject worldObject)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(worldObject.def.SpecialDisplayStats(StatRequest.ForEmpty()));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(worldObject)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.cachedDrawEntries.RemoveAll((StatDrawEntry de) => de.stat != null && !de.stat.showNonAbstract);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, worldObject);
		}

		// Token: 0x06007581 RID: 30081 RVA: 0x00287E04 File Offset: 0x00286004
		public static void DrawStatsReport(Rect rect, RoyalTitleDef title, Faction faction, Pawn pawn = null)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(title.SpecialDisplayStats(StatRequest.For(title, faction, pawn)));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(title, faction)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x06007582 RID: 30082 RVA: 0x00287E7C File Offset: 0x0028607C
		public static void DrawStatsReport(Rect rect, Faction faction)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatRequest req = StatRequest.ForEmpty();
				StatsReportUtility.cachedDrawEntries.AddRange(faction.def.SpecialDisplayStats(req));
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(faction)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x06007583 RID: 30083 RVA: 0x00287EF7 File Offset: 0x002860F7
		private static IEnumerable<StatDrawEntry> StatsToDraw(Def def, ThingDef stuff)
		{
			yield return StatsReportUtility.DescriptionEntry(def);
			BuildableDef eDef = def as BuildableDef;
			if (eDef != null)
			{
				StatRequest statRequest = StatRequest.For(eDef, stuff, QualityCategory.Normal);
				IEnumerable<StatDef> allDefs = DefDatabase<StatDef>.AllDefs;
				Func<StatDef, bool> predicate;
				Func<StatDef, bool> <>9__0;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((StatDef st) => st.Worker.ShouldShowFor(statRequest)));
				}
				foreach (StatDef statDef in allDefs.Where(predicate))
				{
					yield return new StatDrawEntry(statDef.category, statDef, eDef.GetStatValueAbstract(statDef, stuff), StatRequest.For(eDef, stuff, QualityCategory.Normal), ToStringNumberSense.Undefined, null, false);
				}
				IEnumerator<StatDef> enumerator = null;
			}
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null && thingDef.IsStuff)
			{
				foreach (StatDrawEntry statDrawEntry in StatsReportUtility.StuffStats(thingDef))
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06007584 RID: 30084 RVA: 0x00287F0E File Offset: 0x0028610E
		private static IEnumerable<StatDrawEntry> StatsToDraw(RoyalTitleDef title, Faction faction)
		{
			yield return StatsReportUtility.DescriptionEntry(title, faction);
			yield break;
		}

		// Token: 0x06007585 RID: 30085 RVA: 0x00287F25 File Offset: 0x00286125
		private static IEnumerable<StatDrawEntry> StatsToDraw(Faction faction)
		{
			yield return StatsReportUtility.DescriptionEntry(faction);
			yield break;
		}

		// Token: 0x06007586 RID: 30086 RVA: 0x00287F35 File Offset: 0x00286135
		private static IEnumerable<StatDrawEntry> StatsToDraw(AbilityDef def)
		{
			yield return StatsReportUtility.DescriptionEntry(def);
			StatRequest statRequest = StatRequest.For(def, null);
			IEnumerable<StatDef> allDefs = DefDatabase<StatDef>.AllDefs;
			Func<StatDef, bool> <>9__0;
			Func<StatDef, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((StatDef st) => st.Worker.ShouldShowFor(statRequest)));
			}
			foreach (StatDef statDef in allDefs.Where(predicate))
			{
				yield return new StatDrawEntry(statDef.category, statDef, def.GetStatValueAbstract(statDef, null), StatRequest.For(def, null), ToStringNumberSense.Undefined, null, false);
			}
			IEnumerator<StatDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06007587 RID: 30087 RVA: 0x00287F45 File Offset: 0x00286145
		private static IEnumerable<StatDrawEntry> StatsToDraw(Thing thing)
		{
			yield return StatsReportUtility.DescriptionEntry(thing);
			StatDrawEntry statDrawEntry = StatsReportUtility.QualityEntry(thing);
			if (statDrawEntry != null)
			{
				yield return statDrawEntry;
			}
			IEnumerable<StatDef> allDefs = DefDatabase<StatDef>.AllDefs;
			Func<StatDef, bool> <>9__0;
			Func<StatDef, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((StatDef st) => st.Worker.ShouldShowFor(StatRequest.For(thing))));
			}
			foreach (StatDef statDef in allDefs.Where(predicate))
			{
				if (!statDef.Worker.IsDisabledFor(thing))
				{
					float statValue = thing.GetStatValue(statDef, true);
					if (statDef.showOnDefaultValue || statValue != statDef.defaultBaseValue)
					{
						yield return new StatDrawEntry(statDef.category, statDef, statValue, StatRequest.For(thing), ToStringNumberSense.Undefined, null, false);
					}
				}
				else
				{
					yield return new StatDrawEntry(statDef.category, statDef);
				}
			}
			IEnumerator<StatDef> enumerator = null;
			if (thing.def.useHitPoints)
			{
				StatDrawEntry statDrawEntry2 = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "HitPointsBasic".Translate().CapitalizeFirst(), thing.HitPoints.ToString() + " / " + thing.MaxHitPoints.ToString(), "Stat_HitPoints_Desc".Translate(), 99998, null, null, false);
				yield return statDrawEntry2;
			}
			foreach (StatDrawEntry statDrawEntry3 in thing.SpecialDisplayStats())
			{
				yield return statDrawEntry3;
			}
			IEnumerator<StatDrawEntry> enumerator2 = null;
			if (thing.def.IsStuff)
			{
				foreach (StatDrawEntry statDrawEntry4 in StatsReportUtility.StuffStats(thing.def))
				{
					yield return statDrawEntry4;
				}
				enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06007588 RID: 30088 RVA: 0x00287F55 File Offset: 0x00286155
		private static IEnumerable<StatDrawEntry> StatsToDraw(WorldObject worldObject)
		{
			yield return StatsReportUtility.DescriptionEntry(worldObject);
			foreach (StatDrawEntry statDrawEntry in worldObject.SpecialDisplayStats)
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06007589 RID: 30089 RVA: 0x00287F65 File Offset: 0x00286165
		private static IEnumerable<StatDrawEntry> StuffStats(ThingDef stuffDef)
		{
			if (!stuffDef.stuffProps.statFactors.NullOrEmpty<StatModifier>())
			{
				int num;
				for (int i = 0; i < stuffDef.stuffProps.statFactors.Count; i = num + 1)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.StuffStatFactors, stuffDef.stuffProps.statFactors[i].stat, stuffDef.stuffProps.statFactors[i].value, StatRequest.ForEmpty(), ToStringNumberSense.Factor, null, false);
					num = i;
				}
			}
			if (!stuffDef.stuffProps.statOffsets.NullOrEmpty<StatModifier>())
			{
				int num;
				for (int i = 0; i < stuffDef.stuffProps.statOffsets.Count; i = num + 1)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.StuffStatOffsets, stuffDef.stuffProps.statOffsets[i].stat, stuffDef.stuffProps.statOffsets[i].value, StatRequest.ForEmpty(), ToStringNumberSense.Offset, null, false);
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x0600758A RID: 30090 RVA: 0x00287F78 File Offset: 0x00286178
		private static void FinalizeCachedDrawEntries(IEnumerable<StatDrawEntry> original)
		{
			StatsReportUtility.cachedDrawEntries = (from sd in original
			orderby sd.category.displayOrder, sd.DisplayPriorityWithinCategory descending, sd.LabelCap
			select sd).ToList<StatDrawEntry>();
			StatsReportUtility.quickSearchWidget.noResultsMatched = !StatsReportUtility.cachedDrawEntries.Any<StatDrawEntry>();
			if (StatsReportUtility.selectedEntry != null)
			{
				StatsReportUtility.selectedEntry = StatsReportUtility.cachedDrawEntries.FirstOrDefault((StatDrawEntry e) => e.Same(StatsReportUtility.selectedEntry));
			}
			if (StatsReportUtility.quickSearchWidget.filter.Active)
			{
				foreach (StatDrawEntry sd2 in StatsReportUtility.cachedDrawEntries)
				{
					if (StatsReportUtility.Matches(sd2))
					{
						StatsReportUtility.selectedEntry = sd2;
						StatsReportUtility.scrollPositioner.Arm(true);
						break;
					}
				}
			}
		}

		// Token: 0x0600758B RID: 30091 RVA: 0x002880B4 File Offset: 0x002862B4
		private static bool Matches(StatDrawEntry sd)
		{
			return StatsReportUtility.quickSearchWidget.filter.Matches(sd.LabelCap);
		}

		// Token: 0x0600758C RID: 30092 RVA: 0x002880CB File Offset: 0x002862CB
		private static StatDrawEntry DescriptionEntry(Def def)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", def.description, 99999, null, Dialog_InfoCard.DefsToHyperlinks(def.descriptionHyperlinks), false);
		}

		// Token: 0x0600758D RID: 30093 RVA: 0x00288103 File Offset: 0x00286303
		private static StatDrawEntry DescriptionEntry(Faction faction)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", faction.GetReportText, 99999, null, Dialog_InfoCard.DefsToHyperlinks(faction.def.descriptionHyperlinks), false);
		}

		// Token: 0x0600758E RID: 30094 RVA: 0x00288140 File Offset: 0x00286340
		private static StatDrawEntry DescriptionEntry(RoyalTitleDef title, Faction faction)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", title.GetReportText(faction), 99999, null, Dialog_InfoCard.TitleDefsToHyperlinks(title.GetHyperlinks(faction)), false);
		}

		// Token: 0x0600758F RID: 30095 RVA: 0x0028817A File Offset: 0x0028637A
		private static StatDrawEntry DescriptionEntry(Thing thing)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", thing.DescriptionFlavor, 99999, null, Dialog_InfoCard.DefsToHyperlinks(thing.def.descriptionHyperlinks), false);
		}

		// Token: 0x06007590 RID: 30096 RVA: 0x002881B7 File Offset: 0x002863B7
		private static StatDrawEntry DescriptionEntry(WorldObject worldObject)
		{
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Description".Translate(), "", worldObject.GetDescription(), 99999, null, null, false);
		}

		// Token: 0x06007591 RID: 30097 RVA: 0x002881E8 File Offset: 0x002863E8
		private static StatDrawEntry QualityEntry(Thing t)
		{
			QualityCategory cat;
			if (!t.TryGetQuality(out cat))
			{
				return null;
			}
			return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Quality".Translate(), cat.GetLabel().CapitalizeFirst(), "QualityDescription".Translate(), 99999, null, null, false);
		}

		// Token: 0x06007592 RID: 30098 RVA: 0x0028823C File Offset: 0x0028643C
		public static void SelectEntry(int index)
		{
			if (index < 0 || index > StatsReportUtility.cachedDrawEntries.Count)
			{
				return;
			}
			StatsReportUtility.SelectEntry(StatsReportUtility.cachedDrawEntries[index], true);
		}

		// Token: 0x06007593 RID: 30099 RVA: 0x00288264 File Offset: 0x00286464
		public static void SelectEntry(StatDef stat, bool playSound = false)
		{
			foreach (StatDrawEntry statDrawEntry in StatsReportUtility.cachedDrawEntries)
			{
				if (statDrawEntry.stat == stat)
				{
					StatsReportUtility.SelectEntry(statDrawEntry, playSound);
					return;
				}
			}
			Messages.Message("MessageCannotSelectInvisibleStat".Translate(stat), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x06007594 RID: 30100 RVA: 0x002882E4 File Offset: 0x002864E4
		private static void SelectEntry(StatDrawEntry rec, bool playSound = true)
		{
			StatsReportUtility.selectedEntry = rec;
			StatsReportUtility.scrollPositioner.Arm(true);
			if (playSound)
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06007595 RID: 30101 RVA: 0x00288308 File Offset: 0x00286508
		private static void DrawStatsWorker(Rect rect, Thing optionalThing, WorldObject optionalWorldObject)
		{
			Rect rect2 = new Rect(rect);
			rect2.width *= 0.5f;
			Rect rect3 = new Rect(rect);
			rect3.x = rect2.xMax;
			rect3.width = rect.xMax - rect3.x;
			StatsReportUtility.scrollPositioner.ClearInterestRects();
			Text.Font = GameFont.Small;
			Rect viewRect = new Rect(0f, 0f, rect2.width - 16f, StatsReportUtility.listHeight);
			Widgets.BeginScrollView(rect2, ref StatsReportUtility.scrollPosition, viewRect, true);
			float num = 0f;
			string b = null;
			StatsReportUtility.mousedOverEntry = null;
			for (int i = 0; i < StatsReportUtility.cachedDrawEntries.Count; i++)
			{
				StatDrawEntry ent = StatsReportUtility.cachedDrawEntries[i];
				if (ent.category.LabelCap != b)
				{
					Widgets.ListSeparator(ref num, viewRect.width, ent.category.LabelCap);
					b = ent.category.LabelCap;
				}
				bool highlightLabel = false;
				bool lowlightLabel = false;
				bool flag = StatsReportUtility.selectedEntry == ent;
				bool flag2 = false;
				GUI.color = Color.white;
				if (StatsReportUtility.quickSearchWidget.filter.Active)
				{
					if (StatsReportUtility.Matches(ent))
					{
						highlightLabel = true;
						flag2 = true;
					}
					else
					{
						lowlightLabel = true;
					}
				}
				Rect rect4 = new Rect(8f, num, viewRect.width - 8f, 30f);
				num += ent.Draw(rect4.x, rect4.y, rect4.width, flag, highlightLabel, lowlightLabel, delegate
				{
					StatsReportUtility.SelectEntry(ent, true);
				}, delegate
				{
					StatsReportUtility.mousedOverEntry = ent;
				}, StatsReportUtility.scrollPosition, rect2);
				rect4.yMax = num;
				if (flag || flag2)
				{
					StatsReportUtility.scrollPositioner.RegisterInterestRect(rect4);
				}
			}
			StatsReportUtility.listHeight = num + 100f;
			Widgets.EndScrollView();
			StatsReportUtility.scrollPositioner.ScrollVertically(ref StatsReportUtility.scrollPosition, rect2.size);
			Rect outRect = rect3.ContractedBy(10f);
			StatDrawEntry statDrawEntry;
			if ((statDrawEntry = StatsReportUtility.selectedEntry) == null)
			{
				statDrawEntry = (StatsReportUtility.mousedOverEntry ?? StatsReportUtility.cachedDrawEntries.FirstOrDefault<StatDrawEntry>());
			}
			StatDrawEntry statDrawEntry2 = statDrawEntry;
			if (statDrawEntry2 != null)
			{
				Rect rect5 = new Rect(0f, 0f, outRect.width - 16f, StatsReportUtility.rightPanelHeight);
				StatRequest statRequest;
				if (statDrawEntry2.hasOptionalReq)
				{
					statRequest = statDrawEntry2.optionalReq;
				}
				else if (optionalThing != null)
				{
					statRequest = StatRequest.For(optionalThing);
				}
				else
				{
					statRequest = StatRequest.ForEmpty();
				}
				string explanationText = statDrawEntry2.GetExplanationText(statRequest);
				float num2 = 0f;
				Widgets.BeginScrollView(outRect, ref StatsReportUtility.scrollPositionRightPanel, rect5, true);
				Rect rect6 = rect5;
				rect6.width -= 4f;
				Widgets.Label(rect6, explanationText);
				float num3 = Text.CalcHeight(explanationText, rect6.width) + 10f;
				num2 += num3;
				IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks = statDrawEntry2.GetHyperlinks(statRequest);
				if (hyperlinks != null)
				{
					Rect rect7 = new Rect(rect6.x, rect6.y + num3, rect6.width, rect6.height - num3);
					Color color = GUI.color;
					GUI.color = Widgets.NormalOptionColor;
					foreach (Dialog_InfoCard.Hyperlink hyperlink in hyperlinks)
					{
						float num4 = Text.CalcHeight(hyperlink.Label, rect7.width);
						Widgets.HyperlinkWithIcon(new Rect(rect7.x, rect7.y, rect7.width, num4), hyperlink, "ViewHyperlink".Translate(hyperlink.Label), 2f, 6f, null, false, null);
						rect7.y += num4;
						rect7.height -= num4;
						num2 += num4;
					}
					GUI.color = color;
				}
				StatsReportUtility.rightPanelHeight = num2;
				Widgets.EndScrollView();
			}
		}

		// Token: 0x06007596 RID: 30102 RVA: 0x00288748 File Offset: 0x00286948
		public static void Notify_QuickSearchChanged()
		{
			StatsReportUtility.cachedDrawEntries.Clear();
		}

		// Token: 0x04004110 RID: 16656
		private static StatDrawEntry selectedEntry;

		// Token: 0x04004111 RID: 16657
		private static StatDrawEntry mousedOverEntry;

		// Token: 0x04004112 RID: 16658
		private static Vector2 scrollPosition;

		// Token: 0x04004113 RID: 16659
		private static ScrollPositioner scrollPositioner = new ScrollPositioner();

		// Token: 0x04004114 RID: 16660
		private static Vector2 scrollPositionRightPanel;

		// Token: 0x04004115 RID: 16661
		private static QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x04004116 RID: 16662
		private static float listHeight;

		// Token: 0x04004117 RID: 16663
		private static float rightPanelHeight;

		// Token: 0x04004118 RID: 16664
		private static List<StatDrawEntry> cachedDrawEntries = new List<StatDrawEntry>();
	}
}
