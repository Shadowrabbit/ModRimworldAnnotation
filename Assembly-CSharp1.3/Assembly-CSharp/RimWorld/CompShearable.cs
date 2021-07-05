using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001192 RID: 4498
	public class CompShearable : CompHasGatherableBodyResource
	{
		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x06006C33 RID: 27699 RVA: 0x002445A1 File Offset: 0x002427A1
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.shearIntervalDays;
			}
		}

		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x06006C34 RID: 27700 RVA: 0x002445AE File Offset: 0x002427AE
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.woolAmount;
			}
		}

		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x06006C35 RID: 27701 RVA: 0x002445BB File Offset: 0x002427BB
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.woolDef;
			}
		}

		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x06006C36 RID: 27702 RVA: 0x002445C8 File Offset: 0x002427C8
		protected override string SaveKey
		{
			get
			{
				return "woolGrowth";
			}
		}

		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x06006C37 RID: 27703 RVA: 0x002445CF File Offset: 0x002427CF
		public CompProperties_Shearable Props
		{
			get
			{
				return (CompProperties_Shearable)this.props;
			}
		}

		// Token: 0x170012B4 RID: 4788
		// (get) Token: 0x06006C38 RID: 27704 RVA: 0x002445DC File Offset: 0x002427DC
		protected override bool Active
		{
			get
			{
				if (!base.Active)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				return pawn == null || pawn.ageTracker.CurLifeStage.shearable;
			}
		}

		// Token: 0x06006C39 RID: 27705 RVA: 0x00244617 File Offset: 0x00242817
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "WoolGrowth".Translate() + ": " + base.Fullness.ToStringPercent();
		}
	}
}
