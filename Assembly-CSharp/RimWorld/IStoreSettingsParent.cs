using System;

namespace RimWorld
{
	// Token: 0x02001690 RID: 5776
	public interface IStoreSettingsParent
	{
		// Token: 0x17001387 RID: 4999
		// (get) Token: 0x06007E5C RID: 32348
		bool StorageTabVisible { get; }

		// Token: 0x06007E5D RID: 32349
		StorageSettings GetStoreSettings();

		// Token: 0x06007E5E RID: 32350
		StorageSettings GetParentStoreSettings();
	}
}
