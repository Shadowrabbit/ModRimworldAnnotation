using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A27 RID: 2599
	public class MentalState_Jailbreaker : MentalState
	{
		// Token: 0x06003E1D RID: 15901 RVA: 0x0002EBAC File Offset: 0x0002CDAC
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(500) && this.pawn.CurJobDef != JobDefOf.InducePrisonerToEscape && JailbreakerMentalStateUtility.FindPrisoner(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x00177CA4 File Offset: 0x00175EA4
		public void Notify_InducedPrisonerToEscape()
		{
			if (MentalStateDefOf.Wander_OwnRoom.Worker.StateCanOccur(this.pawn))
			{
				this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_OwnRoom, null, false, this.causedByMood, null, true);
				return;
			}
			if (MentalStateDefOf.Wander_Sad.Worker.StateCanOccur(this.pawn))
			{
				this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, this.causedByMood, null, true);
				return;
			}
			base.RecoverFromState();
		}

		// Token: 0x04002AED RID: 10989
		private const int NoPrisonerToFreeCheckInterval = 500;
	}
}
