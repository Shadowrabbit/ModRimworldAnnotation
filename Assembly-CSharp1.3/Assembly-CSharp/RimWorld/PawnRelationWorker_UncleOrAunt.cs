using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1B RID: 3611
	public class PawnRelationWorker_UncleOrAunt : PawnRelationWorker
	{
		// Token: 0x06005366 RID: 21350 RVA: 0x001C3BA4 File Offset: 0x001C1DA4
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Parent.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Grandparent.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
