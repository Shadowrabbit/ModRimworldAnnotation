using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000153 RID: 339
	public class LanguageInfo
	{
		// Token: 0x04000867 RID: 2151
		public string friendlyNameNative;

		// Token: 0x04000868 RID: 2152
		public string friendlyNameEnglish;

		// Token: 0x04000869 RID: 2153
		public bool canBeTiny = true;

		// Token: 0x0400086A RID: 2154
		public List<CreditsEntry> credits = new List<CreditsEntry>();

		// Token: 0x0400086B RID: 2155
		public Type languageWorkerClass = typeof(LanguageWorker_Default);
	}
}
