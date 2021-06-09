using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E4B RID: 3659
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x060052C9 RID: 21193 RVA: 0x00039D1E File Offset: 0x00037F1E
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && pawn.guest.IsPrisoner);
		}
	}
}
