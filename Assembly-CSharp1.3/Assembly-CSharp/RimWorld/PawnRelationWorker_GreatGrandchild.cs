using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0E RID: 3598
	public class PawnRelationWorker_GreatGrandchild : PawnRelationWorker
	{
		// Token: 0x0600533F RID: 21311 RVA: 0x001C2DA4 File Offset: 0x001C0FA4
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Grandchild.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
