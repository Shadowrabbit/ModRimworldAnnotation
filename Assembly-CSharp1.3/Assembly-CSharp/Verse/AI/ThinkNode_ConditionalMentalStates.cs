using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200061D RID: 1565
	public class ThinkNode_ConditionalMentalStates : ThinkNode_Conditional
	{
		// Token: 0x06002D24 RID: 11556 RVA: 0x0010F226 File Offset: 0x0010D426
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStates thinkNode_ConditionalMentalStates = (ThinkNode_ConditionalMentalStates)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStates.states = this.states;
			return thinkNode_ConditionalMentalStates;
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x0010F240 File Offset: 0x0010D440
		protected override bool Satisfied(Pawn pawn)
		{
			return this.states.Contains(pawn.MentalStateDef);
		}

		// Token: 0x04001BAF RID: 7087
		public List<MentalStateDef> states;
	}
}
