using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200077A RID: 1914
	public class JobGiver_AIDefendMaster : JobGiver_AIDefendPawn
	{
		// Token: 0x060034B9 RID: 13497 RVA: 0x0012AB4A File Offset: 0x00128D4A
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn.playerSettings.Master;
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x0012AB57 File Offset: 0x00128D57
		protected override float GetFlagRadius(Pawn pawn)
		{
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				return 50f;
			}
			return 5f;
		}

		// Token: 0x04001E60 RID: 7776
		private const float RadiusUnreleased = 5f;
	}
}
