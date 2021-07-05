using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A7C RID: 2684
	public class ThinkNode_ConditionalMentalStates : ThinkNode_Conditional
	{
		// Token: 0x06004005 RID: 16389 RVA: 0x0002FF45 File Offset: 0x0002E145
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStates thinkNode_ConditionalMentalStates = (ThinkNode_ConditionalMentalStates)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStates.states = this.states;
			return thinkNode_ConditionalMentalStates;
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x0002FF5F File Offset: 0x0002E15F
		protected override bool Satisfied(Pawn pawn)
		{
			return this.states.Contains(pawn.MentalStateDef);
		}

		// Token: 0x04002C26 RID: 11302
		public List<MentalStateDef> states;
	}
}
