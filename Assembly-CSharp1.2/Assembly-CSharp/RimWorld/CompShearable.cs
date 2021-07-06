using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001839 RID: 6201
	public class CompShearable : CompHasGatherableBodyResource
	{
		// Token: 0x17001587 RID: 5511
		// (get) Token: 0x0600897A RID: 35194 RVA: 0x0005C523 File Offset: 0x0005A723
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.shearIntervalDays;
			}
		}

		// Token: 0x17001588 RID: 5512
		// (get) Token: 0x0600897B RID: 35195 RVA: 0x0005C530 File Offset: 0x0005A730
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.woolAmount;
			}
		}

		// Token: 0x17001589 RID: 5513
		// (get) Token: 0x0600897C RID: 35196 RVA: 0x0005C53D File Offset: 0x0005A73D
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.woolDef;
			}
		}

		// Token: 0x1700158A RID: 5514
		// (get) Token: 0x0600897D RID: 35197 RVA: 0x0005C54A File Offset: 0x0005A74A
		protected override string SaveKey
		{
			get
			{
				return "woolGrowth";
			}
		}

		// Token: 0x1700158B RID: 5515
		// (get) Token: 0x0600897E RID: 35198 RVA: 0x0005C551 File Offset: 0x0005A751
		public CompProperties_Shearable Props
		{
			get
			{
				return (CompProperties_Shearable)this.props;
			}
		}

		// Token: 0x1700158C RID: 5516
		// (get) Token: 0x0600897F RID: 35199 RVA: 0x002825F0 File Offset: 0x002807F0
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

		// Token: 0x06008980 RID: 35200 RVA: 0x0005C55E File Offset: 0x0005A75E
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
