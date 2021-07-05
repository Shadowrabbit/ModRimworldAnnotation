using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E95 RID: 3733
	public class Thought_IdeoDisrespectedBuilding : Thought_Situational
	{
		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x060057B6 RID: 22454 RVA: 0x001DD958 File Offset: 0x001DBB58
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.demand.parent.Named("BUILDING"));
			}
		}

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x060057B7 RID: 22455 RVA: 0x001DD984 File Offset: 0x001DBB84
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.demand.parent.Named("BUILDING"));
			}
		}

		// Token: 0x060057B8 RID: 22456 RVA: 0x001DD9B0 File Offset: 0x001DBBB0
		protected override ThoughtState CurrentStateInternal()
		{
			Map mapHeld = this.pawn.MapHeld;
			return mapHeld != null && this.demand.AppliesTo(mapHeld) && this.demand.BuildingPresent(mapHeld) && !this.demand.RequirementsSatisfied(mapHeld);
		}

		// Token: 0x060057B9 RID: 22457 RVA: 0x001DDA00 File Offset: 0x001DBC00
		public override bool GroupsWith(Thought other)
		{
			Thought_IdeoDisrespectedBuilding thought_IdeoDisrespectedBuilding;
			return (thought_IdeoDisrespectedBuilding = (other as Thought_IdeoDisrespectedBuilding)) != null && thought_IdeoDisrespectedBuilding.demand == this.demand;
		}

		// Token: 0x060057BA RID: 22458 RVA: 0x001DDA27 File Offset: 0x001DBC27
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<IdeoBuildingPresenceDemand>(ref this.demand, "demand", false);
		}

		// Token: 0x040033C6 RID: 13254
		public IdeoBuildingPresenceDemand demand;
	}
}
