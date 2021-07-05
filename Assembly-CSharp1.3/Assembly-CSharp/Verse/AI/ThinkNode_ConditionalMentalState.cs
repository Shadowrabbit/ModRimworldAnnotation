using System;

namespace Verse.AI
{
	// Token: 0x0200061B RID: 1563
	public class ThinkNode_ConditionalMentalState : ThinkNode_Conditional
	{
		// Token: 0x06002D1E RID: 11550 RVA: 0x0010F19C File Offset: 0x0010D39C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalState thinkNode_ConditionalMentalState = (ThinkNode_ConditionalMentalState)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalState.state = this.state;
			return thinkNode_ConditionalMentalState;
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x0010F1B8 File Offset: 0x0010D3B8
		protected override bool Satisfied(Pawn pawn)
		{
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			return mentalStateDef != null && mentalStateDef == this.state;
		}

		// Token: 0x04001BAD RID: 7085
		public MentalStateDef state;
	}
}
