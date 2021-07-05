using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001495 RID: 5269
	public class PawnRelationWorker_ChildInLaw : PawnRelationWorker
	{
		// Token: 0x060071AA RID: 29098 RVA: 0x0022D068 File Offset: 0x0022B268
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (other.GetSpouse() == null)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
			return !worker.InRelation(me, other) && worker.InRelation(me, other.GetSpouse());
		}
	}
}
