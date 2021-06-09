using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED1 RID: 3793
	public class ThoughtWorker_ThroneRoomRequirementsNotMet : ThoughtWorker_RoomRequirementsNotMet
	{
		// Token: 0x06005410 RID: 21520 RVA: 0x0003A683 File Offset: 0x00038883
		protected override IEnumerable<string> UnmetRequirements(Pawn p)
		{
			return p.royalty.GetUnmetThroneroomRequirements(false, false);
		}

		// Token: 0x06005411 RID: 21521 RVA: 0x0003A692 File Offset: 0x00038892
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(this.UnmetRequirements(p).ToLineList("- ", false), p.royalty.HighestTitleWithThroneRoomRequirements().Named("TITLE"));
		}
	}
}
