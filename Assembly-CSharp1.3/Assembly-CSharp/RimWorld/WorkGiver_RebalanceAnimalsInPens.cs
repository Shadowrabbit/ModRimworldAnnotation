using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080D RID: 2061
	public class WorkGiver_RebalanceAnimalsInPens : WorkGiver_TakeToPen
	{
		// Token: 0x060036F5 RID: 14069 RVA: 0x001373CC File Offset: 0x001355CC
		public WorkGiver_RebalanceAnimalsInPens()
		{
			this.ropingPriority = RopingPriority.Balanced;
		}
	}
}
