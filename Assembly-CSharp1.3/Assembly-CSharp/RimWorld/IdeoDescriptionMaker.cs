using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000A97 RID: 2711
	public class IdeoDescriptionMaker
	{
		// Token: 0x0400252C RID: 9516
		public List<IdeoDescriptionMaker.PatternEntry> patterns;

		// Token: 0x0400252D RID: 9517
		public RulePack rules;

		// Token: 0x0400252E RID: 9518
		public Dictionary<string, string> constants;

		// Token: 0x0200201C RID: 8220
		public class PatternEntry
		{
			// Token: 0x04007B16 RID: 31510
			public IdeoStoryPatternDef def;

			// Token: 0x04007B17 RID: 31511
			public float weight = 1f;
		}
	}
}
