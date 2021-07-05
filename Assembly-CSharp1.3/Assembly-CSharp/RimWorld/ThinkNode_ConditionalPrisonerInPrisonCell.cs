using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008FD RID: 2301
	public class ThinkNode_ConditionalPrisonerInPrisonCell : ThinkNode_Conditional
	{
		// Token: 0x06003C2B RID: 15403 RVA: 0x0014F278 File Offset: 0x0014D478
		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.IsPrisoner)
			{
				return false;
			}
			Room room = pawn.GetRoom(RegionType.Set_All);
			return room != null && room.IsPrisonCell;
		}
	}
}
