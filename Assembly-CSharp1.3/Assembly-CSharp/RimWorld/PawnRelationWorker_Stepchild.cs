using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E19 RID: 3609
	public class PawnRelationWorker_Stepchild : PawnRelationWorker
	{
		// Token: 0x06005362 RID: 21346 RVA: 0x001C3AB4 File Offset: 0x001C1CB4
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (me.GetSpouseCount(true) == 0)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
			if (worker.InRelation(me, other))
			{
				return false;
			}
			foreach (Pawn me2 in me.GetSpouses(true))
			{
				if (worker.InRelation(me2, other))
				{
					return true;
				}
			}
			return false;
		}
	}
}
