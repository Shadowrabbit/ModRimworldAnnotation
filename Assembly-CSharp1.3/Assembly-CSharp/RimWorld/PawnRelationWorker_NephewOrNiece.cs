using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E13 RID: 3603
	public class PawnRelationWorker_NephewOrNiece : PawnRelationWorker
	{
		// Token: 0x0600534B RID: 21323 RVA: 0x001C2F38 File Offset: 0x001C1138
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Child.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Sibling.Worker;
			PawnRelationWorker worker2 = PawnRelationDefOf.HalfSibling.Worker;
			return (other.GetMother() != null && (worker.InRelation(me, other.GetMother()) || worker2.InRelation(me, other.GetMother()))) || (other.GetFather() != null && (worker.InRelation(me, other.GetFather()) || worker2.InRelation(me, other.GetFather())));
		}
	}
}
