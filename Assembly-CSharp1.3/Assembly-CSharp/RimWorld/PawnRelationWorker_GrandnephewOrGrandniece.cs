using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0B RID: 3595
	public class PawnRelationWorker_GrandnephewOrGrandniece : PawnRelationWorker
	{
		// Token: 0x06005339 RID: 21305 RVA: 0x001C2CD8 File Offset: 0x001C0ED8
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.NephewOrNiece.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
