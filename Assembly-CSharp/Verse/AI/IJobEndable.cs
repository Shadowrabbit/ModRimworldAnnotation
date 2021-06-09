using System;

namespace Verse.AI
{
	// Token: 0x020009AE RID: 2478
	public interface IJobEndable
	{
		// Token: 0x06003C6E RID: 15470
		Pawn GetActor();

		// Token: 0x06003C6F RID: 15471
		void AddEndCondition(Func<JobCondition> newEndCondition);
	}
}
