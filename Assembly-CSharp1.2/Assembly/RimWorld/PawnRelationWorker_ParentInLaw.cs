using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A6 RID: 5286
	public class PawnRelationWorker_ParentInLaw : PawnRelationWorker
	{
		// Token: 0x060071D9 RID: 29145 RVA: 0x0022D6D4 File Offset: 0x0022B8D4
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
			PawnRelationWorker worker = PawnRelationDefOf.Parent.Worker;
			return !worker.InRelation(me, other) && worker.InRelation(me.GetSpouse(), other);
		}
	}
}
