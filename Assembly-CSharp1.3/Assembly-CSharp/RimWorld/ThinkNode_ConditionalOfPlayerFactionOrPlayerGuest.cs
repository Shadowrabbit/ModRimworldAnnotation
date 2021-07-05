using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000903 RID: 2307
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuest : ThinkNode_Conditional
	{
		// Token: 0x06003C37 RID: 15415 RVA: 0x0014F311 File Offset: 0x0014D511
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && !pawn.guest.IsPrisoner);
		}
	}
}
