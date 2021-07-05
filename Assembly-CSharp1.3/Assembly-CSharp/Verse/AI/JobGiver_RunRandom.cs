using System;

namespace Verse.AI
{
	// Token: 0x0200064C RID: 1612
	public class JobGiver_RunRandom : JobGiver_Wander
	{
		// Token: 0x06002DB0 RID: 11696 RVA: 0x00110B33 File Offset: 0x0010ED33
		public JobGiver_RunRandom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(5, 10);
			this.locomotionUrgency = LocomotionUrgency.Sprint;
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x0011056E File Offset: 0x0010E76E
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
