using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C5 RID: 2501
	public class ThoughtWorker_ThroneRoomRequirementsNotMet : ThoughtWorker_RoomRequirementsNotMet
	{
		// Token: 0x06003E24 RID: 15908 RVA: 0x0015480F File Offset: 0x00152A0F
		protected override IEnumerable<string> UnmetRequirements(Pawn p)
		{
			return p.royalty.GetUnmetThroneroomRequirements(false, false);
		}

		// Token: 0x06003E25 RID: 15909 RVA: 0x0015481E File Offset: 0x00152A1E
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(this.UnmetRequirements(p).ToLineList("- ", false), p.royalty.HighestTitleWithThroneRoomRequirements().Named("TITLE"));
		}
	}
}
