using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020C6 RID: 8390
	public class CaravanArrivalAction_VisitSite : CaravanArrivalAction
	{
		// Token: 0x17001A42 RID: 6722
		// (get) Token: 0x0600B1E9 RID: 45545 RVA: 0x000739F7 File Offset: 0x00071BF7
		public override string Label
		{
			get
			{
				return this.site.ApproachOrderString;
			}
		}

		// Token: 0x17001A43 RID: 6723
		// (get) Token: 0x0600B1EA RID: 45546 RVA: 0x00073A04 File Offset: 0x00071C04
		public override string ReportString
		{
			get
			{
				return this.site.ApproachingReportString;
			}
		}

		// Token: 0x0600B1EB RID: 45547 RVA: 0x00073602 File Offset: 0x00071802
		public CaravanArrivalAction_VisitSite()
		{
		}

		// Token: 0x0600B1EC RID: 45548 RVA: 0x00073A11 File Offset: 0x00071C11
		public CaravanArrivalAction_VisitSite(Site site)
		{
			this.site = site;
		}

		// Token: 0x0600B1ED RID: 45549 RVA: 0x00339294 File Offset: 0x00337494
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.site != null && this.site.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitSite.CanVisit(caravan, this.site);
		}

		// Token: 0x0600B1EE RID: 45550 RVA: 0x003392E0 File Offset: 0x003374E0
		public override void Arrived(Caravan caravan)
		{
			if (!this.site.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					this.DoEnter(caravan, this.site);
				}, "GeneratingMapForNewEncounter", false, null, true);
				return;
			}
			this.DoEnter(caravan, this.site);
		}

		// Token: 0x0600B1EF RID: 45551 RVA: 0x0033933C File Offset: 0x0033753C
		private void DoEnter(Caravan caravan, Site site)
		{
			LookTargets lookTargets = new LookTargets(caravan.PawnsListForReading);
			bool draftColonists = site.Faction == null || site.Faction.HostileTo(Faction.OfPlayer);
			bool flag = !site.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(site.Tile, CaravanArrivalAction_VisitSite.MapSize, null);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(orGenerateMap.mapPawns.AllPawns, "LetterRelatedPawnsSite".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("LetterCaravanEnteredMap".Translate(caravan.Label, site).CapitalizeFirst());
				LetterDef letterDef;
				LookTargets lookTargets2;
				this.AppendThreatInfo(stringBuilder, site, orGenerateMap, out letterDef, out lookTargets2);
				TaggedString taggedString = null;
				SettlementUtility.AffectRelationsOnAttacked_NewTmp(site, ref taggedString);
				if (!taggedString.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendLineTagged(taggedString);
				}
				List<HediffDef> list = null;
				foreach (SitePart sitePart in site.parts)
				{
					if (!sitePart.def.arrivedLetterHediffHyperlinks.NullOrEmpty<HediffDef>())
					{
						if (list == null)
						{
							list = new List<HediffDef>();
						}
						foreach (HediffDef item in sitePart.def.arrivedLetterHediffHyperlinks)
						{
							if (!list.Contains(item))
							{
								list.Add(item);
							}
						}
					}
				}
				ChoiceLetter choiceLetter = LetterMaker.MakeLetter("LetterLabelCaravanEnteredMap".Translate(site), stringBuilder.ToString(), letterDef ?? LetterDefOf.NeutralEvent, lookTargets2.IsValid() ? lookTargets2 : lookTargets, null, null, null);
				choiceLetter.hyperlinkHediffDefs = list;
				Find.LetterStack.ReceiveLetter(choiceLetter, null);
			}
			else
			{
				Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(site), "LetterCaravanEnteredMap".Translate(caravan.Label, site).CapitalizeFirst(), LetterDefOf.NeutralEvent, lookTargets, null, null, null, null);
			}
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.DoNotDrop, draftColonists, null);
		}

		// Token: 0x0600B1F0 RID: 45552 RVA: 0x003395AC File Offset: 0x003377AC
		private void AppendThreatInfo(StringBuilder sb, Site site, Map map, out LetterDef letterDef, out LookTargets allLookTargets)
		{
			allLookTargets = new LookTargets();
			CaravanArrivalAction_VisitSite.tmpUsedDefs.Clear();
			CaravanArrivalAction_VisitSite.tmpDefs.Clear();
			for (int i = 0; i < site.parts.Count; i++)
			{
				CaravanArrivalAction_VisitSite.tmpDefs.Add(site.parts[i].def);
			}
			letterDef = null;
			for (int j = 0; j < CaravanArrivalAction_VisitSite.tmpDefs.Count; j++)
			{
				LetterDef letterDef2;
				LookTargets lookTargets;
				string arrivedLetterPart = CaravanArrivalAction_VisitSite.tmpDefs[j].Worker.GetArrivedLetterPart(map, out letterDef2, out lookTargets);
				if (arrivedLetterPart != null)
				{
					if (!CaravanArrivalAction_VisitSite.tmpUsedDefs.Contains(CaravanArrivalAction_VisitSite.tmpDefs[j]))
					{
						CaravanArrivalAction_VisitSite.tmpUsedDefs.Add(CaravanArrivalAction_VisitSite.tmpDefs[j]);
						if (sb.Length > 0)
						{
							sb.AppendLine();
							sb.AppendLine();
						}
						sb.Append(arrivedLetterPart);
					}
					if (letterDef == null)
					{
						letterDef = letterDef2;
					}
					if (lookTargets.IsValid())
					{
						allLookTargets = new LookTargets(allLookTargets.targets.Concat(lookTargets.targets));
					}
				}
			}
		}

		// Token: 0x0600B1F1 RID: 45553 RVA: 0x00073A20 File Offset: 0x00071C20
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
		}

		// Token: 0x0600B1F2 RID: 45554 RVA: 0x003396C0 File Offset: 0x003378C0
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, Site site)
		{
			if (site == null || !site.Spawned)
			{
				return false;
			}
			if (site.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(site.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true)));
			}
			return true;
		}

		// Token: 0x0600B1F3 RID: 45555 RVA: 0x00339718 File Offset: 0x00337918
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Site site)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSite>(() => CaravanArrivalAction_VisitSite.CanVisit(caravan, site), () => new CaravanArrivalAction_VisitSite(site), site.ApproachOrderString, caravan, site.Tile, site, null);
		}

		// Token: 0x04007A6C RID: 31340
		private Site site;

		// Token: 0x04007A6D RID: 31341
		public static readonly IntVec3 MapSize = new IntVec3(120, 1, 120);

		// Token: 0x04007A6E RID: 31342
		private static List<SitePartDef> tmpDefs = new List<SitePartDef>();

		// Token: 0x04007A6F RID: 31343
		private static List<SitePartDef> tmpUsedDefs = new List<SitePartDef>();
	}
}
