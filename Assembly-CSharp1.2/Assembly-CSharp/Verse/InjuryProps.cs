using System;

namespace Verse
{
	// Token: 0x0200013D RID: 317
	public class InjuryProps
	{
		// Token: 0x04000653 RID: 1619
		public float painPerSeverity = 1f;

		// Token: 0x04000654 RID: 1620
		public float averagePainPerSeverityPermanent = 0.5f;

		// Token: 0x04000655 RID: 1621
		public float bleedRate;

		// Token: 0x04000656 RID: 1622
		public bool canMerge;

		// Token: 0x04000657 RID: 1623
		public string destroyedLabel;

		// Token: 0x04000658 RID: 1624
		public string destroyedOutLabel;

		// Token: 0x04000659 RID: 1625
		public bool useRemovedLabel;
	}
}
