using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001497 RID: 5271
	public class PawnRelationWorker_CousinOnceRemoved : PawnRelationWorker
	{
		// Token: 0x060071AE RID: 29102 RVA: 0x0022D0FC File Offset: 0x0022B2FC
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Cousin.Worker;
			if ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())))
			{
				return true;
			}
			PawnRelationWorker worker2 = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
			return (other.GetMother() != null && worker2.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker2.InRelation(me, other.GetFather()));
		}
	}
}
