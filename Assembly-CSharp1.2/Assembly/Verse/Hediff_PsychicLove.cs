using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003B7 RID: 951
	public class Hediff_PsychicLove : HediffWithTarget
	{
		// Token: 0x060017B0 RID: 6064 RVA: 0x00016A70 File Offset: 0x00014C70
		public override void Notify_RelationAdded(Pawn otherPawn, PawnRelationDef relationDef)
		{
			if (otherPawn == this.target && (relationDef == PawnRelationDefOf.Lover || relationDef == PawnRelationDefOf.Fiance || relationDef == PawnRelationDefOf.Spouse))
			{
				this.pawn.health.RemoveHediff(this);
			}
		}
	}
}
