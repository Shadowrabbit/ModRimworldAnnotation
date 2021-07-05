using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005DD RID: 1501
	public static class MentalStateUtility
	{
		// Token: 0x06002B76 RID: 11126 RVA: 0x001037DF File Offset: 0x001019DF
		public static MentalStateDef GetWanderToOwnRoomStateOrFallback(Pawn pawn)
		{
			if (MentalStateDefOf.Wander_OwnRoom.Worker.StateCanOccur(pawn))
			{
				return MentalStateDefOf.Wander_OwnRoom;
			}
			if (MentalStateDefOf.Wander_Sad.Worker.StateCanOccur(pawn))
			{
				return MentalStateDefOf.Wander_Sad;
			}
			return null;
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x00103814 File Offset: 0x00101A14
		public static void TryTransitionToWanderOwnRoom(MentalState mentalState)
		{
			MentalStateDef wanderToOwnRoomStateOrFallback = MentalStateUtility.GetWanderToOwnRoomStateOrFallback(mentalState.pawn);
			if (wanderToOwnRoomStateOrFallback != null)
			{
				mentalState.pawn.mindState.mentalStateHandler.TryStartMentalState(wanderToOwnRoomStateOrFallback, null, false, mentalState.causedByMood, null, true, false, false);
				return;
			}
			mentalState.RecoverFromState();
		}
	}
}
