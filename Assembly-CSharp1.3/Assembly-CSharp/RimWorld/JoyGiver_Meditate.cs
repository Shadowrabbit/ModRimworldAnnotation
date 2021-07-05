using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F0 RID: 2032
	public class JoyGiver_Meditate : JoyGiver_InPrivateRoom
	{
		// Token: 0x06003676 RID: 13942 RVA: 0x00134E56 File Offset: 0x00133056
		public override Job TryGiveJob(Pawn pawn)
		{
			if (ModsConfig.RoyaltyActive)
			{
				return MeditationUtility.GetMeditationJob(pawn, true);
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x00134E6E File Offset: 0x0013306E
		public override bool CanBeGivenTo(Pawn pawn)
		{
			return (!ModsConfig.RoyaltyActive || MeditationUtility.CanMeditateNow(pawn)) && base.CanBeGivenTo(pawn);
		}
	}
}
