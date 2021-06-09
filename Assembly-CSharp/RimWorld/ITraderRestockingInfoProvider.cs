using System;

namespace RimWorld
{
	// Token: 0x020018E1 RID: 6369
	public interface ITraderRestockingInfoProvider
	{
		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x06008D11 RID: 36113
		bool EverVisited { get; }

		// Token: 0x17001632 RID: 5682
		// (get) Token: 0x06008D12 RID: 36114
		bool RestockedSinceLastVisit { get; }

		// Token: 0x17001633 RID: 5683
		// (get) Token: 0x06008D13 RID: 36115
		int NextRestockTick { get; }
	}
}
