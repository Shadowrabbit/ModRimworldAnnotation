using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B94 RID: 2964
	public class JobDriver_PlayHoopstone : JobDriver_WatchBuilding
	{
		// Token: 0x060045A0 RID: 17824 RVA: 0x001934D0 File Offset: 0x001916D0
		protected override void WatchTickAction()
		{
			if (this.pawn.IsHashIntervalTick(400))
			{
				MoteMaker.ThrowStone(this.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction();
		}

		// Token: 0x04002F0B RID: 12043
		private const int StoneThrowInterval = 400;
	}
}
