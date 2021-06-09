using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001975 RID: 6517
	public class Alert_UnhappyNudity : Alert_Thought
	{
		// Token: 0x170016C7 RID: 5831
		// (get) Token: 0x06009012 RID: 36882 RVA: 0x00060B6E File Offset: 0x0005ED6E
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.Naked;
			}
		}

		// Token: 0x06009013 RID: 36883 RVA: 0x00060B75 File Offset: 0x0005ED75
		public Alert_UnhappyNudity()
		{
			this.defaultLabel = "AlertUnhappyNudity".Translate();
			this.explanationKey = "AlertUnhappyNudityDesc";
		}
	}
}
