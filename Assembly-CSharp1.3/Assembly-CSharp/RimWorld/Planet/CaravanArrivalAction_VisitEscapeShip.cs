using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001796 RID: 6038
	public class CaravanArrivalAction_VisitEscapeShip : CaravanArrivalAction
	{
		// Token: 0x170016BE RID: 5822
		// (get) Token: 0x06008B8F RID: 35727 RVA: 0x00321A69 File Offset: 0x0031FC69
		public override string Label
		{
			get
			{
				return "VisitEscapeShip".Translate(this.target.Label);
			}
		}

		// Token: 0x170016BF RID: 5823
		// (get) Token: 0x06008B90 RID: 35728 RVA: 0x00321A8A File Offset: 0x0031FC8A
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.target.Label);
			}
		}

		// Token: 0x06008B91 RID: 35729 RVA: 0x0032126E File Offset: 0x0031F46E
		public CaravanArrivalAction_VisitEscapeShip()
		{
		}

		// Token: 0x06008B92 RID: 35730 RVA: 0x00321AAB File Offset: 0x0031FCAB
		public CaravanArrivalAction_VisitEscapeShip(EscapeShipComp escapeShip)
		{
			this.target = (MapParent)escapeShip.parent;
		}

		// Token: 0x06008B93 RID: 35731 RVA: 0x00321AC4 File Offset: 0x0031FCC4
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.target != null && this.target.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitEscapeShip.CanVisit(caravan, this.target);
		}

		// Token: 0x06008B94 RID: 35732 RVA: 0x00321B10 File Offset: 0x0031FD10
		public override void Arrived(Caravan caravan)
		{
			if (!this.target.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					this.DoArrivalAction(caravan);
				}, "GeneratingMapForNewEncounter", false, null, true);
				return;
			}
			this.DoArrivalAction(caravan);
		}

		// Token: 0x06008B95 RID: 35733 RVA: 0x00321B64 File Offset: 0x0031FD64
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.target, "target", false);
		}

		// Token: 0x06008B96 RID: 35734 RVA: 0x00321B80 File Offset: 0x0031FD80
		private void DoArrivalAction(Caravan caravan)
		{
			bool flag = !this.target.HasMap;
			if (flag)
			{
				this.target.SetFaction(Faction.OfPlayer);
			}
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.target.Tile, null);
			LookTargets lookTargets = new LookTargets(caravan.PawnsListForReading);
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.UnloadIndividually, false, null);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				Find.LetterStack.ReceiveLetter("EscapeShipFoundLabel".Translate(), (!Find.Storyteller.difficulty.allowBigThreats) ? "EscapeShipFoundPeaceful".Translate() : "EscapeShipFound".Translate(), LetterDefOf.PositiveEvent, new GlobalTargetInfo(this.target.Map.Center, this.target.Map, false), null, null, null, null);
				return;
			}
			Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(this.target), "LetterCaravanEnteredMap".Translate(caravan.Label, this.target).CapitalizeFirst(), LetterDefOf.NeutralEvent, lookTargets, null, null, null, null);
		}

		// Token: 0x06008B97 RID: 35735 RVA: 0x00321CA0 File Offset: 0x0031FEA0
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, MapParent escapeShip)
		{
			if (escapeShip == null || !escapeShip.Spawned || escapeShip.GetComponent<EscapeShipComp>() == null)
			{
				return false;
			}
			if (escapeShip.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(escapeShip.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true)));
			}
			return true;
		}

		// Token: 0x06008B98 RID: 35736 RVA: 0x00321D00 File Offset: 0x0031FF00
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent escapeShip)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitEscapeShip>(() => CaravanArrivalAction_VisitEscapeShip.CanVisit(caravan, escapeShip), () => new CaravanArrivalAction_VisitEscapeShip(escapeShip.GetComponent<EscapeShipComp>()), "VisitEscapeShip".Translate(escapeShip.Label), caravan, escapeShip.Tile, escapeShip, null);
		}

		// Token: 0x040058DE RID: 22750
		private MapParent target;
	}
}
