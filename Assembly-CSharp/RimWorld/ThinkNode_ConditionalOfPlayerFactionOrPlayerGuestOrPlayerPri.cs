using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E4D RID: 3661
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuestOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x060052CD RID: 21197 RVA: 0x0001DF84 File Offset: 0x0001C184
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer;
		}
	}
}
