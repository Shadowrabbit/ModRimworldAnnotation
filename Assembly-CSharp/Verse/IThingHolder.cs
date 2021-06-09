using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000512 RID: 1298
	public interface IThingHolder
	{
		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x060020C2 RID: 8386
		IThingHolder ParentHolder { get; }

		// Token: 0x060020C3 RID: 8387
		void GetChildHolders(List<IThingHolder> outChildren);

		// Token: 0x060020C4 RID: 8388
		ThingOwner GetDirectlyHeldThings();
	}
}
