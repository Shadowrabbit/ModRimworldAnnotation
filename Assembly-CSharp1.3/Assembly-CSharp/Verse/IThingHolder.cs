using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000377 RID: 887
	public interface IThingHolder
	{
		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001993 RID: 6547
		IThingHolder ParentHolder { get; }

		// Token: 0x06001994 RID: 6548
		void GetChildHolders(List<IThingHolder> outChildren);

		// Token: 0x06001995 RID: 6549
		ThingOwner GetDirectlyHeldThings();
	}
}
