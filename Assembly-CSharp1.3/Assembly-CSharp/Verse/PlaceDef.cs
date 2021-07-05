using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020000EF RID: 239
	public class PlaceDef : Def
	{
		// Token: 0x040005A5 RID: 1445
		public RulePack placeRules;

		// Token: 0x040005A6 RID: 1446
		[NoTranslate]
		public List<string> tags = new List<string>();
	}
}
