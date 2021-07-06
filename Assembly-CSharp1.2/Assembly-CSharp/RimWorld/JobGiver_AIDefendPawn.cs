using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C91 RID: 3217
	public abstract class JobGiver_AIDefendPawn : JobGiver_AIFightEnemy
	{
		// Token: 0x06004B02 RID: 19202 RVA: 0x000358C2 File Offset: 0x00033AC2
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIDefendPawn jobGiver_AIDefendPawn = (JobGiver_AIDefendPawn)base.DeepCopy(resolve);
			jobGiver_AIDefendPawn.attackMeleeThreatEvenIfNotHostile = this.attackMeleeThreatEvenIfNotHostile;
			return jobGiver_AIDefendPawn;
		}

		// Token: 0x06004B03 RID: 19203
		protected abstract Pawn GetDefendee(Pawn pawn);

		// Token: 0x06004B04 RID: 19204 RVA: 0x001A3E5C File Offset: 0x001A205C
		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Pawn defendee = this.GetDefendee(pawn);
			if (defendee.Spawned || defendee.CarriedBy != null)
			{
				return defendee.PositionHeld;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x001A3E90 File Offset: 0x001A2090
		protected override Job TryGiveJob(Pawn pawn)
		{
			//被当前角色守护的目标
			Pawn defendee = this.GetDefendee(pawn);
			if (defendee == null)
			{
				Log.Error(base.GetType() + " has null defendee. pawn=" + pawn.ToStringSafe<Pawn>(), false);
				return null;
			}
			//被守护者正在被搬运中
			Pawn carriedBy = defendee.CarriedBy;
			if (carriedBy != null)
			{
				if (!pawn.CanReach(carriedBy, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return null;
				}
			}
			//无法接触被守护者
			else if (!defendee.Spawned || !pawn.CanReach(defendee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return null;
			}
			//寻找目标战斗
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06004B06 RID: 19206 RVA: 0x001A3F08 File Offset: 0x001A2108
		protected override Thing FindAttackTarget(Pawn pawn)
		{
			if (this.attackMeleeThreatEvenIfNotHostile)
			{
				Pawn defendee = this.GetDefendee(pawn);
				if (defendee.Spawned && !defendee.InMentalState && defendee.mindState.meleeThreat != null && defendee.mindState.meleeThreat != pawn && pawn.CanReach(defendee.mindState.meleeThreat, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return defendee.mindState.meleeThreat;
				}
			}
			return base.FindAttackTarget(pawn);
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x001A3F80 File Offset: 0x001A2180
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest)
		{
			Verb verb = pawn.TryGetAttackVerb(null, !pawn.IsColonist);
			if (verb == null)
			{
				dest = IntVec3.Invalid;
				return false;
			}
			return CastPositionFinder.TryFindCastPosition(new CastPositionRequest
			{
				caster = pawn,
				target = pawn.mindState.enemyTarget,
				verb = verb,
				maxRangeFromTarget = 9999f,
				locus = this.GetDefendee(pawn).PositionHeld,
				maxRangeFromLocus = this.GetFlagRadius(pawn),
				wantCoverFromTarget = (verb.verbProps.range > 7f),
				maxRegions = 50
			}, out dest);
		}

		// Token: 0x040031B1 RID: 12721
		private bool attackMeleeThreatEvenIfNotHostile;
	}
}
