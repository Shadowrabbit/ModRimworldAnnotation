using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A0 RID: 5280
	public class PawnRelationWorker_GreatGrandparent : PawnRelationWorker
	{
		// Token: 0x060071C8 RID: 29128 RVA: 0x0004C825 File Offset: 0x0004AA25
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.GreatGrandchild.Worker.InRelation(other, me);
		}
	}
}
