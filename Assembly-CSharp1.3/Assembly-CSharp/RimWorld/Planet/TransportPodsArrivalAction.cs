using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017E1 RID: 6113
	public abstract class TransportPodsArrivalAction : IExposable
	{
		// Token: 0x06008E49 RID: 36425 RVA: 0x003211E7 File Offset: 0x0031F3E7
		public virtual FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			return true;
		}

		// Token: 0x06008E4A RID: 36426 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return false;
		}

		// Token: 0x06008E4B RID: 36427
		public abstract void Arrived(List<ActiveDropPodInfo> pods, int tile);

		// Token: 0x06008E4C RID: 36428 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExposeData()
		{
		}
	}
}
