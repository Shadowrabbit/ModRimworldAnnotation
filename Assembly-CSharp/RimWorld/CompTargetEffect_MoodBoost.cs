using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C9 RID: 6345
	public class CompTargetEffect_MoodBoost : CompTargetEffect
	{
		// Token: 0x06008CB7 RID: 36023 RVA: 0x0028D910 File Offset: 0x0028BB10
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
