using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017EA RID: 6122
	public class TransportPodsArrivalAction_VisitSite : TransportPodsArrivalAction
	{
		// Token: 0x06008E86 RID: 36486 RVA: 0x00331E93 File Offset: 0x00330093
		public TransportPodsArrivalAction_VisitSite()
		{
		}

		// Token: 0x06008E87 RID: 36487 RVA: 0x00332CB0 File Offset: 0x00330EB0
		public TransportPodsArrivalAction_VisitSite(Site site, PawnsArrivalModeDef arrivalMode)
		{
			this.site = site;
			this.arrivalMode = arrivalMode;
		}

		// Token: 0x06008E88 RID: 36488 RVA: 0x00332CC6 File Offset: 0x00330EC6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		// Token: 0x06008E89 RID: 36489 RVA: 0x00332CF0 File Offset: 0x00330EF0
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.site != null && this.site.Tile != destinationTile)
			{
				return false;
			}
			return TransportPodsArrivalAction_VisitSite.CanVisit(pods, this.site);
		}

		// Token: 0x06008E8A RID: 36490 RVA: 0x00332D39 File Offset: 0x00330F39
		public override bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return !this.site.HasMap;
		}

		// Token: 0x06008E8B RID: 36491 RVA: 0x00332D4C File Offset: 0x00330F4C
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			bool flag = !this.site.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.site.Tile, this.site.PreferredMapSize, null);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(orGenerateMap.mapPawns.AllPawns, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
			}
			if (this.site.Faction != null && this.site.Faction != Faction.OfPlayer)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this.site.Faction, Faction.OfPlayer.GoodwillToMakeHostile(this.site.Faction), true, true, HistoryEventDefOf.AttackedSettlement, null);
			}
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
			this.arrivalMode.Worker.TravelingTransportPodsArrived(pods, orGenerateMap);
		}

		// Token: 0x06008E8C RID: 36492 RVA: 0x00332E60 File Offset: 0x00331060
		public static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, Site site)
		{
			if (site == null || !site.Spawned)
			{
				return false;
			}
			if (!TransportPodsArrivalActionUtility.AnyNonDownedColonist(pods))
			{
				return false;
			}
			if (site.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(site.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true)));
			}
			return true;
		}

		// Token: 0x06008E8D RID: 36493 RVA: 0x00332EC5 File Offset: 0x003310C5
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Site site)
		{
			Func<FloatMenuAcceptanceReport> <>9__0;
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter;
			if ((acceptanceReportGetter = <>9__0) == null)
			{
				acceptanceReportGetter = (<>9__0 = (() => TransportPodsArrivalAction_VisitSite.CanVisit(pods, site)));
			}
			Func<TransportPodsArrivalAction_VisitSite> <>9__1;
			Func<TransportPodsArrivalAction_VisitSite> arrivalActionGetter;
			if ((arrivalActionGetter = <>9__1) == null)
			{
				arrivalActionGetter = (<>9__1 = (() => new TransportPodsArrivalAction_VisitSite(site, PawnsArrivalModeDefOf.EdgeDrop)));
			}
			foreach (FloatMenuOption floatMenuOption in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSite>(acceptanceReportGetter, arrivalActionGetter, "DropAtEdge".Translate(), representative, site.Tile, null))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			Func<FloatMenuAcceptanceReport> <>9__2;
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter2;
			if ((acceptanceReportGetter2 = <>9__2) == null)
			{
				acceptanceReportGetter2 = (<>9__2 = (() => TransportPodsArrivalAction_VisitSite.CanVisit(pods, site)));
			}
			Func<TransportPodsArrivalAction_VisitSite> <>9__3;
			Func<TransportPodsArrivalAction_VisitSite> arrivalActionGetter2;
			if ((arrivalActionGetter2 = <>9__3) == null)
			{
				arrivalActionGetter2 = (<>9__3 = (() => new TransportPodsArrivalAction_VisitSite(site, PawnsArrivalModeDefOf.CenterDrop)));
			}
			foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSite>(acceptanceReportGetter2, arrivalActionGetter2, "DropInCenter".Translate(), representative, site.Tile, null))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040059E8 RID: 23016
		private Site site;

		// Token: 0x040059E9 RID: 23017
		private PawnsArrivalModeDef arrivalMode;
	}
}
