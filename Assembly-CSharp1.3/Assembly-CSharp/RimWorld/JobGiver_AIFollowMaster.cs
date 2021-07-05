using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200077F RID: 1919
	public class JobGiver_AIFollowMaster : JobGiver_AIFollowPawn
	{
		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x060034D4 RID: 13524 RVA: 0x0011EB3C File Offset: 0x0011CD3C
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x0012B1C0 File Offset: 0x001293C0
		protected override Pawn GetFollowee(Pawn pawn)
		{
			if (pawn.playerSettings == null)
			{
				return null;
			}
			return pawn.playerSettings.Master;
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x0012B1D7 File Offset: 0x001293D7
		protected override float GetRadius(Pawn pawn)
		{
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				return 50f;
			}
			return 3f;
		}

		// Token: 0x04001E69 RID: 7785
		public const float RadiusUnreleased = 3f;

		// Token: 0x04001E6A RID: 7786
		public const float RadiusReleased = 50f;
	}
}
