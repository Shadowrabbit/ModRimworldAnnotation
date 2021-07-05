using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0C RID: 3596
	public class PawnRelationWorker_Grandparent : PawnRelationWorker
	{
		// Token: 0x0600533B RID: 21307 RVA: 0x001C2D27 File Offset: 0x001C0F27
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.Grandchild.Worker.InRelation(other, me);
		}
	}
}
