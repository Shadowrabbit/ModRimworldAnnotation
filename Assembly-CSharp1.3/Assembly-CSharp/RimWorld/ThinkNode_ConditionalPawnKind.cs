using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000929 RID: 2345
	public class ThinkNode_ConditionalPawnKind : ThinkNode_Conditional
	{
		// Token: 0x06003C88 RID: 15496 RVA: 0x0014F9D4 File Offset: 0x0014DBD4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalPawnKind thinkNode_ConditionalPawnKind = (ThinkNode_ConditionalPawnKind)base.DeepCopy(resolve);
			thinkNode_ConditionalPawnKind.pawnKind = this.pawnKind;
			return thinkNode_ConditionalPawnKind;
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x0014F9EE File Offset: 0x0014DBEE
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.kindDef == this.pawnKind;
		}

		// Token: 0x040020B1 RID: 8369
		public PawnKindDef pawnKind;
	}
}
