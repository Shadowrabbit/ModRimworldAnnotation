using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0A RID: 3594
	public class PawnRelationWorker_Grandchild : PawnRelationWorker
	{
		// Token: 0x06005337 RID: 21303 RVA: 0x001C2C88 File Offset: 0x001C0E88
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
