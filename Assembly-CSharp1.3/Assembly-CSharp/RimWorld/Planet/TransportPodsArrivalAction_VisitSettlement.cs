using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017E8 RID: 6120
	public class TransportPodsArrivalAction_VisitSettlement : TransportPodsArrivalAction_FormCaravan
	{
		// Token: 0x06008E7B RID: 36475 RVA: 0x00332969 File Offset: 0x00330B69
		public TransportPodsArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x06008E7C RID: 36476 RVA: 0x00332971 File Offset: 0x00330B71
		public TransportPodsArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06008E7D RID: 36477 RVA: 0x00332980 File Offset: 0x00330B80
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06008E7E RID: 36478 RVA: 0x0033299C File Offset: 0x00330B9C
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return TransportPodsArrivalAction_VisitSettlement.CanVisit(pods, this.settlement);
		}

		// Token: 0x06008E7F RID: 36479 RVA: 0x003329E5 File Offset: 0x00330BE5
		public static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (settlement == null || !settlement.Spawned || !settlement.Visitable)
			{
				return false;
			}
			if (!TransportPodsArrivalActionUtility.AnyPotentialCaravanOwner(pods, Faction.OfPlayer))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06008E80 RID: 36480 RVA: 0x00332A1C File Offset: 0x00330C1C
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSettlement>(() => TransportPodsArrivalAction_VisitSettlement.CanVisit(pods, settlement), () => new TransportPodsArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(settlement.Label), representative, settlement.Tile, null);
		}

		// Token: 0x040059E7 RID: 23015
		protected Settlement settlement;
	}
}
