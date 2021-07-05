using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000775 RID: 1909
	public class JobGiver_AIFightEnemies : JobGiver_AIFightEnemy
	{
		// Token: 0x060034A6 RID: 13478 RVA: 0x0012A56C File Offset: 0x0012876C
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			bool allowManualCastWeapons = !pawn.IsColonist;
			Verb verb = pawn.TryGetAttackVerb(enemyTarget, allowManualCastWeapons);
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
				maxRangeFromTarget = verb.verbProps.range,
				wantCoverFromTarget = (verb.verbProps.range > 5f)
			}, out dest);
		}
	}
}
