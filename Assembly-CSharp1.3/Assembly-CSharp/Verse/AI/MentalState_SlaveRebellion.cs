using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005E3 RID: 1507
	public class MentalState_SlaveRebellion : MentalState
	{
		// Token: 0x06002B95 RID: 11157 RVA: 0x001041C2 File Offset: 0x001023C2
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(500) && this.pawn.CurJobDef != JobDefOf.InduceSlaveToRebel && SlaveRebellionUtility.FindSlaveForRebellion(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x04001A95 RID: 6805
		private const int NoSlaveToRebelWithCheckInterval = 500;
	}
}
