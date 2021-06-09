using System;

namespace Verse.AI
{
	// Token: 0x02000AAB RID: 2731
	public class JobGiver_RunRandom : JobGiver_Wander
	{
		// Token: 0x0600409D RID: 16541 RVA: 0x000305AE File Offset: 0x0002E7AE
		public JobGiver_RunRandom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(5, 10);
			this.locomotionUrgency = LocomotionUrgency.Sprint;
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x0003044E File Offset: 0x0002E64E
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
