using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000906 RID: 2310
	public class ThinkNode_ConditionalTrainableCompleted : ThinkNode_Conditional
	{
		// Token: 0x06003C3D RID: 15421 RVA: 0x0014F365 File Offset: 0x0014D565
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalTrainableCompleted thinkNode_ConditionalTrainableCompleted = (ThinkNode_ConditionalTrainableCompleted)base.DeepCopy(resolve);
			thinkNode_ConditionalTrainableCompleted.trainable = this.trainable;
			return thinkNode_ConditionalTrainableCompleted;
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x0014F37F File Offset: 0x0014D57F
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.training != null && pawn.training.HasLearned(this.trainable);
		}

		// Token: 0x040020AC RID: 8364
		private TrainableDef trainable;
	}
}
