using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C8D RID: 7309
	[DefOf]
	public static class LetterDefOf
	{
		// Token: 0x06009F90 RID: 40848 RVA: 0x0006A5FD File Offset: 0x000687FD
		static LetterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LetterDefOf));
		}

		// Token: 0x04006BE1 RID: 27617
		public static LetterDef ThreatBig;

		// Token: 0x04006BE2 RID: 27618
		public static LetterDef ThreatSmall;

		// Token: 0x04006BE3 RID: 27619
		public static LetterDef NegativeEvent;

		// Token: 0x04006BE4 RID: 27620
		public static LetterDef NeutralEvent;

		// Token: 0x04006BE5 RID: 27621
		public static LetterDef PositiveEvent;

		// Token: 0x04006BE6 RID: 27622
		public static LetterDef Death;

		// Token: 0x04006BE7 RID: 27623
		public static LetterDef NewQuest;

		// Token: 0x04006BE8 RID: 27624
		public static LetterDef AcceptVisitors;

		// Token: 0x04006BE9 RID: 27625
		public static LetterDef BetrayVisitors;

		// Token: 0x04006BEA RID: 27626
		public static LetterDef ChoosePawn;
	}
}
