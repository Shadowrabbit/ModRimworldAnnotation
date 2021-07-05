using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200077D RID: 1917
	public interface ISelectable
	{
		// Token: 0x06003023 RID: 12323
		IEnumerable<Gizmo> GetGizmos();

		// Token: 0x06003024 RID: 12324
		string GetInspectString();

		// Token: 0x06003025 RID: 12325
		IEnumerable<InspectTabBase> GetInspectTabs();
	}
}
