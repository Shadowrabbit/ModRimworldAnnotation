using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E15 RID: 3605
	public class PawnRelationWorker_ParentInLaw : PawnRelationWorker
	{
		// Token: 0x06005352 RID: 21330 RVA: 0x001C31E0 File Offset: 0x001C13E0
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
			PawnRelationWorker worker = PawnRelationDefOf.Parent.Worker;
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
