using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200043A RID: 1082
	public interface ISelectable
	{
		// Token: 0x0600207A RID: 8314
		IEnumerable<Gizmo> GetGizmos();

		// Token: 0x0600207B RID: 8315
		string GetInspectString();

		// Token: 0x0600207C RID: 8316
		IEnumerable<InspectTabBase> GetInspectTabs();
	}
}
