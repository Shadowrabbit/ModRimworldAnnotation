using System;

namespace Verse.AI
{
	// Token: 0x020005A9 RID: 1449
	public interface IJobEndable
	{
		// Token: 0x06002A5E RID: 10846
		Pawn GetActor();

		// Token: 0x06002A5F RID: 10847
		void AddEndCondition(Func<JobCondition> newEndCondition);
	}
}
