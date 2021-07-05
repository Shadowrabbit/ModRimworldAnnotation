using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000907 RID: 2311
	public class ThinkNode_ConditionalNonPlayerNonHostileFaction : ThinkNode_Conditional
	{
		// Token: 0x06003C40 RID: 15424 RVA: 0x0014F39C File Offset: 0x0014D59C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer);
		}
	}
}
