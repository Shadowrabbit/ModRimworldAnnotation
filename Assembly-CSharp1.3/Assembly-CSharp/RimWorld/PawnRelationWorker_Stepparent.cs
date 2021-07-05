using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1A RID: 3610
	public class PawnRelationWorker_Stepparent : PawnRelationWorker
	{
		// Token: 0x06005364 RID: 21348 RVA: 0x001C3B3C File Offset: 0x001C1D3C
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
			PawnRelationWorker worker = PawnRelationDefOf.Spouse.Worker;
			return (me.GetMother() != null && worker.InRelation(me.GetMother(), other)) || (me.GetFather() != null && worker.InRelation(me.GetFather(), other));
		}
	}
}
