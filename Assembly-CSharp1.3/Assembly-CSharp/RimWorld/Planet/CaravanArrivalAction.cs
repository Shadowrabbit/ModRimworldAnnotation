using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001790 RID: 6032
	public abstract class CaravanArrivalAction : IExposable
	{
		// Token: 0x170016B4 RID: 5812
		// (get) Token: 0x06008B62 RID: 35682
		public abstract string Label { get; }

		// Token: 0x170016B5 RID: 5813
		// (get) Token: 0x06008B63 RID: 35683
		public abstract string ReportString { get; }

		// Token: 0x06008B64 RID: 35684 RVA: 0x003211E7 File Offset: 0x0031F3E7
		public virtual FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			return true;
		}

		// Token: 0x06008B65 RID: 35685
		public abstract void Arrived(Caravan caravan);

		// Token: 0x06008B66 RID: 35686 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExposeData()
		{
		}
	}
}
