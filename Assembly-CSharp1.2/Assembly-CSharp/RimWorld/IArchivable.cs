using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101F RID: 4127
	public interface IArchivable : IExposable, ILoadReferenceable
	{
		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06005A13 RID: 23059
		Texture ArchivedIcon { get; }

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06005A14 RID: 23060
		Color ArchivedIconColor { get; }

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06005A15 RID: 23061
		string ArchivedLabel { get; }

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06005A16 RID: 23062
		string ArchivedTooltip { get; }

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06005A17 RID: 23063
		int CreatedTicksGame { get; }

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06005A18 RID: 23064
		bool CanCullArchivedNow { get; }

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06005A19 RID: 23065
		LookTargets LookTargets { get; }

		// Token: 0x06005A1A RID: 23066
		void OpenArchived();
	}
}
