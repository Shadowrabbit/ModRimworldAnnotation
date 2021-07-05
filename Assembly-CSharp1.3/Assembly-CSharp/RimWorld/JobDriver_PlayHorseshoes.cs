using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006FA RID: 1786
	public class JobDriver_PlayHorseshoes : JobDriver_WatchBuilding
	{
		// Token: 0x060031AC RID: 12716 RVA: 0x00120DA8 File Offset: 0x0011EFA8
		protected override void WatchTickAction()
		{
			if (this.pawn.IsHashIntervalTick(400))
			{
				FleckMaker.ThrowHorseshoe(this.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction();
		}

		// Token: 0x04001D97 RID: 7575
		private const int HorseshoeThrowInterval = 400;
	}
}
