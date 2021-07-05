using System;

namespace Verse.AI
{
	// Token: 0x02000646 RID: 1606
	public class JobGiver_WanderNearRoamingExit : JobGiver_Wander
	{
		// Token: 0x06002DA1 RID: 11681 RVA: 0x00110938 File Offset: 0x0010EB38
		public JobGiver_WanderNearRoamingExit()
		{
			this.wanderRadius = 12f;
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x0011094C File Offset: 0x0010EB4C
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_Roaming mentalState_Roaming = pawn.MentalState as MentalState_Roaming;
			if (mentalState_Roaming == null)
			{
				return null;
			}
			if (mentalState_Roaming.ShouldExitMapNow())
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x0011097B File Offset: 0x0010EB7B
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			MentalState_Roaming mentalState_Roaming = pawn.MentalState as MentalState_Roaming;
			if (mentalState_Roaming == null)
			{
				return pawn.Position;
			}
			return mentalState_Roaming.exitDest;
		}
	}
}
