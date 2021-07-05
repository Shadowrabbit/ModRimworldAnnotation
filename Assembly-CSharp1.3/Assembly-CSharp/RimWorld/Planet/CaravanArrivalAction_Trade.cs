using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001795 RID: 6037
	public class CaravanArrivalAction_Trade : CaravanArrivalAction
	{
		// Token: 0x170016BC RID: 5820
		// (get) Token: 0x06008B85 RID: 35717 RVA: 0x0032183D File Offset: 0x0031FA3D
		public override string Label
		{
			get
			{
				return "TradeWithSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x170016BD RID: 5821
		// (get) Token: 0x06008B86 RID: 35718 RVA: 0x0032185E File Offset: 0x0031FA5E
		public override string ReportString
		{
			get
			{
				return "CaravanTrading".Translate(this.settlement.Label);
			}
		}

		// Token: 0x06008B87 RID: 35719 RVA: 0x0032126E File Offset: 0x0031F46E
		public CaravanArrivalAction_Trade()
		{
		}

		// Token: 0x06008B88 RID: 35720 RVA: 0x0032187F File Offset: 0x0031FA7F
		public CaravanArrivalAction_Trade(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06008B89 RID: 35721 RVA: 0x00321890 File Offset: 0x0031FA90
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_Trade.CanTradeWith(caravan, this.settlement);
		}

		// Token: 0x06008B8A RID: 35722 RVA: 0x003218DC File Offset: 0x0031FADC
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, this.settlement.Faction, this.settlement.TraderKind);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, false));
		}

		// Token: 0x06008B8B RID: 35723 RVA: 0x00321928 File Offset: 0x0031FB28
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06008B8C RID: 35724 RVA: 0x00321944 File Offset: 0x0031FB44
		public static FloatMenuAcceptanceReport CanTradeWith(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_Trade.HasNegotiator(caravan, settlement);
		}

		// Token: 0x06008B8D RID: 35725 RVA: 0x003219B4 File Offset: 0x0031FBB4
		public static bool HasNegotiator(Caravan caravan, Settlement settlement)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, settlement.Faction, settlement.TraderKind);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		// Token: 0x06008B8E RID: 35726 RVA: 0x003219F4 File Offset: 0x0031FBF4
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_Trade>(() => CaravanArrivalAction_Trade.CanTradeWith(caravan, settlement), () => new CaravanArrivalAction_Trade(settlement), "TradeWith".Translate(settlement.Label), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x040058DD RID: 22749
		private Settlement settlement;
	}
}
