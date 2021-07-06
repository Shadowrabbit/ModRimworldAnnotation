using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D15 RID: 3349
	public struct MeditationSpotAndFocus
	{
		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x06004CBB RID: 19643 RVA: 0x00036653 File Offset: 0x00034853
		public bool IsValid
		{
			get
			{
				return this.spot.IsValid;
			}
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x00036660 File Offset: 0x00034860
		public MeditationSpotAndFocus(LocalTargetInfo spot)
		{
			this.spot = spot;
			this.focus = LocalTargetInfo.Invalid;
		}

		// Token: 0x06004CBD RID: 19645 RVA: 0x00036674 File Offset: 0x00034874
		public MeditationSpotAndFocus(LocalTargetInfo spot, LocalTargetInfo focus)
		{
			this.spot = spot;
			this.focus = focus;
		}

		// Token: 0x04003279 RID: 12921
		public LocalTargetInfo spot;

		// Token: 0x0400327A RID: 12922
		public LocalTargetInfo focus;
	}
}
