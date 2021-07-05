using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200108A RID: 4234
	public class Building_WorkTable : Building, IBillGiver, IBillGiverWithTickAction
	{
		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x060064D1 RID: 25809 RVA: 0x0021F914 File Offset: 0x0021DB14
		public bool CanWorkWithoutPower
		{
			get
			{
				return this.powerComp == null || this.def.building.unpoweredWorkTableWorkSpeedFactor > 0f;
			}
		}

		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x060064D2 RID: 25810 RVA: 0x0021F93A File Offset: 0x0021DB3A
		public bool CanWorkWithoutFuel
		{
			get
			{
				return this.refuelableComp == null;
			}
		}

		// Token: 0x060064D3 RID: 25811 RVA: 0x0021F945 File Offset: 0x0021DB45
		public Building_WorkTable()
		{
			this.billStack = new BillStack(this);
		}

		// Token: 0x060064D4 RID: 25812 RVA: 0x0021F959 File Offset: 0x0021DB59
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<BillStack>(ref this.billStack, "billStack", new object[]
			{
				this
			});
		}

		// Token: 0x060064D5 RID: 25813 RVA: 0x0021F97C File Offset: 0x0021DB7C
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

		// Token: 0x060064D6 RID: 25814 RVA: 0x0021F9F4 File Offset: 0x0021DBF4
		public virtual void UsedThisTick()
		{
			if (this.refuelableComp != null)
			{
				this.refuelableComp.Notify_UsedThisTick();
			}
		}

		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x060064D7 RID: 25815 RVA: 0x0021FA09 File Offset: 0x0021DC09
		public BillStack BillStack
		{
			get
			{
				return this.billStack;
			}
		}

		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x060064D8 RID: 25816 RVA: 0x0021FA11 File Offset: 0x0021DC11
		public IntVec3 BillInteractionCell
		{
			get
			{
				return this.InteractionCell;
			}
		}

		// Token: 0x17001148 RID: 4424
		// (get) Token: 0x060064D9 RID: 25817 RVA: 0x0021FA19 File Offset: 0x0021DC19
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				return GenAdj.CellsOccupiedBy(this);
			}
		}

		// Token: 0x060064DA RID: 25818 RVA: 0x0021FA24 File Offset: 0x0021DC24
		public bool CurrentlyUsableForBills()
		{
			return this.UsableForBillsAfterFueling() && (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.CanWorkWithoutFuel || (this.refuelableComp != null && this.refuelableComp.HasFuel));
		}

		// Token: 0x060064DB RID: 25819 RVA: 0x0021FA7A File Offset: 0x0021DC7A
		public bool UsableForBillsAfterFueling()
		{
			return (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
		}

		// Token: 0x040038BA RID: 14522
		public BillStack billStack;

		// Token: 0x040038BB RID: 14523
		private CompPowerTrader powerComp;

		// Token: 0x040038BC RID: 14524
		private CompRefuelable refuelableComp;

		// Token: 0x040038BD RID: 14525
		private CompBreakdownable breakdownableComp;
	}
}
