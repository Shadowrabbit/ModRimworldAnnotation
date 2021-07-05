using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C90 RID: 3216
	public class JobGiver_AIFightEnemies : JobGiver_AIFightEnemy
	{
		/// <summary>
		/// 寻找射击位置
		/// </summary>
		/// <param name="pawn"></param>
		/// <param name="dest"></param>
		/// <returns></returns>
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			bool allowManualCastWeapons = !pawn.IsColonist;
			Verb verb = pawn.TryGetAttackVerb(enemyTarget, allowManualCastWeapons);
			//没有攻击动作
			if (verb == null)
			{
				dest = IntVec3.Invalid;
				return false;
			}
			//寻找投射位置
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
