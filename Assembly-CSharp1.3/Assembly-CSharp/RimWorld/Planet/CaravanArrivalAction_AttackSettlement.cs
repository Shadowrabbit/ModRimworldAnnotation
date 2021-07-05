using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001792 RID: 6034
	public class CaravanArrivalAction_AttackSettlement : CaravanArrivalAction
	{
		// Token: 0x170016B6 RID: 5814
		// (get) Token: 0x06008B69 RID: 35689 RVA: 0x0032122C File Offset: 0x0031F42C
		public override string Label
		{
			get
			{
				return "AttackSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x170016B7 RID: 5815
		// (get) Token: 0x06008B6A RID: 35690 RVA: 0x0032124D File Offset: 0x0031F44D
		public override string ReportString
		{
			get
			{
				return "CaravanAttacking".Translate(this.settlement.Label);
			}
		}

		// Token: 0x06008B6B RID: 35691 RVA: 0x0032126E File Offset: 0x0031F46E
		public CaravanArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x06008B6C RID: 35692 RVA: 0x00321276 File Offset: 0x0031F476
		public CaravanArrivalAction_AttackSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06008B6D RID: 35693 RVA: 0x00321288 File Offset: 0x0031F488
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

		// Token: 0x06008B6E RID: 35694 RVA: 0x003212D1 File Offset: 0x0031F4D1
		public override void Arrived(Caravan caravan)
		{
			SettlementUtility.Attack(caravan, this.settlement);
		}

		// Token: 0x06008B6F RID: 35695 RVA: 0x003212DF File Offset: 0x0031F4DF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06008B70 RID: 35696 RVA: 0x003212F8 File Offset: 0x0031F4F8
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

		// Token: 0x06008B71 RID: 35697 RVA: 0x00321358 File Offset: 0x0031F558
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

		// Token: 0x040058DA RID: 22746
		private Settlement settlement;
	}
}
