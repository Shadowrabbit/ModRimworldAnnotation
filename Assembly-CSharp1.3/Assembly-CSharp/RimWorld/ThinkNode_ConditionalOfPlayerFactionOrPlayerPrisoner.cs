using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000902 RID: 2306
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x06003C35 RID: 15413 RVA: 0x0014F2E6 File Offset: 0x0014D4E6
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && pawn.guest.IsPrisoner);
		}
	}
}
