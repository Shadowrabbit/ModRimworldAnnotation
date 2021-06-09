using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E4F RID: 3663
	public class ThinkNode_ConditionalTrainableCompleted : ThinkNode_Conditional
	{
		// Token: 0x060052D1 RID: 21201 RVA: 0x00039D7F File Offset: 0x00037F7F
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalTrainableCompleted thinkNode_ConditionalTrainableCompleted = (ThinkNode_ConditionalTrainableCompleted)base.DeepCopy(resolve);
			thinkNode_ConditionalTrainableCompleted.trainable = this.trainable;
			return thinkNode_ConditionalTrainableCompleted;
		}

		// Token: 0x060052D2 RID: 21202 RVA: 0x00039D99 File Offset: 0x00037F99
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.training != null && pawn.training.HasLearned(this.trainable);
		}

		// Token: 0x040034FA RID: 13562
		private TrainableDef trainable;
	}
}
