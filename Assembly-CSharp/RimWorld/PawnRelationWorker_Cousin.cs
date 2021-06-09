using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001496 RID: 5270
	public class PawnRelationWorker_Cousin : PawnRelationWorker
	{
		// Token: 0x060071AC RID: 29100 RVA: 0x0022D0AC File Offset: 0x0022B2AC
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.UncleOrAunt.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
