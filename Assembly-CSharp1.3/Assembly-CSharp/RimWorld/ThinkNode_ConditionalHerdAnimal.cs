using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000910 RID: 2320
	public class ThinkNode_ConditionalHerdAnimal : ThinkNode_Conditional
	{
		// Token: 0x06003C53 RID: 15443 RVA: 0x0014F539 File Offset: 0x0014D739
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.herdAnimal;
		}
	}
}
