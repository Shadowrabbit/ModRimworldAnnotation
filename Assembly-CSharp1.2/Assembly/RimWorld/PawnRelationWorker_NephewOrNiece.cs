using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A4 RID: 5284
	public class PawnRelationWorker_NephewOrNiece : PawnRelationWorker
	{
		// Token: 0x060071D2 RID: 29138 RVA: 0x0022D42C File Offset: 0x0022B62C
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
