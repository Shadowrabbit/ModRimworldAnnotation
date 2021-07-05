using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA7 RID: 4007
	public class RitualStageAction_ReleaseLanterns : RitualStageTickActionMaker
	{
		// Token: 0x06005EA9 RID: 24233 RVA: 0x00206E0A File Offset: 0x0020500A
		public override IEnumerable<ActionOnTick> GenerateTimedActions(LordJob_Ritual ritual, RitualStage stage)
		{
			StageEndTrigger_DurationPercentage stageEndTrigger_DurationPercentage = (StageEndTrigger_DurationPercentage)stage.endTriggers.FirstOrFallback((StageEndTrigger e) => e is StageEndTrigger_DurationPercentage, null);
			if (stageEndTrigger_DurationPercentage == null)
			{
				yield break;
			}
			int durationTicks = (int)(stageEndTrigger_DurationPercentage.percentage * (float)ritual.DurationTicks);
			foreach (Pawn pawn in ritual.assignments.Participants)
			{
				yield return new ActionOnTick_ReleaseLantern
				{
					tick = durationTicks - this.preliminaryTicks.RandomInRange,
					pawn = pawn,
					woodCost = this.woodCost
				};
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x04003692 RID: 13970
		public int woodCost = 4;

		// Token: 0x04003693 RID: 13971
		public IntRange preliminaryTicks;
	}
}
