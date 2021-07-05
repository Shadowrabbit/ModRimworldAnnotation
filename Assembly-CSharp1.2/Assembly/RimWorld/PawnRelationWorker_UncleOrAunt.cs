using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AC RID: 5292
	public class PawnRelationWorker_UncleOrAunt : PawnRelationWorker
	{
		// Token: 0x060071ED RID: 29165 RVA: 0x0022DF40 File Offset: 0x0022C140
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Parent.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Grandparent.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
