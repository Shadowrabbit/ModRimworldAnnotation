using System;

namespace Verse.AI
{
	// Token: 0x02000A79 RID: 2681
	public abstract class ThinkNode_Conditional : ThinkNode_Priority
	{
		// Token: 0x06003FFB RID: 16379 RVA: 0x0002FEBD File Offset: 0x0002E0BD
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Conditional thinkNode_Conditional = (ThinkNode_Conditional)base.DeepCopy(resolve);
			thinkNode_Conditional.invert = this.invert;
			return thinkNode_Conditional;
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x0002FED7 File Offset: 0x0002E0D7
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (this.Satisfied(pawn) == !this.invert)
			{
				return base.TryIssueJobPackage(pawn, jobParams);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x06003FFD RID: 16381
		protected abstract bool Satisfied(Pawn pawn);

		// Token: 0x04002C23 RID: 11299
		public bool invert;
	}
}
