using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E10 RID: 3600
	public class PawnRelationWorker_HalfSibling : PawnRelationWorker
	{
		// Token: 0x06005343 RID: 21315 RVA: 0x001C2E0C File Offset: 0x001C100C
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && !PawnRelationDefOf.Sibling.Worker.InRelation(me, other) && ((me.GetMother() != null && me.GetMother() == other.GetMother()) || (me.GetFather() != null && me.GetFather() == other.GetFather()));
		}
	}
}
