using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E46 RID: 3654
	public class ThinkNode_ConditionalPrisonerInPrisonCell : ThinkNode_Conditional
	{
		// Token: 0x060052BF RID: 21183 RVA: 0x001BFB74 File Offset: 0x001BDD74
		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.IsPrisoner)
			{
				return false;
			}
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			return room != null && room.isPrisonCell;
		}
	}
}
