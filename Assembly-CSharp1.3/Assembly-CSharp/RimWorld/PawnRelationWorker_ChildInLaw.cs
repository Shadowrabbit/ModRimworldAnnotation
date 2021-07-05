using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E04 RID: 3588
	public class PawnRelationWorker_ChildInLaw : PawnRelationWorker
	{
		// Token: 0x06005323 RID: 21283 RVA: 0x001C2908 File Offset: 0x001C0B08
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (other.GetSpouseCount(true) == 0)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
			if (worker.InRelation(me, other))
			{
				return false;
			}
			foreach (Pawn other2 in other.GetSpouses(true))
			{
				if (worker.InRelation(me, other2))
				{
					return true;
				}
			}
			return false;
		}
	}
}
