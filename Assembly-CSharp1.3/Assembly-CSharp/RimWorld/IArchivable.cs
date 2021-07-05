using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF5 RID: 2805
	public interface IArchivable : IExposable, ILoadReferenceable
	{
		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06004207 RID: 16903
		Texture ArchivedIcon { get; }

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06004208 RID: 16904
		Color ArchivedIconColor { get; }

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06004209 RID: 16905
		string ArchivedLabel { get; }

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x0600420A RID: 16906
		string ArchivedTooltip { get; }

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x0600420B RID: 16907
		int CreatedTicksGame { get; }

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x0600420C RID: 16908
		bool CanCullArchivedNow { get; }

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x0600420D RID: 16909
		LookTargets LookTargets { get; }

		// Token: 0x0600420E RID: 16910
		void OpenArchived();
	}
}
