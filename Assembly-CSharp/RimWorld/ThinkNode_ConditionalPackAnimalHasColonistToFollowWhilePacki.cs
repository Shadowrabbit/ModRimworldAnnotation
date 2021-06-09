using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E75 RID: 3701
	public class ThinkNode_ConditionalPackAnimalHasColonistToFollowWhilePacking : ThinkNode_Conditional
	{
		// Token: 0x06005324 RID: 21284 RVA: 0x0003A15B File Offset: 0x0003835B
		protected override bool Satisfied(Pawn pawn)
		{
			return JobGiver_PackAnimalFollowColonists.GetPawnToFollow(pawn) != null;
		}
	}
}
