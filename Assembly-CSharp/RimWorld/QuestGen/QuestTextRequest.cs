using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EB5 RID: 7861
	public class QuestTextRequest
	{
		// Token: 0x0400726E RID: 29294
		public string keyword;

		// Token: 0x0400726F RID: 29295
		public Action<string> setter;

		// Token: 0x04007270 RID: 29296
		public List<Rule> extraRules;
	}
}
