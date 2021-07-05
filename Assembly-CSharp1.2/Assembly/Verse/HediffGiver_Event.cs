using System;

namespace Verse
{
	// Token: 0x0200040D RID: 1037
	public class HediffGiver_Event : HediffGiver
	{
		// Token: 0x06001937 RID: 6455 RVA: 0x00017CC8 File Offset: 0x00015EC8
		public bool EventOccurred(Pawn pawn)
		{
			return Rand.Value < this.chance && base.TryApply(pawn, null);
		}

		// Token: 0x040012E1 RID: 4833
		private float chance = 1f;
	}
}
