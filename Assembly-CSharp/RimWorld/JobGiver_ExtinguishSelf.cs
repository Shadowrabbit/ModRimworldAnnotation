using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CCF RID: 3279
	public class JobGiver_ExtinguishSelf : ThinkNode_JobGiver
	{
		// Token: 0x06004BC4 RID: 19396 RVA: 0x001A6E1C File Offset: 0x001A501C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Rand.Value < 0.1f)
			{
				Fire fire = (Fire)pawn.GetAttachment(ThingDefOf.Fire);
				if (fire != null)
				{
					return JobMaker.MakeJob(JobDefOf.ExtinguishSelf, fire);
				}
			}
			return null;
		}

		// Token: 0x040031F9 RID: 12793
		private const float ActivateChance = 0.1f;
	}
}
