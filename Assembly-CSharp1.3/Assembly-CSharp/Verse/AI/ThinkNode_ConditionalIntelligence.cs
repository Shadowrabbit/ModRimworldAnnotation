using System;

namespace Verse.AI
{
	// Token: 0x02000622 RID: 1570
	public class ThinkNode_ConditionalIntelligence : ThinkNode_Conditional
	{
		// Token: 0x06002D31 RID: 11569 RVA: 0x0010F34A File Offset: 0x0010D54A
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalIntelligence thinkNode_ConditionalIntelligence = (ThinkNode_ConditionalIntelligence)base.DeepCopy(resolve);
			thinkNode_ConditionalIntelligence.minIntelligence = this.minIntelligence;
			return thinkNode_ConditionalIntelligence;
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x0010F364 File Offset: 0x0010D564
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.intelligence >= this.minIntelligence;
		}

		// Token: 0x04001BB2 RID: 7090
		public Intelligence minIntelligence = Intelligence.ToolUser;
	}
}
