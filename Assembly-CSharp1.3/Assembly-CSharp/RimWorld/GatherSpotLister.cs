using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x0200113B RID: 4411
	public class GatherSpotLister
	{
		// Token: 0x060069FF RID: 27135 RVA: 0x0023B00F File Offset: 0x0023920F
		public void RegisterActivated(CompGatherSpot spot)
		{
			if (!this.activeSpots.Contains(spot))
			{
				this.activeSpots.Add(spot);
			}
		}

		// Token: 0x06006A00 RID: 27136 RVA: 0x0023B02B File Offset: 0x0023922B
		public void RegisterDeactivated(CompGatherSpot spot)
		{
			if (this.activeSpots.Contains(spot))
			{
				this.activeSpots.Remove(spot);
			}
		}

		// Token: 0x04003B23 RID: 15139
		public List<CompGatherSpot> activeSpots = new List<CompGatherSpot>();
	}
}
