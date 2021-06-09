using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E51 RID: 3665
	public class ThinkNode_ConditionalNonPlayerNonHostileFactionOrFactionless : ThinkNode_Conditional
	{
		// Token: 0x060052D6 RID: 21206 RVA: 0x00039DE2 File Offset: 0x00037FE2
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == null || (pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer));
		}
	}
}
