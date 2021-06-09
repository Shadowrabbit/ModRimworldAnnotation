using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200149F RID: 5279
	public class PawnRelationWorker_GreatGrandchild : PawnRelationWorker
	{
		// Token: 0x060071C6 RID: 29126 RVA: 0x0022D334 File Offset: 0x0022B534
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Grandchild.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
