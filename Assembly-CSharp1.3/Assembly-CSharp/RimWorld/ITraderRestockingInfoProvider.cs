using System;

namespace RimWorld
{
	// Token: 0x02001212 RID: 4626
	public interface ITraderRestockingInfoProvider
	{
		// Token: 0x17001352 RID: 4946
		// (get) Token: 0x06006F10 RID: 28432
		bool EverVisited { get; }

		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x06006F11 RID: 28433
		bool RestockedSinceLastVisit { get; }

		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x06006F12 RID: 28434
		int NextRestockTick { get; }
	}
}
