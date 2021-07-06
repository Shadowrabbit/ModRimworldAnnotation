using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AB RID: 5291
	public class PawnRelationWorker_Stepparent : PawnRelationWorker
	{
		// Token: 0x060071EB RID: 29163 RVA: 0x0022DED8 File Offset: 0x0022C0D8
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
