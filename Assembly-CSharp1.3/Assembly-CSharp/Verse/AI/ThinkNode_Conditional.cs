using System;

namespace Verse.AI
{
	// Token: 0x0200061A RID: 1562
	public abstract class ThinkNode_Conditional : ThinkNode_Priority
	{
		// Token: 0x06002D1A RID: 11546 RVA: 0x0010F160 File Offset: 0x0010D360
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Conditional thinkNode_Conditional = (ThinkNode_Conditional)base.DeepCopy(resolve);
			thinkNode_Conditional.invert = this.invert;
			return thinkNode_Conditional;
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x0010F17A File Offset: 0x0010D37A
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (this.Satisfied(pawn) == !this.invert)
			{
				return base.TryIssueJobPackage(pawn, jobParams);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x06002D1C RID: 11548
		protected abstract bool Satisfied(Pawn pawn);

		// Token: 0x04001BAC RID: 7084
		public bool invert;
	}
}
