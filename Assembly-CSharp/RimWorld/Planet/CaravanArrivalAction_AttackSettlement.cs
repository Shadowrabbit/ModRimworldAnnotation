using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020B6 RID: 8374
	public class CaravanArrivalAction_AttackSettlement : CaravanArrivalAction
	{
		// Token: 0x17001A34 RID: 6708
		// (get) Token: 0x0600B18D RID: 45453 RVA: 0x000735C0 File Offset: 0x000717C0
		public override string Label
		{
			get
			{
				return "AttackSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x17001A35 RID: 6709
		// (get) Token: 0x0600B18E RID: 45454 RVA: 0x000735E1 File Offset: 0x000717E1
		public override string ReportString
		{
			get
			{
				return "CaravanAttacking".Translate(this.settlement.Label);
			}
		}

		// Token: 0x0600B18F RID: 45455 RVA: 0x00073602 File Offset: 0x00071802
		public CaravanArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x0600B190 RID: 45456 RVA: 0x0007360A File Offset: 0x0007180A
		public CaravanArrivalAction_AttackSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x0600B191 RID: 45457 RVA: 0x00338714 File Offset: 0x00336914
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
			return CaravanArrivalAction_AttackSettlement.CanAttack(caravan, this.settlement);
		}

		// Token: 0x0600B192 RID: 45458 RVA: 0x00073619 File Offset: 0x00071819
		public override void Arrived(Caravan caravan)
		{
			SettlementUtility.Attack(caravan, this.settlement);
		}

		// Token: 0x0600B193 RID: 45459 RVA: 0x00073627 File Offset: 0x00071827
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x0600B194 RID: 45460 RVA: 0x00338760 File Offset: 0x00336960
		public static FloatMenuAcceptanceReport CanAttack(Caravan caravan, Settlement settlement)
		{
			if (settlement == null || !settlement.Spawned || !settlement.Attackable)
			{
				return false;
			}
			if (settlement.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(settlement.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true)));
			}
			return true;
		}

		// Token: 0x0600B195 RID: 45461 RVA: 0x003387C0 File Offset: 0x003369C0
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_AttackSettlement>(() => CaravanArrivalAction_AttackSettlement.CanAttack(caravan, settlement), () => new CaravanArrivalAction_AttackSettlement(settlement), "AttackSettlement".Translate(settlement.Label), caravan, settlement.Tile, settlement, settlement.Faction.AllyOrNeutralTo(Faction.OfPlayer) ? delegate(Action action)
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmAttackFriendlyFaction".Translate(settlement.LabelCap, settlement.Faction.Name), delegate
				{
					action();
				}, false, null));
			} : null);
		}

		// Token: 0x04007A54 RID: 31316
		private Settlement settlement;
	}
}
