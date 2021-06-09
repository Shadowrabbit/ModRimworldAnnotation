using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F6 RID: 6134
	public class CompMilkable : CompHasGatherableBodyResource
	{
		// Token: 0x1700151F RID: 5407
		// (get) Token: 0x060087C1 RID: 34753 RVA: 0x0005B11A File Offset: 0x0005931A
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.milkIntervalDays;
			}
		}

		// Token: 0x17001520 RID: 5408
		// (get) Token: 0x060087C2 RID: 34754 RVA: 0x0005B127 File Offset: 0x00059327
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.milkAmount;
			}
		}

		// Token: 0x17001521 RID: 5409
		// (get) Token: 0x060087C3 RID: 34755 RVA: 0x0005B134 File Offset: 0x00059334
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.milkDef;
			}
		}

		// Token: 0x17001522 RID: 5410
		// (get) Token: 0x060087C4 RID: 34756 RVA: 0x0005B141 File Offset: 0x00059341
		protected override string SaveKey
		{
			get
			{
				return "milkFullness";
			}
		}

		// Token: 0x17001523 RID: 5411
		// (get) Token: 0x060087C5 RID: 34757 RVA: 0x0005B148 File Offset: 0x00059348
		public CompProperties_Milkable Props
		{
			get
			{
				return (CompProperties_Milkable)this.props;
			}
		}

		// Token: 0x17001524 RID: 5412
		// (get) Token: 0x060087C6 RID: 34758 RVA: 0x0027C7A0 File Offset: 0x0027A9A0
		protected override bool Active
		{
			get
			{
				if (!base.Active)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				return (!this.Props.milkFemaleOnly || pawn == null || pawn.gender == Gender.Female) && (pawn == null || pawn.ageTracker.CurLifeStage.milkable);
			}
		}

		// Token: 0x060087C7 RID: 34759 RVA: 0x0005B155 File Offset: 0x00059355
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "MilkFullness".Translate() + ": " + base.Fullness.ToStringPercent();
		}
	}
}
