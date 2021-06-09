using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED2 RID: 3794
	public class ThoughtWorker_BedroomRequirementsNotMet : ThoughtWorker_RoomRequirementsNotMet
	{
		// Token: 0x06005413 RID: 21523 RVA: 0x0003A6D3 File Offset: 0x000388D3
		protected override IEnumerable<string> UnmetRequirements(Pawn p)
		{
			return p.royalty.GetUnmetBedroomRequirements(false, false);
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x001C2AA8 File Offset: 0x001C0CA8
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(this.UnmetRequirements(p).ToLineList("- ", false), p.royalty.HighestTitleWithBedroomRequirements().Named("TITLE")).CapitalizeFirst();
		}
	}
}
