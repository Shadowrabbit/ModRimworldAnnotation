using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200216E RID: 8558
	public abstract class TransportPodsArrivalAction : IExposable
	{
		// Token: 0x0600B64D RID: 46669 RVA: 0x000735B8 File Offset: 0x000717B8
		public virtual FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			return true;
		}

		// Token: 0x0600B64E RID: 46670 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return false;
		}

		// Token: 0x0600B64F RID: 46671
		public abstract void Arrived(List<ActiveDropPodInfo> pods, int tile);

		// Token: 0x0600B650 RID: 46672 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ExposeData()
		{
		}
	}
}
