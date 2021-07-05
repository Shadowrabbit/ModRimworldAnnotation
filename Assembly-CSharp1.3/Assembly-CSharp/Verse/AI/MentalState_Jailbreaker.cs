using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005E0 RID: 1504
	public class MentalState_Jailbreaker : MentalState
	{
		// Token: 0x06002B84 RID: 11140 RVA: 0x00103E57 File Offset: 0x00102057
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(500) && this.pawn.CurJobDef != JobDefOf.InducePrisonerToEscape && JailbreakerMentalStateUtility.FindPrisoner(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x00103E96 File Offset: 0x00102096
		public void Notify_InducedPrisonerToEscape()
		{
			MentalStateUtility.TryTransitionToWanderOwnRoom(this);
		}

		// Token: 0x04001A8D RID: 6797
		private const int NoPrisonerToFreeCheckInterval = 500;
	}
}
