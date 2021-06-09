using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D21 RID: 3361
	public class JoyGiver_Meditate : JoyGiver_InPrivateRoom
	{
		// Token: 0x06004D14 RID: 19732 RVA: 0x000369DE File Offset: 0x00034BDE
		public override Job TryGiveJob(Pawn pawn)
		{
			if (ModsConfig.RoyaltyActive)
			{
				return MeditationUtility.GetMeditationJob(pawn, true);
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06004D15 RID: 19733 RVA: 0x000369F6 File Offset: 0x00034BF6
		public override bool CanBeGivenTo(Pawn pawn)
		{
			return (!ModsConfig.RoyaltyActive || MeditationUtility.CanMeditateNow(pawn)) && base.CanBeGivenTo(pawn);
		}
	}
}
