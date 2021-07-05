using System;

namespace Verse.AI
{
	// Token: 0x0200061C RID: 1564
	public class ThinkNode_ConditionalMentalStateClass : ThinkNode_Conditional
	{
		// Token: 0x06002D21 RID: 11553 RVA: 0x0010F1E2 File Offset: 0x0010D3E2
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStateClass thinkNode_ConditionalMentalStateClass = (ThinkNode_ConditionalMentalStateClass)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStateClass.stateClass = this.stateClass;
			return thinkNode_ConditionalMentalStateClass;
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x0010F1FC File Offset: 0x0010D3FC
		protected override bool Satisfied(Pawn pawn)
		{
			MentalState mentalState = pawn.MentalState;
			return mentalState != null && this.stateClass.IsAssignableFrom(mentalState.GetType());
		}

		// Token: 0x04001BAE RID: 7086
		public Type stateClass;
	}
}
