using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E16 RID: 3606
	public class PawnRelationWorker_SecondCousin : PawnRelationWorker
	{
		// Token: 0x06005354 RID: 21332 RVA: 0x001C3268 File Offset: 0x001C1468
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
