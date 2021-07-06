using System;

namespace Verse.AI
{
	// Token: 0x02000A7A RID: 2682
	public class ThinkNode_ConditionalMentalState : ThinkNode_Conditional
	{
		// Token: 0x06003FFF RID: 16383 RVA: 0x0002FEF9 File Offset: 0x0002E0F9
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalState thinkNode_ConditionalMentalState = (ThinkNode_ConditionalMentalState)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalState.state = this.state;
			return thinkNode_ConditionalMentalState;
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x0002FF13 File Offset: 0x0002E113
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MentalStateDef == this.state;
		}

		// Token: 0x04002C24 RID: 11300
		public MentalStateDef state;
	}
}
