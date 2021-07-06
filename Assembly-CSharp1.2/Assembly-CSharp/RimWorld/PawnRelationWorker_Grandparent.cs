using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200149D RID: 5277
	public class PawnRelationWorker_Grandparent : PawnRelationWorker
	{
		// Token: 0x060071C2 RID: 29122 RVA: 0x0004C80C File Offset: 0x0004AA0C
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.Grandchild.Worker.InRelation(other, me);
		}
	}
}
