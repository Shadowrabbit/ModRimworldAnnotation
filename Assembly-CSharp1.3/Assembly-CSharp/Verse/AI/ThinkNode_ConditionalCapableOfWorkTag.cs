using System;

namespace Verse.AI
{
	// Token: 0x0200061F RID: 1567
	public class ThinkNode_ConditionalCapableOfWorkTag : ThinkNode_Conditional
	{
		// Token: 0x06002D29 RID: 11561 RVA: 0x0010F263 File Offset: 0x0010D463
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCapableOfWorkTag thinkNode_ConditionalCapableOfWorkTag = (ThinkNode_ConditionalCapableOfWorkTag)base.DeepCopy(resolve);
			thinkNode_ConditionalCapableOfWorkTag.workTags = this.workTags;
			return thinkNode_ConditionalCapableOfWorkTag;
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x0010F27D File Offset: 0x0010D47D
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.WorkTagIsDisabled(this.workTags);
		}

		// Token: 0x04001BB0 RID: 7088
		public WorkTags workTags;
	}
}
