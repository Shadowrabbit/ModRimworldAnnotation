using System;

namespace Verse.AI
{
	// Token: 0x02000A81 RID: 2689
	public class ThinkNode_ConditionalIntelligence : ThinkNode_Conditional
	{
		// Token: 0x06004012 RID: 16402 RVA: 0x0002FFED File Offset: 0x0002E1ED
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalIntelligence thinkNode_ConditionalIntelligence = (ThinkNode_ConditionalIntelligence)base.DeepCopy(resolve);
			thinkNode_ConditionalIntelligence.minIntelligence = this.minIntelligence;
			return thinkNode_ConditionalIntelligence;
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x00030007 File Offset: 0x0002E207
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.intelligence >= this.minIntelligence;
		}

		// Token: 0x04002C29 RID: 11305
		public Intelligence minIntelligence = Intelligence.ToolUser;
	}
}
