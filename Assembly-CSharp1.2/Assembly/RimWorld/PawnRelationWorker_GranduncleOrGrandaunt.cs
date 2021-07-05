using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200149E RID: 5278
	public class PawnRelationWorker_GranduncleOrGrandaunt : PawnRelationWorker
	{
		// Token: 0x060071C4 RID: 29124 RVA: 0x0022D2D0 File Offset: 0x0022B4D0
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Grandparent.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.GreatGrandparent.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
