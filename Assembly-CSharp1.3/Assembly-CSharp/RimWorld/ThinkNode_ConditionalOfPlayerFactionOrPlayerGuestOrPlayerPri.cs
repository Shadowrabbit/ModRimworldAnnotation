using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000904 RID: 2308
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuestOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x06003C39 RID: 15417 RVA: 0x0014F33F File Offset: 0x0014D53F
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer;
		}
	}
}
