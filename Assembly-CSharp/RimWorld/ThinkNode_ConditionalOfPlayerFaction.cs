using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E4A RID: 3658
	public class ThinkNode_ConditionalOfPlayerFaction : ThinkNode_Conditional
	{
		// Token: 0x060052C7 RID: 21191 RVA: 0x00039D0F File Offset: 0x00037F0F
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer;
		}
	}
}
