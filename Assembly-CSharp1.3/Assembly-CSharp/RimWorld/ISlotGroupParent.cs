using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105D RID: 4189
	public interface ISlotGroupParent : IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x06006352 RID: 25426
		bool IgnoreStoredThingsBeauty { get; }

		// Token: 0x06006353 RID: 25427
		IEnumerable<IntVec3> AllSlotCells();

		// Token: 0x06006354 RID: 25428
		List<IntVec3> AllSlotCellsList();

		// Token: 0x06006355 RID: 25429
		void Notify_ReceivedThing(Thing newItem);

		// Token: 0x06006356 RID: 25430
		void Notify_LostThing(Thing newItem);

		// Token: 0x06006357 RID: 25431
		string SlotYielderLabel();

		// Token: 0x06006358 RID: 25432
		SlotGroup GetSlotGroup();
	}
}
