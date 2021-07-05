using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001160 RID: 4448
	public class CompMilkable : CompHasGatherableBodyResource
	{
		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x06006ADF RID: 27359 RVA: 0x0023E20A File Offset: 0x0023C40A
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.milkIntervalDays;
			}
		}

		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x06006AE0 RID: 27360 RVA: 0x0023E217 File Offset: 0x0023C417
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.milkAmount;
			}
		}

		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x06006AE1 RID: 27361 RVA: 0x0023E224 File Offset: 0x0023C424
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.milkDef;
			}
		}

		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x06006AE2 RID: 27362 RVA: 0x0023E231 File Offset: 0x0023C431
		protected override string SaveKey
		{
			get
			{
				return "milkFullness";
			}
		}

		// Token: 0x17001260 RID: 4704
		// (get) Token: 0x06006AE3 RID: 27363 RVA: 0x0023E238 File Offset: 0x0023C438
		public CompProperties_Milkable Props
		{
			get
			{
				return (CompProperties_Milkable)this.props;
			}
		}

		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x06006AE4 RID: 27364 RVA: 0x0023E248 File Offset: 0x0023C448
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

		// Token: 0x06006AE5 RID: 27365 RVA: 0x0023E29E File Offset: 0x0023C49E
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
