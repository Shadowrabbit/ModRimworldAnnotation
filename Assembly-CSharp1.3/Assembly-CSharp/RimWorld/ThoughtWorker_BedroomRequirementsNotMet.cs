using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C6 RID: 2502
	public class ThoughtWorker_BedroomRequirementsNotMet : ThoughtWorker_RoomRequirementsNotMet
	{
		// Token: 0x06003E27 RID: 15911 RVA: 0x0015485F File Offset: 0x00152A5F
		protected override IEnumerable<string> UnmetRequirements(Pawn p)
		{
			return p.royalty.GetUnmetBedroomRequirements(false, false);
		}

		// Token: 0x06003E28 RID: 15912 RVA: 0x00154870 File Offset: 0x00152A70
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(this.UnmetRequirements(p).ToLineList("- ", false), p.royalty.HighestTitleWithBedroomRequirements().Named("TITLE")).CapitalizeFirst();
		}
	}
}
