using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200090A RID: 2314
	public class ThinkNode_ConditionalHasVoluntarilyJoinableLord : ThinkNode_Conditional
	{
		// Token: 0x06003C46 RID: 15430 RVA: 0x0014F404 File Offset: 0x0014D604
		protected override bool Satisfied(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.LordJob is LordJob_VoluntarilyJoinable;
		}
	}
}
