using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AA RID: 5290
	public class PawnRelationWorker_Stepchild : PawnRelationWorker
	{
		// Token: 0x060071E9 RID: 29161 RVA: 0x0022DE94 File Offset: 0x0022C094
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (me.GetSpouse() == null)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
			return !worker.InRelation(me, other) && worker.InRelation(me.GetSpouse(), other);
		}
	}
}
