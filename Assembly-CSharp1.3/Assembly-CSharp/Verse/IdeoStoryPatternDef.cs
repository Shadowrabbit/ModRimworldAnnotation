using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020000D2 RID: 210
	public class IdeoStoryPatternDef : Def
	{
		// Token: 0x0600060F RID: 1551 RVA: 0x0001E958 File Offset: 0x0001CB58
		public override IEnumerable<string> ConfigErrors()
		{
			if (!this.segments.Any<string>())
			{
				yield return "Pattern must have at least one segment";
			}
			yield break;
		}

		// Token: 0x04000492 RID: 1170
		[NoTranslate]
		public List<string> segments = new List<string>();

		// Token: 0x04000493 RID: 1171
		public List<string> noCapitalizeFirstSentence = new List<string>();

		// Token: 0x04000494 RID: 1172
		public RulePack rules;
	}
}
