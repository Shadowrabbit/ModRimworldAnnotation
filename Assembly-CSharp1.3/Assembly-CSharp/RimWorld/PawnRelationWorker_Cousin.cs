using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E05 RID: 3589
	public class PawnRelationWorker_Cousin : PawnRelationWorker
	{
		// Token: 0x06005325 RID: 21285 RVA: 0x001C2990 File Offset: 0x001C0B90
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
