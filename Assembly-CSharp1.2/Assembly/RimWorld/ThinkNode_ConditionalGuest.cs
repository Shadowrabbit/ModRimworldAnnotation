using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E47 RID: 3655
	public class ThinkNode_ConditionalGuest : ThinkNode_Conditional
	{
		// Token: 0x060052C1 RID: 21185 RVA: 0x00039CE3 File Offset: 0x00037EE3
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HostFaction != null && !pawn.IsPrisoner;
		}
	}
}
