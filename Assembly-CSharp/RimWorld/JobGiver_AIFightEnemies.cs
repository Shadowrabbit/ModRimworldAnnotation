using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C90 RID: 3216
	public class JobGiver_AIFightEnemies : JobGiver_AIFightEnemy
	{
		// Token: 0x06004B00 RID: 19200 RVA: 0x001A3DCC File Offset: 0x001A1FCC
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
