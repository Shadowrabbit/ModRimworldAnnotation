using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F9 RID: 1785
	public class JobDriver_PlayHoopstone : JobDriver_WatchBuilding
	{
		// Token: 0x060031AA RID: 12714 RVA: 0x00120D60 File Offset: 0x0011EF60
		protected override void WatchTickAction()
		{
			if (this.pawn.IsHashIntervalTick(400))
			{
				FleckMaker.ThrowStone(this.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction();
		}

		// Token: 0x04001D96 RID: 7574
		private const int StoneThrowInterval = 400;
	}
}
