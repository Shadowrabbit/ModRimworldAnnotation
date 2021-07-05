using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D95 RID: 7573
	public struct RoyalTitleInheritanceOutcome
	{
		// Token: 0x17001932 RID: 6450
		// (get) Token: 0x0600A47B RID: 42107 RVA: 0x0006D0FC File Offset: 0x0006B2FC
		public bool FoundHeir
		{
			get
			{
				return this.heir != null;
			}
		}

		// Token: 0x17001933 RID: 6451
		// (get) Token: 0x0600A47C RID: 42108 RVA: 0x0006D107 File Offset: 0x0006B307
		public bool HeirHasTitle
		{
			get
			{
				return this.heirCurrentTitle != null;
			}
		}

		// Token: 0x04006F80 RID: 28544
		public Pawn heir;

		// Token: 0x04006F81 RID: 28545
		public RoyalTitleDef heirCurrentTitle;

		// Token: 0x04006F82 RID: 28546
		public bool heirTitleHigher;
	}
}
