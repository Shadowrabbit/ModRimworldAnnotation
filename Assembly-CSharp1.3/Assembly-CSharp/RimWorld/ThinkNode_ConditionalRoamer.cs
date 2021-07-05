using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000930 RID: 2352
	public class ThinkNode_ConditionalRoamer : ThinkNode_Conditional
	{
		// Token: 0x06003C97 RID: 15511 RVA: 0x0014FCCA File Offset: 0x0014DECA
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.Roamer;
		}
	}
}
