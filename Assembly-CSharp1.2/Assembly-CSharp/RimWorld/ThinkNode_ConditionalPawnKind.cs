using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E73 RID: 3699
	public class ThinkNode_ConditionalPawnKind : ThinkNode_Conditional
	{
		// Token: 0x0600531F RID: 21279 RVA: 0x0003A129 File Offset: 0x00038329
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalPawnKind thinkNode_ConditionalPawnKind = (ThinkNode_ConditionalPawnKind)base.DeepCopy(resolve);
			thinkNode_ConditionalPawnKind.pawnKind = this.pawnKind;
			return thinkNode_ConditionalPawnKind;
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x0003A143 File Offset: 0x00038343
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.kindDef == this.pawnKind;
		}

		// Token: 0x04003501 RID: 13569
		public PawnKindDef pawnKind;
	}
}
