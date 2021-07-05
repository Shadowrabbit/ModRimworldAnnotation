using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C94 RID: 3220
	public class JobGiver_AIDefendMaster : JobGiver_AIDefendPawn
	{
		// Token: 0x06004B0F RID: 19215 RVA: 0x00035900 File Offset: 0x00033B00
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn.playerSettings.Master;
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x0003590D File Offset: 0x00033B0D
		protected override float GetFlagRadius(Pawn pawn)
		{
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				return 50f;
			}
			return 5f;
		}

		// Token: 0x040031B2 RID: 12722
		private const float RadiusUnreleased = 5f;
	}
}
