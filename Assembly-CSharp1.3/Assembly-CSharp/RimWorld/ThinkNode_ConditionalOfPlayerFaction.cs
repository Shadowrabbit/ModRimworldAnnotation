using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000901 RID: 2305
	public class ThinkNode_ConditionalOfPlayerFaction : ThinkNode_Conditional
	{
		// Token: 0x06003C33 RID: 15411 RVA: 0x0014F2D7 File Offset: 0x0014D4D7
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer;
		}
	}
}
