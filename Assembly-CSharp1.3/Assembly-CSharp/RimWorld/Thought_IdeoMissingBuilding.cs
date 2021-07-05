using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E94 RID: 3732
	public class Thought_IdeoMissingBuilding : Thought_Situational
	{
		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x060057B0 RID: 22448 RVA: 0x001DD869 File Offset: 0x001DBA69
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.demand.parent.Named("BUILDING"));
			}
		}

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x060057B1 RID: 22449 RVA: 0x001DD895 File Offset: 0x001DBA95
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.demand.parent.Named("BUILDING"));
			}
		}

		// Token: 0x060057B2 RID: 22450 RVA: 0x001DD8C4 File Offset: 0x001DBAC4
		protected override ThoughtState CurrentStateInternal()
		{
			if (this.pawn.IsSlave)
			{
				return ThoughtState.Inactive;
			}
			Map mapHeld = this.pawn.MapHeld;
			return mapHeld != null && this.demand.AppliesTo(mapHeld) && !this.demand.BuildingPresent(mapHeld);
		}

		// Token: 0x060057B3 RID: 22451 RVA: 0x001DD918 File Offset: 0x001DBB18
		public override bool GroupsWith(Thought other)
		{
			Thought_IdeoMissingBuilding thought_IdeoMissingBuilding;
			return (thought_IdeoMissingBuilding = (other as Thought_IdeoMissingBuilding)) != null && thought_IdeoMissingBuilding.demand == this.demand;
		}

		// Token: 0x060057B4 RID: 22452 RVA: 0x001DD93F File Offset: 0x001DBB3F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<IdeoBuildingPresenceDemand>(ref this.demand, "demand", false);
		}

		// Token: 0x040033C5 RID: 13253
		public IdeoBuildingPresenceDemand demand;
	}
}
