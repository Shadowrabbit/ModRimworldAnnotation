using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200144E RID: 5198
	[DefOf]
	public static class LetterDefOf
	{
		// Token: 0x06007D41 RID: 32065 RVA: 0x002C4A6D File Offset: 0x002C2C6D
		static LetterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LetterDefOf));
		}

		// Token: 0x04004CD8 RID: 19672
		public static LetterDef ThreatBig;

		// Token: 0x04004CD9 RID: 19673
		public static LetterDef ThreatSmall;

		// Token: 0x04004CDA RID: 19674
		public static LetterDef NegativeEvent;

		// Token: 0x04004CDB RID: 19675
		public static LetterDef NeutralEvent;

		// Token: 0x04004CDC RID: 19676
		public static LetterDef PositiveEvent;

		// Token: 0x04004CDD RID: 19677
		public static LetterDef Death;

		// Token: 0x04004CDE RID: 19678
		public static LetterDef NewQuest;

		// Token: 0x04004CDF RID: 19679
		public static LetterDef AcceptVisitors;

		// Token: 0x04004CE0 RID: 19680
		public static LetterDef AcceptJoiner;

		// Token: 0x04004CE1 RID: 19681
		public static LetterDef BetrayVisitors;

		// Token: 0x04004CE2 RID: 19682
		public static LetterDef ChoosePawn;

		// Token: 0x04004CE3 RID: 19683
		public static LetterDef RitualOutcomeNegative;

		// Token: 0x04004CE4 RID: 19684
		public static LetterDef RitualOutcomePositive;

		// Token: 0x04004CE5 RID: 19685
		[MayRequireIdeology]
		public static LetterDef RelicHuntInstallationFound;
	}
}
