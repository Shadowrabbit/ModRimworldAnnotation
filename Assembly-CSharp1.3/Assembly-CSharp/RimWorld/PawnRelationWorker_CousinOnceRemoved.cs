using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E06 RID: 3590
	public class PawnRelationWorker_CousinOnceRemoved : PawnRelationWorker
	{
		// Token: 0x06005327 RID: 21287 RVA: 0x001C29E0 File Offset: 0x001C0BE0
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
