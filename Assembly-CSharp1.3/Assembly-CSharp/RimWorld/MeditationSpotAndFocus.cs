using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007ED RID: 2029
	public struct MeditationSpotAndFocus
	{
		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06003655 RID: 13909 RVA: 0x001340A7 File Offset: 0x001322A7
		public bool IsValid
		{
			get
			{
				return this.spot.IsValid;
			}
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x001340B4 File Offset: 0x001322B4
		public MeditationSpotAndFocus(LocalTargetInfo spot)
		{
			this.spot = spot;
			this.focus = LocalTargetInfo.Invalid;
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x001340C8 File Offset: 0x001322C8
		public MeditationSpotAndFocus(LocalTargetInfo spot, LocalTargetInfo focus)
		{
			this.spot = spot;
			this.focus = focus;
		}

		// Token: 0x04001EE6 RID: 7910
		public LocalTargetInfo spot;

		// Token: 0x04001EE7 RID: 7911
		public LocalTargetInfo focus;
	}
}
