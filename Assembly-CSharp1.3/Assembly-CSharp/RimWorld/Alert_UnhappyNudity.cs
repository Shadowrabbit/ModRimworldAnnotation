using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001290 RID: 4752
	public class Alert_UnhappyNudity : Alert_Thought
	{
		// Token: 0x170013C9 RID: 5065
		// (get) Token: 0x06007177 RID: 29047 RVA: 0x0025D41A File Offset: 0x0025B61A
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.Naked;
			}
		}

		// Token: 0x06007178 RID: 29048 RVA: 0x0025D421 File Offset: 0x0025B621
		public Alert_UnhappyNudity()
		{
			this.defaultLabel = "AlertUnhappyNudity".Translate();
			this.explanationKey = "AlertUnhappyNudityDesc";
		}
	}
}
