using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E69 RID: 3689
	public class ThinkNode_ConditionalMustKeepLyingDown : ThinkNode_Conditional
	{
		// Token: 0x06005309 RID: 21257 RVA: 0x001BFCF4 File Offset: 0x001BDEF4
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
