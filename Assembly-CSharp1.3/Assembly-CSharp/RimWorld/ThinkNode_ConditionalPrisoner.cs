using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008FC RID: 2300
	public class ThinkNode_ConditionalPrisoner : ThinkNode_Conditional
	{
		// Token: 0x06003C29 RID: 15401 RVA: 0x0014F26E File Offset: 0x0014D46E
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
