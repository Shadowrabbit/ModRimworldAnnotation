using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020B5 RID: 8373
	public abstract class CaravanArrivalAction : IExposable
	{
		// Token: 0x17001A32 RID: 6706
		// (get) Token: 0x0600B187 RID: 45447
		public abstract string Label { get; }

		// Token: 0x17001A33 RID: 6707
		// (get) Token: 0x0600B188 RID: 45448
		public abstract string ReportString { get; }

		// Token: 0x0600B189 RID: 45449 RVA: 0x000735B8 File Offset: 0x000717B8
		public virtual FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			return true;
		}

		// Token: 0x0600B18A RID: 45450
		public abstract void Arrived(Caravan caravan);

		// Token: 0x0600B18B RID: 45451 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ExposeData()
		{
		}
	}
}
