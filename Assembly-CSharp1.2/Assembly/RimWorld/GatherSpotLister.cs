using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020017C7 RID: 6087
	public class GatherSpotLister
	{
		// Token: 0x060086AA RID: 34474 RVA: 0x0005A52B File Offset: 0x0005872B
		public void RegisterActivated(CompGatherSpot spot)
		{
			if (!this.activeSpots.Contains(spot))
			{
				this.activeSpots.Add(spot);
			}
		}

		// Token: 0x060086AB RID: 34475 RVA: 0x0005A547 File Offset: 0x00058747
		public void RegisterDeactivated(CompGatherSpot spot)
		{
			if (this.activeSpots.Contains(spot))
			{
				this.activeSpots.Remove(spot);
			}
		}

		// Token: 0x0400569E RID: 22174
		public List<CompGatherSpot> activeSpots = new List<CompGatherSpot>();
	}
}
