using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200077B RID: 1915
	public class JobGiver_AIDefendPoint : JobGiver_AIFightEnemy
	{
		// Token: 0x060034BC RID: 13500 RVA: 0x0012AB90 File Offset: 0x00128D90
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			Verb verb = pawn.TryGetAttackVerb(enemyTarget, !pawn.IsColonist);
			if (verb == null)
			{
				dest = IntVec3.Invalid;
				return false;
			}
			return CastPositionFinder.TryFindCastPosition(new CastPositionRequest
			{
				caster = pawn,
				target = enemyTarget,
				verb = verb,
				maxRangeFromTarget = 9999f,
				locus = (IntVec3)pawn.mindState.duty.focus,
				maxRangeFromLocus = pawn.mindState.duty.radius,
				wantCoverFromTarget = (verb.verbProps.range > 7f)
			}, out dest);
		}
	}
}
