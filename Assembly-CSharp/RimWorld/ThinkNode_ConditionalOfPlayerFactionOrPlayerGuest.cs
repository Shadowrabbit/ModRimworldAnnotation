using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E4C RID: 3660
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuest : ThinkNode_Conditional
	{
		// Token: 0x060052CB RID: 21195 RVA: 0x00039D49 File Offset: 0x00037F49
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && !pawn.guest.IsPrisoner);
		}
	}
}
