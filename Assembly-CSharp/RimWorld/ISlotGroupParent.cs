using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001691 RID: 5777
	public interface ISlotGroupParent : IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x17001388 RID: 5000
		// (get) Token: 0x06007E5F RID: 32351
		bool IgnoreStoredThingsBeauty { get; }

		// Token: 0x06007E60 RID: 32352
		IEnumerable<IntVec3> AllSlotCells();

		// Token: 0x06007E61 RID: 32353
		List<IntVec3> AllSlotCellsList();

		// Token: 0x06007E62 RID: 32354
		void Notify_ReceivedThing(Thing newItem);

		// Token: 0x06007E63 RID: 32355
		void Notify_LostThing(Thing newItem);

		// Token: 0x06007E64 RID: 32356
		string SlotYielderLabel();

		// Token: 0x06007E65 RID: 32357
		SlotGroup GetSlotGroup();
	}
}
