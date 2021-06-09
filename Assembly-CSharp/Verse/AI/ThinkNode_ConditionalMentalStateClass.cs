using System;

namespace Verse.AI
{
	// Token: 0x02000A7B RID: 2683
	public class ThinkNode_ConditionalMentalStateClass : ThinkNode_Conditional
	{
		// Token: 0x06004002 RID: 16386 RVA: 0x0002FF2B File Offset: 0x0002E12B
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStateClass thinkNode_ConditionalMentalStateClass = (ThinkNode_ConditionalMentalStateClass)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStateClass.stateClass = this.stateClass;
			return thinkNode_ConditionalMentalStateClass;
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x00181CBC File Offset: 0x0017FEBC
		protected override bool Satisfied(Pawn pawn)
		{
			MentalState mentalState = pawn.MentalState;
			return mentalState != null && this.stateClass.IsAssignableFrom(mentalState.GetType());
		}

		// Token: 0x04002C25 RID: 11301
		public Type stateClass;
	}
}
