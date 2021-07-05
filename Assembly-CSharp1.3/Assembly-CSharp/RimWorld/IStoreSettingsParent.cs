using System;

namespace RimWorld
{
	// Token: 0x0200105C RID: 4188
	public interface IStoreSettingsParent
	{
		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x0600634F RID: 25423
		bool StorageTabVisible { get; }

		// Token: 0x06006350 RID: 25424
		StorageSettings GetStoreSettings();

		// Token: 0x06006351 RID: 25425
		StorageSettings GetParentStoreSettings();
	}
}
