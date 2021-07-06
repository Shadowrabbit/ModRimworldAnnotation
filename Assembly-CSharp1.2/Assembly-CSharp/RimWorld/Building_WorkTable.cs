using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D7 RID: 5847
	public class Building_WorkTable : Building, IBillGiver, IBillGiverWithTickAction
	{
		// Token: 0x170013F7 RID: 5111
		// (get) Token: 0x06008071 RID: 32881 RVA: 0x00056338 File Offset: 0x00054538
		public bool CanWorkWithoutPower
		{
			get
			{
				return this.powerComp == null || this.def.building.unpoweredWorkTableWorkSpeedFactor > 0f;
			}
		}

		// Token: 0x170013F8 RID: 5112
		// (get) Token: 0x06008072 RID: 32882 RVA: 0x0005635E File Offset: 0x0005455E
		public bool CanWorkWithoutFuel
		{
			get
			{
				return this.refuelableComp == null;
			}
		}

		// Token: 0x06008073 RID: 32883 RVA: 0x00056369 File Offset: 0x00054569
		public Building_WorkTable()
		{
			this.billStack = new BillStack(this);
		}

		// Token: 0x06008074 RID: 32884 RVA: 0x0005637D File Offset: 0x0005457D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<BillStack>(ref this.billStack, "billStack", new object[]
			{
				this
			});
		}

		// Token: 0x06008075 RID: 32885 RVA: 0x0026059C File Offset: 0x0025E79C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.refuelableComp = base.GetComp<CompRefuelable>();
			this.breakdownableComp = base.GetComp<CompBreakdownable>();
			foreach (Bill bill in this.billStack)
			{
				bill.ValidateSettings();
			}
		}

		// Token: 0x06008076 RID: 32886 RVA: 0x0005639F File Offset: 0x0005459F
		public virtual void UsedThisTick()
		{
			if (this.refuelableComp != null)
			{
				this.refuelableComp.Notify_UsedThisTick();
			}
		}

		// Token: 0x170013F9 RID: 5113
		// (get) Token: 0x06008077 RID: 32887 RVA: 0x000563B4 File Offset: 0x000545B4
		public BillStack BillStack
		{
			get
			{
				return this.billStack;
			}
		}

		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x06008078 RID: 32888 RVA: 0x000563BC File Offset: 0x000545BC
		public IntVec3 BillInteractionCell
		{
			get
			{
				return this.InteractionCell;
			}
		}

		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x06008079 RID: 32889 RVA: 0x000563C4 File Offset: 0x000545C4
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				return GenAdj.CellsOccupiedBy(this);
			}
		}

		// Token: 0x0600807A RID: 32890 RVA: 0x00260614 File Offset: 0x0025E814
		public bool CurrentlyUsableForBills()
		{
			return this.UsableForBillsAfterFueling() && (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.CanWorkWithoutFuel || (this.refuelableComp != null && this.refuelableComp.HasFuel));
		}

		// Token: 0x0600807B RID: 32891 RVA: 0x000563CC File Offset: 0x000545CC
		public bool UsableForBillsAfterFueling()
		{
			return (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
		}

		// Token: 0x0400532B RID: 21291
		public BillStack billStack;

		// Token: 0x0400532C RID: 21292
		private CompPowerTrader powerComp;

		// Token: 0x0400532D RID: 21293
		private CompRefuelable refuelableComp;

		// Token: 0x0400532E RID: 21294
		private CompBreakdownable breakdownableComp;
	}
}
