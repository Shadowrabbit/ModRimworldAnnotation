using System;

namespace Verse
{
	// Token: 0x020002D6 RID: 726
	public class HediffGiver_Event : HediffGiver
	{
		// Token: 0x06001397 RID: 5015 RVA: 0x0006F1C5 File Offset: 0x0006D3C5
		public bool EventOccurred(Pawn pawn)
		{
			return Rand.Value < this.chance && base.TryApply(pawn, null);
		}

		// Token: 0x04000E6C RID: 3692
		private float chance = 1f;
	}
}
