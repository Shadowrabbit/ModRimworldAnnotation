using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200149C RID: 5276
	public class PawnRelationWorker_GrandnephewOrGrandniece : PawnRelationWorker
	{
		// Token: 0x060071C0 RID: 29120 RVA: 0x0022D280 File Offset: 0x0022B480
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.NephewOrNiece.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
