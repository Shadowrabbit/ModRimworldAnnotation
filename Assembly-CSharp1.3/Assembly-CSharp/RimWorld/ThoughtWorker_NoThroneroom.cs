using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C7 RID: 2503
	public class ThoughtWorker_NoThroneroom : ThoughtWorker
	{
		// Token: 0x06003E2A RID: 15914 RVA: 0x001548BC File Offset: 0x00152ABC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null || p.MapHeld == null || !p.MapHeld.IsPlayerHome || p.royalty.HighestTitleWithThroneRoomRequirements() == null)
			{
				return false;
			}
			return p.ownership.AssignedThrone == null;
		}
	}
}
