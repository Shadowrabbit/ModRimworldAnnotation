using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E50 RID: 3664
	public class ThinkNode_ConditionalNonPlayerNonHostileFaction : ThinkNode_Conditional
	{
		// Token: 0x060052D4 RID: 21204 RVA: 0x00039DB6 File Offset: 0x00037FB6
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer);
		}
	}
}
