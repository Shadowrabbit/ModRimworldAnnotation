using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200149B RID: 5275
	public class PawnRelationWorker_Grandchild : PawnRelationWorker
	{
		// Token: 0x060071BE RID: 29118 RVA: 0x0022D230 File Offset: 0x0022B430
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
