using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED3 RID: 3795
	public class ThoughtWorker_NoThroneroom : ThoughtWorker
	{
		// Token: 0x06005416 RID: 21526 RVA: 0x001C2AF4 File Offset: 0x001C0CF4
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
