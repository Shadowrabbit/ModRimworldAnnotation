using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000908 RID: 2312
	public class ThinkNode_ConditionalNonPlayerNonHostileFactionOrFactionless : ThinkNode_Conditional
	{
		// Token: 0x06003C42 RID: 15426 RVA: 0x0014F3C8 File Offset: 0x0014D5C8
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == null || (pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer));
		}
	}
}
