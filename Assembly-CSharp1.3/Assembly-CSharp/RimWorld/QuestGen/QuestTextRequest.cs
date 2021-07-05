using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200160B RID: 5643
	public class QuestTextRequest
	{
		// Token: 0x04005257 RID: 21079
		public string keyword;

		// Token: 0x04005258 RID: 21080
		public Action<string> setter;

		// Token: 0x04005259 RID: 21081
		public List<Rule> extraRules;
	}
}
