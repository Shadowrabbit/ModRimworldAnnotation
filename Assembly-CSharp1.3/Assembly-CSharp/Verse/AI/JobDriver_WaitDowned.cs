using System;

namespace Verse.AI
{
	// Token: 0x02000594 RID: 1428
	public class JobDriver_WaitDowned : JobDriver_Wait
	{
		// Token: 0x060029C9 RID: 10697 RVA: 0x000FCC1D File Offset: 0x000FAE1D
		public override void DecorateWaitToil(Toil wait)
		{
			base.DecorateWaitToil(wait);
			wait.AddFailCondition(() => !this.pawn.Downed);
		}
	}
}
