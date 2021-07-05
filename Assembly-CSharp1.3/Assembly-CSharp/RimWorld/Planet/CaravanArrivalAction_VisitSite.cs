using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001799 RID: 6041
	public class CaravanArrivalAction_VisitSite : CaravanArrivalAction
	{
		// Token: 0x170016C4 RID: 5828
		// (get) Token: 0x06008BAB RID: 35755 RVA: 0x00322075 File Offset: 0x00320275
		public override string Label
		{
			get
			{
				return this.site.ApproachOrderString;
			}
		}

		// Token: 0x170016C5 RID: 5829
		// (get) Token: 0x06008BAC RID: 35756 RVA: 0x00322082 File Offset: 0x00320282
		public override string ReportString
		{
			get
			{
				return this.site.ApproachingReportString;
			}
		}

		// Token: 0x06008BAD RID: 35757 RVA: 0x0032126E File Offset: 0x0031F46E
		public CaravanArrivalAction_VisitSite()
		{
		}

		// Token: 0x06008BAE RID: 35758 RVA: 0x0032208F File Offset: 0x0032028F
		public CaravanArrivalAction_VisitSite(Site site)
		{
			this.site = site;
		}

		// Token: 0x06008BAF RID: 35759 RVA: 0x003220A0 File Offset: 0x003202A0
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

		// Token: 0x06008BB0 RID: 35760 RVA: 0x003220EC File Offset: 0x003202EC
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

		// Token: 0x06008BB1 RID: 35761 RVA: 0x00322148 File Offset: 0x00320348
		private void DoEnter(Caravan caravan, Site site)
		{
			LookTargets lookTargets = new LookTargets(caravan.PawnsListForReading);
			bool draftColonists = site.Faction == null || site.Faction.HostileTo(Faction.OfPlayer);
			bool flag = !site.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(site.Tile, site.PreferredMapSize, null);
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
				SettlementUtility.AffectRelationsOnAttacked(site, ref taggedString);
				if (!taggedString.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						if (stringBuilder[stringBuilder.Length - 1] != '\n')
						{
							stringBuilder.AppendLine();
						}
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

		// Token: 0x06008BB2 RID: 35762 RVA: 0x003223D4 File Offset: 0x003205D4
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

		// Token: 0x06008BB3 RID: 35763 RVA: 0x003224E5 File Offset: 0x003206E5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
		}

		// Token: 0x06008BB4 RID: 35764 RVA: 0x00322500 File Offset: 0x00320700
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

		// Token: 0x06008BB5 RID: 35765 RVA: 0x00322558 File Offset: 0x00320758
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Site site)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSite>(() => CaravanArrivalAction_VisitSite.CanVisit(caravan, site), () => new CaravanArrivalAction_VisitSite(site), site.ApproachOrderString, caravan, site.Tile, site, null);
		}

		// Token: 0x040058E1 RID: 22753
		private Site site;

		// Token: 0x040058E2 RID: 22754
		private static List<SitePartDef> tmpDefs = new List<SitePartDef>();

		// Token: 0x040058E3 RID: 22755
		private static List<SitePartDef> tmpUsedDefs = new List<SitePartDef>();
	}
}
