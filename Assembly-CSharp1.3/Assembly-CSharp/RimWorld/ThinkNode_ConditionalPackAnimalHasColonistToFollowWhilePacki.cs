using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200092B RID: 2347
	public class ThinkNode_ConditionalPackAnimalHasColonistToFollowWhilePacking : ThinkNode_Conditional
	{
		// Token: 0x06003C8D RID: 15501 RVA: 0x0014FA06 File Offset: 0x0014DC06
		protected override bool Satisfied(Pawn pawn)
		{
			return JobGiver_PackAnimalFollowColonists.GetPawnToFollow(pawn) != null;
		}
	}
}
