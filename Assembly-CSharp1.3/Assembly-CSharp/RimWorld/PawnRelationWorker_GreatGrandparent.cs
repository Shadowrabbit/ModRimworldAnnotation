using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0F RID: 3599
	public class PawnRelationWorker_GreatGrandparent : PawnRelationWorker
	{
		// Token: 0x06005341 RID: 21313 RVA: 0x001C2DF3 File Offset: 0x001C0FF3
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.GreatGrandchild.Worker.InRelation(other, me);
		}
	}
}
