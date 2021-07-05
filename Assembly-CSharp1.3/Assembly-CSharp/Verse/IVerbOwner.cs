using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004F2 RID: 1266
	public interface IVerbOwner
	{
		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x0600263B RID: 9787
		VerbTracker VerbTracker { get; }

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x0600263C RID: 9788
		List<VerbProperties> VerbProperties { get; }

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x0600263D RID: 9789
		List<Tool> Tools { get; }

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x0600263E RID: 9790
		ImplementOwnerTypeDef ImplementOwnerTypeDef { get; }

		// Token: 0x0600263F RID: 9791
		string UniqueVerbOwnerID();

		// Token: 0x06002640 RID: 9792
		bool VerbsStillUsableBy(Pawn p);

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002641 RID: 9793
		Thing ConstantCaster { get; }
	}
}
