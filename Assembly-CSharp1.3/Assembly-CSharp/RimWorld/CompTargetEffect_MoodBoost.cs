using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011EB RID: 4587
	public class CompTargetEffect_MoodBoost : CompTargetEffect
	{
		// Token: 0x06006E81 RID: 28289 RVA: 0x0025050C File Offset: 0x0024E70C
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead || pawn.needs == null || pawn.needs.mood == null)
			{
				return;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.ArtifactMoodBoost), null);
		}
	}
}
