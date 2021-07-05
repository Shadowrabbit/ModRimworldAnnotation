using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000207 RID: 519
	public class LanguageInfo
	{
		// Token: 0x04000B5F RID: 2911
		public string friendlyNameNative;

		// Token: 0x04000B60 RID: 2912
		public string friendlyNameEnglish;

		// Token: 0x04000B61 RID: 2913
		public bool canBeTiny = true;

		// Token: 0x04000B62 RID: 2914
		public List<CreditsEntry> credits = new List<CreditsEntry>();

		// Token: 0x04000B63 RID: 2915
		public Type languageWorkerClass = typeof(LanguageWorker_Default);
	}
}
