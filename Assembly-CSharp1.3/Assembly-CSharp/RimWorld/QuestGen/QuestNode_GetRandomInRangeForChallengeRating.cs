using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200168A RID: 5770
	public class QuestNode_GetRandomInRangeForChallengeRating : QuestNode
	{
		// Token: 0x06008635 RID: 34357 RVA: 0x0030214C File Offset: 0x0030034C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			float randomInRange = this.GetRangeFromRating().RandomInRange;
			slate.Set<float>(this.storeAs.GetValue(slate), this.roundRandom.GetValue(slate) ? ((float)GenMath.RoundRandom(randomInRange)) : randomInRange, false);
		}

		// Token: 0x06008636 RID: 34358 RVA: 0x0030219C File Offset: 0x0030039C
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

		// Token: 0x06008637 RID: 34359 RVA: 0x003021E8 File Offset: 0x003003E8
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<int>(this.storeAs.GetValue(slate), 0, false);
			return true;
		}

		// Token: 0x04005403 RID: 21507
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005404 RID: 21508
		public SlateRef<FloatRange> oneStarRange;

		// Token: 0x04005405 RID: 21509
		public SlateRef<FloatRange> twoStarRange;

		// Token: 0x04005406 RID: 21510
		public SlateRef<FloatRange> threeStarRange;

		// Token: 0x04005407 RID: 21511
		public SlateRef<bool> roundRandom;
	}
}
