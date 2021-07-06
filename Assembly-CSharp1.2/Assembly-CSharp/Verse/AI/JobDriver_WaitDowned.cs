using System;

namespace Verse.AI
{
	// Token: 0x02000977 RID: 2423
	public class JobDriver_WaitDowned : JobDriver_Wait
	{
		// Token: 0x06003B55 RID: 15189 RVA: 0x0002D81A File Offset: 0x0002BA1A
		public override void DecorateWaitToil(Toil wait)
		{
			base.DecorateWaitToil(wait);
			wait.AddFailCondition(() => !this.pawn.Downed);
		}
	}
}
