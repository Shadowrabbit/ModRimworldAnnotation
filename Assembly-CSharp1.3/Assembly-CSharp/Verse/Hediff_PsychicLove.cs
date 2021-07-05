using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002CE RID: 718
	public class Hediff_PsychicLove : HediffWithTarget
	{
		// Token: 0x06001378 RID: 4984 RVA: 0x0006E7AE File Offset: 0x0006C9AE
		public override void Notify_RelationAdded(Pawn otherPawn, PawnRelationDef relationDef)
		{
			if (otherPawn == this.target && (relationDef == PawnRelationDefOf.Lover || relationDef == PawnRelationDefOf.Fiance || relationDef == PawnRelationDefOf.Spouse))
			{
				this.pawn.health.RemoveHediff(this);
			}
		}
	}
}
