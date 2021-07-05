using System;

namespace Verse.AI
{
	// Token: 0x02000A7E RID: 2686
	public class ThinkNode_ConditionalCapableOfWorkTag : ThinkNode_Conditional
	{
		// Token: 0x0600400A RID: 16394 RVA: 0x0002FF82 File Offset: 0x0002E182
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCapableOfWorkTag thinkNode_ConditionalCapableOfWorkTag = (ThinkNode_ConditionalCapableOfWorkTag)base.DeepCopy(resolve);
			thinkNode_ConditionalCapableOfWorkTag.workTags = this.workTags;
			return thinkNode_ConditionalCapableOfWorkTag;
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x0002FF9C File Offset: 0x0002E19C
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.WorkTagIsDisabled(this.workTags);
		}

		// Token: 0x04002C27 RID: 11303
		public WorkTags workTags;
	}
}
