using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A7 RID: 5287
	public class PawnRelationWorker_SecondCousin : PawnRelationWorker
	{
		// Token: 0x060071DB RID: 29147 RVA: 0x0022D718 File Offset: 0x0022B918
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
			Pawn mother = other.GetMother();
			if (mother != null && ((mother.GetMother() != null && worker.InRelation(me, mother.GetMother())) || (mother.GetFather() != null && worker.InRelation(me, mother.GetFather()))))
			{
				return true;
			}
			Pawn father = other.GetFather();
			return father != null && ((father.GetMother() != null && worker.InRelation(me, father.GetMother())) || (father.GetFather() != null && worker.InRelation(me, father.GetFather())));
		}
	}
}
