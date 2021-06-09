using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E58 RID: 3672
	public class ThinkNode_ConditionalHasFaction : ThinkNode_Conditional
	{
		// Token: 0x060052E5 RID: 21221 RVA: 0x00039E62 File Offset: 0x00038062
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null;
		}
	}
}
