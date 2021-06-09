using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B95 RID: 2965
	public class JobDriver_PlayHorseshoes : JobDriver_WatchBuilding
	{
		// Token: 0x060045A2 RID: 17826 RVA: 0x00193510 File Offset: 0x00191710
		protected override void WatchTickAction()
		{
			if (this.pawn.IsHashIntervalTick(400))
			{
				MoteMaker.ThrowHorseshoe(this.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction();
		}

		// Token: 0x04002F0C RID: 12044
		private const int HorseshoeThrowInterval = 400;
	}
}
