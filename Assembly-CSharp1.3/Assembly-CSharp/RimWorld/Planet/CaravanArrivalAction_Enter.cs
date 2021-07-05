using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001793 RID: 6035
	public class CaravanArrivalAction_Enter : CaravanArrivalAction
	{
		// Token: 0x170016B8 RID: 5816
		// (get) Token: 0x06008B72 RID: 35698 RVA: 0x003213F2 File Offset: 0x0031F5F2
		public override string Label
		{
			get
			{
				return "EnterMap".Translate(this.mapParent.Label);
			}
		}

		// Token: 0x170016B9 RID: 5817
		// (get) Token: 0x06008B73 RID: 35699 RVA: 0x00321413 File Offset: 0x0031F613
		public override string ReportString
		{
			get
			{
				return "CaravanEntering".Translate(this.mapParent.Label);
			}
		}

		// Token: 0x06008B74 RID: 35700 RVA: 0x0032126E File Offset: 0x0031F46E
		public CaravanArrivalAction_Enter()
		{
		}

		// Token: 0x06008B75 RID: 35701 RVA: 0x00321434 File Offset: 0x0031F634
		public CaravanArrivalAction_Enter(MapParent mapParent)
		{
			this.mapParent = mapParent;
		}

		// Token: 0x06008B76 RID: 35702 RVA: 0x00321444 File Offset: 0x0031F644
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.mapParent != null && this.mapParent.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_Enter.CanEnter(caravan, this.mapParent);
		}

		// Token: 0x06008B77 RID: 35703 RVA: 0x00321490 File Offset: 0x0031F690
		public override void Arrived(Caravan caravan)
		{
			Map map = this.mapParent.Map;
			if (map == null)
			{
				return;
			}
			CaravanDropInventoryMode dropInventoryMode = map.IsPlayerHome ? CaravanDropInventoryMode.UnloadIndividually : CaravanDropInventoryMode.DoNotDrop;
			bool draftColonists = this.mapParent.Faction != null && this.mapParent.Faction.HostileTo(Faction.OfPlayer);
			if (caravan.IsPlayerControlled || this.mapParent.Faction == Faction.OfPlayer)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(this.mapParent), "LetterCaravanEnteredMap".Translate(caravan.Label, this.mapParent).CapitalizeFirst(), LetterDefOf.NeutralEvent, caravan.PawnsListForReading, null, null, null, null);
			}
			CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Edge, dropInventoryMode, draftColonists, null);
		}

		// Token: 0x06008B78 RID: 35704 RVA: 0x00321560 File Offset: 0x0031F760
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x06008B79 RID: 35705 RVA: 0x0032157C File Offset: 0x0031F77C
		public static FloatMenuAcceptanceReport CanEnter(Caravan caravan, MapParent mapParent)
		{
			if (mapParent == null || !mapParent.Spawned || !mapParent.HasMap)
			{
				return false;
			}
			if (mapParent.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(mapParent.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true)));
			}
			return true;
		}

		// Token: 0x06008B7A RID: 35706 RVA: 0x003215DC File Offset: 0x0031F7DC
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent mapParent)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_Enter>(() => CaravanArrivalAction_Enter.CanEnter(caravan, mapParent), () => new CaravanArrivalAction_Enter(mapParent), "EnterMap".Translate(mapParent.Label), caravan, mapParent.Tile, mapParent, null);
		}

		// Token: 0x040058DB RID: 22747
		private MapParent mapParent;
	}
}
