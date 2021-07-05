using System;

namespace Verse
{
	// Token: 0x020000CD RID: 205
	public class InjuryProps
	{
		// Token: 0x0400045B RID: 1115
		public float painPerSeverity = 1f;

		// Token: 0x0400045C RID: 1116
		public float averagePainPerSeverityPermanent = 0.5f;

		// Token: 0x0400045D RID: 1117
		public float bleedRate;

		// Token: 0x0400045E RID: 1118
		public bool canMerge;

		// Token: 0x0400045F RID: 1119
		public string destroyedLabel;

		// Token: 0x04000460 RID: 1120
		public string destroyedOutLabel;

		// Token: 0x04000461 RID: 1121
		public bool useRemovedLabel;
	}
}
