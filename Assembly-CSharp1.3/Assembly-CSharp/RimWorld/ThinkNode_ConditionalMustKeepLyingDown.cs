using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000920 RID: 2336
	public class ThinkNode_ConditionalMustKeepLyingDown : ThinkNode_Conditional
	{
		// Token: 0x06003C75 RID: 15477 RVA: 0x0014F79C File Offset: 0x0014D99C
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.CurJob == null || !pawn.GetPosture().Laying())
			{
				return false;
			}
			if (!pawn.Downed)
			{
				if (RestUtility.DisturbancePreventsLyingDown(pawn))
				{
					return false;
				}
				if (!pawn.CurJob.restUntilHealed || !HealthAIUtility.ShouldSeekMedicalRest(pawn))
				{
					if (!pawn.jobs.curDriver.asleep)
					{
						return false;
					}
					if (!pawn.CurJob.playerForced && RestUtility.TimetablePreventsLayDown(pawn))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
