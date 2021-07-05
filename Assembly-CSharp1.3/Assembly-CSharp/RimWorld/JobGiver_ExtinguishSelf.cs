using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007BE RID: 1982
	public class JobGiver_ExtinguishSelf : ThinkNode_JobGiver
	{
		// Token: 0x06003599 RID: 13721 RVA: 0x0012EF48 File Offset: 0x0012D148
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

		// Token: 0x04001EA6 RID: 7846
		private const float ActivateChance = 0.1f;
	}
}
