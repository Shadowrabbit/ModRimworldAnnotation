using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C9A RID: 3226
	public class JobGiver_AIFollowMaster : JobGiver_AIFollowPawn
	{
		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06004B2C RID: 19244 RVA: 0x000325BD File Offset: 0x000307BD
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06004B2D RID: 19245 RVA: 0x00035A30 File Offset: 0x00033C30
		protected override Pawn GetFollowee(Pawn pawn)
		{
			if (pawn.playerSettings == null)
			{
				return null;
			}
			return pawn.playerSettings.Master;
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x00035A47 File Offset: 0x00033C47
		protected override float GetRadius(Pawn pawn)
		{
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				return 50f;
			}
			return 3f;
		}

		// Token: 0x040031BD RID: 12733
		public const float RadiusUnreleased = 3f;

		// Token: 0x040031BE RID: 12734
		public const float RadiusReleased = 50f;
	}
}
