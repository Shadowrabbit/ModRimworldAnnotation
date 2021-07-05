using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A1 RID: 5281
	public class PawnRelationWorker_HalfSibling : PawnRelationWorker
	{
		// Token: 0x060071CA RID: 29130 RVA: 0x0022D384 File Offset: 0x0022B584
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && !PawnRelationDefOf.Sibling.Worker.InRelation(me, other) && ((me.GetMother() != null && me.GetMother() == other.GetMother()) || (me.GetFather() != null && me.GetFather() == other.GetFather()));
		}
	}
}
