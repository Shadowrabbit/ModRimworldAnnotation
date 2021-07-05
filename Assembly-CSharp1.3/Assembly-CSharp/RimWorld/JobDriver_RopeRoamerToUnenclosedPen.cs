using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C9 RID: 1737
	public class JobDriver_RopeRoamerToUnenclosedPen : JobDriver_RopeToPen
	{
		// Token: 0x0600307B RID: 12411 RVA: 0x000D491C File Offset: 0x000D2B1C
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			return roperWaitingAtDest;
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x0011DD0E File Offset: 0x0011BF0E
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
			MentalState_Roaming mentalState_Roaming = JobDriver_RopeRoamerToUnenclosedPen.RoamingMentalState(ropee);
			if (mentalState_Roaming == null)
			{
				return;
			}
			mentalState_Roaming.RecoverFromState();
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x0011DD20 File Offset: 0x0011BF20
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal, CompAnimalPenMarker targetPenMarker)
		{
			return targetPenMarker == base.DestinationMarker && JobDriver_RopeRoamerToUnenclosedPen.RoamingMentalState(animal) != null;
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x0011DD36 File Offset: 0x0011BF36
		private static MentalState_Roaming RoamingMentalState(Pawn ropee)
		{
			return ropee.MentalState as MentalState_Roaming;
		}
	}
}
