using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F50 RID: 8016
	public class QuestNode_GetRandomInRangeForChallengeRating : QuestNode
	{
		// Token: 0x0600AB14 RID: 43796 RVA: 0x0031E0FC File Offset: 0x0031C2FC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			float randomInRange = this.GetRangeFromRating().RandomInRange;
			slate.Set<float>(this.storeAs.GetValue(slate), this.roundRandom.GetValue(slate) ? ((float)GenMath.RoundRandom(randomInRange)) : randomInRange, false);
		}

		// Token: 0x0600AB15 RID: 43797 RVA: 0x0031E14C File Offset: 0x0031C34C
		public FloatRange GetRangeFromRating()
		{
			int challengeRating = QuestGen.quest.challengeRating;
			Slate slate = QuestGen.slate;
			if (challengeRating == 3)
			{
				return this.threeStarRange.GetValue(slate);
			}
			if (challengeRating == 2)
			{
				return this.twoStarRange.GetValue(slate);
			}
			return this.oneStarRange.GetValue(slate);
		}

		// Token: 0x0600AB16 RID: 43798 RVA: 0x0006FF16 File Offset: 0x0006E116
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<int>(this.storeAs.GetValue(slate), 0, false);
			return true;
		}

		// Token: 0x0400746A RID: 29802
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400746B RID: 29803
		public SlateRef<FloatRange> oneStarRange;

		// Token: 0x0400746C RID: 29804
		public SlateRef<FloatRange> twoStarRange;

		// Token: 0x0400746D RID: 29805
		public SlateRef<FloatRange> threeStarRange;

		// Token: 0x0400746E RID: 29806
		public SlateRef<bool> roundRandom;
	}
}
